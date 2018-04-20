using System;
using System.Linq;
using System.Text;

namespace LeagueClientAPI.RemoteCalls
{
    internal class RemoteLoLReplays
    {
        public bool Enabled { get; private set; }

        Byte[] asm = { 0x55, 0x8B, 0xEC, 0x83, 0xE4, 0xF8, 0x8B, 0x55, 0x08, 0xFF, 0x72, 0x14, 0x8B, 0x4A, 0x04, 
                       0x8D, 0x42, 0x08, 0xFF, 0x72, 0x10, 0x50, 0x8B, 0x02, 0xFF, 0xD0, 0x8B, 0xE5, 0x5D, 0xC3 };

        private readonly MemoryEditor _leagueClientMemoryEditor = MemoryEditor.Instance;
        private readonly uint _functionAddress;
        private readonly uint _classAddress;

        public RemoteLoLReplays(String region)
        {
            if (!_leagueClientMemoryEditor.SetModule("rcp-be-lol-replays.dll")) return;
            uint off;
            //getDownloadUrl String:
            _leagueClientMemoryEditor.FindPattern(@"\x67\x65\x74\x44\x6F\x77\x6E\x6C\x6F\x61\x64\x55\x72\x6C", "xxxxxxxxxxxxxx", out off);
            byte[] offB = BitConverter.GetBytes(off);
            offB.Reverse();
            //Find the Push getDownloadUrl:
            _leagueClientMemoryEditor.FindPattern(String.Format(@"\x68\x{0}\x{1}\x{2}\x{3}", offB[0].ToString("X2"), offB[1].ToString("X2"), offB[2].ToString("X2"), offB[3].ToString("X2")), "xxxxx", out off);
            //Scan Backwards to find the start of the call
            _leagueClientMemoryEditor.FindPatternReverse(@"\x55\x8b\xEC\x6A\xFF", "xxxxx", (IntPtr)off, out off);
            //Store the Call
            _functionAddress = off;
            //Scan Memory for Where a vtable is located which points to the function
            offB = BitConverter.GetBytes(_functionAddress);
            _leagueClientMemoryEditor.FindPattern(String.Format(@"\x{0}\x{1}\x{2}\x{3}", offB[0].ToString("X2"), offB[1].ToString("X2"), offB[2].ToString("X2"), offB[3].ToString("X2")), "xxxx", out off);
            //Find the base of the vtable, its currently -0xc, not hard code it?
            off -= 0xC;
            offB = BitConverter.GetBytes(off);
            offB.Reverse();
            //Find the Class, Its always got the function we call in its vtable, 8 unknown bytes then the region of the player
            _leagueClientMemoryEditor.ScanAllModules(String.Format(@"\x{0}\x{1}\x{2}\x{3}\x00\x00\x00\x00\x00\x00\x00\x00" + GetRegionPattern(region),
               offB[0].ToString("X2"), offB[1].ToString("X2"), offB[2].ToString("X2"), offB[3].ToString("X2")), "xxxx????????" + new String('x', region.Length), out off);
            _classAddress = off;
            Enabled = true;
        }

        public String GetRegionPattern(String region)
        {
            byte[] regionBytes = Encoding.UTF8.GetBytes(region);
            string pattern = "";
            foreach (byte b in regionBytes)
            {
                pattern += @"\x" + b.ToString("X2");
            }
            return pattern;
        }

        public String fetchRoflDownloadUrl(UInt64 matchId)
        {
            IntPtr funcAddr = _leagueClientMemoryEditor.AllocateMemory(asm.Length);
            //Structs not packed so 0x18 instead of 0x14 and 0xC is skipped
            IntPtr paramsAddr = _leagueClientMemoryEditor.AllocateMemory(0x18);
            //Write our custom function
            _leagueClientMemoryEditor.WriteBytes(funcAddr, asm);
            //Function Address is the address which our custom asm calls
            _leagueClientMemoryEditor.WriteUInt32(paramsAddr, _functionAddress);
            //Class Address is ECX, a class which is initialized which gets passed in to above function
            _leagueClientMemoryEditor.WriteUInt32(paramsAddr + 0x4, _classAddress);
            //Successful call will place a ptr to the download link here
            _leagueClientMemoryEditor.WriteUInt32(paramsAddr + 0x8, 0);
            //+16 (0x10) not +12 (0xC) because it wasnt packed, MatchID is a MatchID
            _leagueClientMemoryEditor.WriteUInt64(paramsAddr + 0x10, matchId);
            //Call Func and wait for completion
            _leagueClientMemoryEditor.WaitForSingleObject(_leagueClientMemoryEditor.CreateRemoteThread(funcAddr, paramsAddr)); 
            IntPtr downloadUrlIntPtr;
            //Read the download Ptr
            _leagueClientMemoryEditor.ReadIntPtr(paramsAddr + 0x8, out downloadUrlIntPtr);
            String downloadUrl = "";
            //If its null then call failed, Match Might not exist, User might not be logged in etc
            if (downloadUrlIntPtr != IntPtr.Zero)
                _leagueClientMemoryEditor.ReadNullTerminatedString(downloadUrlIntPtr, out downloadUrl);
            //Free Memory of the ones we allocated and also the injected function
            //TODO: Get LeagueClient to free the memory of the download URL though this is minor, though still a memory leak
            //TODO: Potentially just allocated and reuse memory once?
            _leagueClientMemoryEditor.FreeMemory(funcAddr);
            _leagueClientMemoryEditor.FreeMemory(paramsAddr);
            return downloadUrl;
        }
    }
}
