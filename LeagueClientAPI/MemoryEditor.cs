using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

public enum FindGameResult
{
    GameNotFound,
    GameFound,
    OpenProcessIssue,
}

namespace LeagueClientAPI
{
    //Gutted Memory Editor from the Creator Suite
    //TODO: Cleanup
    internal sealed class MemoryEditor : IDisposable
    {
        #region Process Access Consts
        const int ProcessCreateThread = 0x0002;
        const int ProcessWmRead = 0x0010;
        const int ProcessVmWrite = 0x0020;
        const int ProcessQueryInformation = 0x0400;
        const int ProcessVmOperation = 0x0008;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;

        public enum Protection : uint
        {
            PageNoaccess = 0x01,
            PageReadonly = 0x02,
            PageReadwrite = 0x04,
            PageWritecopy = 0x08,
            PageExecute = 0x10,
            PageExecuteRead = 0x20,
            PageExecuteReadwrite = 0x40,
            PageExecuteWritecopy = 0x80,
            PageGuard = 0x100,
            PageNocache = 0x200,
        }

        [Flags]
        public enum MemoryAllocation : uint
        {
            MemCommit = 0x1000,
            MemReserve = 0x2000,
            MemDecommit = 0x4000,
            MemRelease = 0x8000,
            MemFree = 0x10000,
            MemPrivate = 0x20000,
            MemMapped = 0x40000,
            MemReset = 0x80000,
            MemTopDown = 0x100000,
            MemWriteWatch = 0x200000,
            MemPhysical = 0x400000,
            MemRotate = 0x800000,
            MemDifferentImageBaseOk = 0x800000,
            MemResetUndo = 0x1000000,
            MemLargePages = 0x20000000,
            Mem4MbPages = 0x80000000
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            public uint ContextFlags;
            public uint Dr0;
            public uint Dr1;
            public uint Dr2;
            public uint Dr3;
            public uint Dr6;
            public uint Dr7;
            public FloatingSaveArea FloatSave;
            public uint SegGs;
            public uint SegFs;
            public uint SegEs;
            public uint SegDs;
            public uint Edi;
            public uint Esi;
            public uint Ebx;
            public uint Edx;
            public uint Ecx;
            public uint Eax;
            public uint Ebp;
            public uint Eip;
            public uint SegCs;
            public uint EFlags;
            public uint Esp;
            public uint SegSs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] ExtendedRegisters;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FloatingSaveArea
        {
            public uint ControlWord;
            public uint StatusWord;
            public uint TagWord;
            public uint ErrorOffset;
            public uint ErrorSelector;
            public uint DataOffset;
            public uint DataSelector;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] RegisterArea;

            public uint Cr0NpxState;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PROCESS_BASIC_INFORMATION
        {
            public int ExitStatus;
            public int PebBaseAddress;
            public int AffinityMask;
            public int BasePriority;
            public int UniqueProcessId;
            public int InheritedFromUniqueProcessId;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct PEB
        {
            //public fixed uint Filler[4];
            public uint Filler0; //Reserved1[2], debug, reversed2[1]
            public uint Filler1;
            public uint Filler2;
            public uint Filler3;
            public uint InfoBlockAddress;
        };

        [StructLayout(LayoutKind.Sequential)]
        struct INFOBLOCK
        {
            public uint Filler0; //4
            public uint Filler1; //8
            public uint Filler2; //c
            public uint Filler3; //10
            public uint Filler4; //14
            public uint Filler5; //18
            public uint Filler6; //1c
            public uint Filler7; //20
            public uint Filler8; //24
            public uint Filler9; //28
            public uint Filler10;//2c
            public uint Filler11;//30
            public uint Filler12;//34
            public IntPtr EnvironmentPaths;
            public uint Filler13;
            public IntPtr imageAddress;
            public uint Filler14;
            public IntPtr commandLineAddress;
            public uint Filler15;
        };

        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }
        #endregion

        #region Imports
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(int hProcess,
          int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
          byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool GetThreadContext(IntPtr hThread, ref Context lpContext);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, MemoryAllocation flAllocationType, Protection flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFree(IntPtr lpAddress, IntPtr dwSize, MemoryAllocation freeType);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess,
        IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("ntdll.dll")]
        static extern int NtQueryInformationProcess(
            IntPtr ProcessHandle,
            int ProcessInformationClass,
            out PROCESS_BASIC_INFORMATION ProcessInformation,
            int ProcessInformationLength,
            out int ReturnLength
            );
        #endregion

        #region Public & Private Vars
        private static readonly MemoryEditor _instance = new MemoryEditor();
        private bool _gameFound = false;

        public IntPtr ProcessHandle
        {
            get { return _processHandle; }
        }

        private IntPtr _processHandle;
        private IntPtr _baseModule;

        private int _moduleSize;
        public bool Unlocked { get; private set; }

        public static MemoryEditor Instance
        {
            get { return _instance; }
        }

        private EventHandler _processExitedHandler;
        public EventHandler ProcessExitedHandler
        {
            get { return _processExitedHandler; }
            set
            {
                if (_processExitedHandler == null)
                    _processExitedHandler = value;
            }
        }

        public void Unlock()
        {
            Unlocked = true;
        }

        #endregion

        #region Game Closed Callback

        private void ProcessExited(object sender, EventArgs e)
        {
            _gameFound = false;
            Unlocked = false;
            _processHandle = IntPtr.Zero;
            pGame = null;
            if (_processExitedHandler != null)
            {
                _processExitedHandler(sender, e);
            }
        }
        #endregion

        #region FindGame
        public Process pGame = null;
        public FindGameResult FindGame(string gameName, out String error, bool findUsingTitle = true, bool useContains = false)
        {

            error = "";
            if (findUsingTitle)
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    //Sleep to stop Insane CPU Usage:
                    String titleName = process.MainWindowTitle;
                    Thread.Sleep(1);
                    if (String.CompareOrdinal(titleName, gameName) == 0 || (useContains && titleName.Contains(gameName)))
                    {
                        pGame = process;
                        break;
                    }
                }
            }
            else
            {
                Process[] processes = Process.GetProcessesByName(gameName);
                if (processes.Count() != 0)
                {
                    pGame = processes[0];
                }
            }

            if (pGame != null)
            {
                try
                {
                    _processHandle =
                        OpenProcess(ProcessQueryInformation | ProcessCreateThread | ProcessWmRead | ProcessVmOperation | ProcessVmWrite, false,
                            pGame.Id);

                    //TODO: double check this
                    if (pGame.ProcessName == "dwm")
                        return FindGameResult.GameNotFound;

                    _baseModule = pGame.MainModule.BaseAddress;
                    _moduleSize = pGame.MainModule.ModuleMemorySize;
                    pGame.EnableRaisingEvents = true;
                    pGame.Exited += ProcessExited;
                    _gameFound = true;
                    return FindGameResult.GameFound;
                }
                catch (Exception e)
                {
                    error = FlattenException(e);
                    return FindGameResult.OpenProcessIssue;
                }
            }

            return FindGameResult.GameNotFound;
        }

        public bool SetModule(String moduleName)
        {
            foreach (ProcessModule module in pGame.Modules)
            {
                if (moduleName == module.ModuleName)
                {
                    _baseModule = module.BaseAddress;
                    _moduleSize = module.ModuleMemorySize;
                    return true;
                }
            }

            return false;
        }

        public bool ScanAllModules(String pattern, String mask, out uint ptr)
        {
            IntPtr oldBase = _baseModule;
            int oldSize = _moduleSize;
            ptr = 0;

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;
            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory

            bool result = false;

            while (proc_min_address_l < proc_max_address_l)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(_processHandle, proc_min_address, out mem_basic_info, 28);

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    _baseModule = (IntPtr)mem_basic_info.BaseAddress;
                    _moduleSize = mem_basic_info.RegionSize;
                    result = FindPattern(pattern, mask, out ptr);
                    if (result)
                        break;
                }

                // move to the next memory chunk
                proc_min_address_l += mem_basic_info.RegionSize;
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            _baseModule = oldBase;
            _moduleSize = oldSize;
            return result;
        }

        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region ReadMemory Funcs

        public bool ReadIntPtr(IntPtr address, out IntPtr val)
        {
            Byte[] buffer;
            bool result = ReadBytes(address, sizeof(Int32), out buffer);
            val = (IntPtr)BitConverter.ToInt32(buffer, 0);
            return result;
        }

        public bool ReadByte(IntPtr address, out Byte val)
        {
            Byte[] buffer;
            bool result = ReadBytes(address, sizeof(Byte), out buffer);
            val = buffer[0];
            return result;
        }

        public bool ReadBytes(IntPtr address, int size, out Byte[] buffer)
        {
            int bytesRead = 0;
            buffer = new byte[size];
            return ReadProcessMemory((int)_processHandle, (int)address, buffer, buffer.Length, ref bytesRead);
        }

        public bool ReadNullTerminatedString(IntPtr address, out String str)
        {
            str = "";
            bool result = true;
            while (result)
            {
                Byte s;
                result = ReadByte(address, out s);
                if (s == 0)
                    break;
                str += Convert.ToChar(s);
                address += 0x1;
            }
            return result;
        }
        #endregion

        #region WriteMemory Funcs

        public bool WriteUInt32(IntPtr address, UInt32 value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public bool WriteUInt64(IntPtr address, UInt64 value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public bool WriteBytes(IntPtr address, byte[] buffer)
        {
            int bytesWritten = 0;
            return WriteProcessMemory((int)_processHandle, (int)address, buffer, buffer.Length, ref bytesWritten);
        }

        #endregion

        #region Memory Allocation And Execution

        public IntPtr AllocateMemory(int size)
        {
            IntPtr allocatedAddress = VirtualAllocEx(_processHandle, IntPtr.Zero, size, MemoryAllocation.MemReserve | MemoryAllocation.MemCommit, Protection.PageExecuteReadwrite);
            return allocatedAddress;
        }

        public bool FreeMemory(IntPtr addr)
        {
            Boolean result = VirtualFree(addr, (IntPtr)0, MemoryAllocation.MemRelease);
            return result;
        }

        public IntPtr CreateRemoteThread(IntPtr addr, IntPtr dataAddr)
        {
            uint threadId;
            return CreateRemoteThread(_processHandle, IntPtr.Zero, 0, addr, dataAddr, 0, out threadId);
        }

        public UInt32 WaitForSingleObject(IntPtr hHandle)
        {
            return WaitForSingleObject(hHandle, 0xFFFFFFFF);
        }

        #endregion

        #region Pattern Scanning
        public bool FindPattern(string pattern, string mask, out uint val)
        {
            byte[] data;
            bool result = ReadBytes(_baseModule, _moduleSize, out data);
            byte[] patternBytes = GetBytesFromPattern(pattern);
            uint offset = Find(data, mask, patternBytes);
            if (offset == 0)
            {
                result = false;
                val = 0;
            }
            else
                val = (uint)_baseModule + offset;
            return result;
        }

        public bool FindPatternReverse(string pattern, string mask, IntPtr startAddress, out uint val)
        {
            if ((int)startAddress < (int)_baseModule)
                startAddress = _baseModule;

            byte[] data;
            bool result = ReadBytes(_baseModule, ((int)startAddress - (int)_baseModule), out data);
            byte[] patternBytes = GetBytesFromPattern(pattern);
            uint offset = FindReverse(data, mask, patternBytes);
            if (offset == 0)
            {
                result = false;
                val = 0;
            }
            else
                val = (uint)_baseModule + offset;
            return result;
        }

        public bool FindPattern(string pattern, string mask, IntPtr startAddress, out uint val)
        {
            if ((int)startAddress < (int)_baseModule)
                startAddress = _baseModule;

            byte[] data;
            bool result = ReadBytes(startAddress, _moduleSize - ((int)startAddress - (int)_baseModule), out data);
            byte[] patternBytes = GetBytesFromPattern(pattern);
            uint offset = Find(data, mask, patternBytes);
            if (offset == 0)
            {
                result = false;
                val = 0;
            }
            else
                val = (uint)startAddress + offset;
            return result;
        }

        private static byte[] GetBytesFromPattern(string pattern)
        {
            string[] split = pattern.Split(new[] { '\\', 'x' }, StringSplitOptions.RemoveEmptyEntries);
            var ret = new byte[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                ret[i] = byte.Parse(split[i], NumberStyles.HexNumber);
            }
            return ret;
        }

        private static uint Find(byte[] data, string mask, byte[] byteMask)
        {
            for (uint i = 0; i < data.Length; i++)
            {
                //Slow down the find but improve cpu performance:
                //if (i % 25000 == 0)
                //    Thread.Sleep(1);

                if (DataCompare(data, (int)i, byteMask, mask))
                    return i;
            }
            return 0;
        }

        private static uint FindReverse(byte[] data, string mask, byte[] byteMask)
        {
            for (uint i = (uint)(data.Length - mask.Length); i > 0; i--)
            {
                //Slow down the find but improve cpu performance:
                //if (i % 25000 == 0)
                //    Thread.Sleep(1);

                if (DataCompare(data, (int)i, byteMask, mask))
                    return i;
            }
            return 0;
        }

        private static bool DataCompare(byte[] data, int offset, byte[] byteMask, string mask)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'x' && byteMask[i] != data[i + offset])
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region CommandLine
        public string GetCommandLine()
        {
            PROCESS_BASIC_INFORMATION pbi;
            int size;
            NtQueryInformationProcess(pGame.Handle, 0, out pbi, Marshal.SizeOf(typeof(PROCESS_BASIC_INFORMATION)), out size);
            IntPtr pebAddress = (IntPtr)pbi.PebBaseAddress;
            String commandLine = null;
            if ((uint)pebAddress != 0)
            {
                byte[] PEBData;
                ReadBytes(pebAddress, Marshal.SizeOf(typeof(PEB)), out PEBData);
                PEB peb = ByteArrayToStructure<PEB>(PEBData);
                IntPtr infoblkAddr = (IntPtr)peb.InfoBlockAddress;

                byte[] data;
                ReadBytes(infoblkAddr, Marshal.SizeOf(typeof(INFOBLOCK)) * 100, out data);
                INFOBLOCK infoBlock = ByteArrayToStructure<INFOBLOCK>(data);

                byte[] commandlineString;
                ReadBytes(infoBlock.commandLineAddress, 0x1000, out commandlineString);
                commandLine = Encoding.Unicode.GetString(commandlineString);
                try
                {
                    commandLine = commandLine.Substring(0, commandLine.IndexOf("\0"));
                }
                catch
                {
                    commandLine = "";
                }
            }

            return commandLine;
        }

        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return structure;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
        }
        #endregion
    }
}
