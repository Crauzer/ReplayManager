using System;
using System.Collections.Generic;
using System.IO;

namespace ReplayManager.ROFL
{
    public class Replay
    {
        private string m_path;
        private Stream m_stream;
        private ReplayHeader m_header;
        private List<ReplayPayloadEntry> m_payloadEntryList;
        private ReplayPayloadHeader m_payloadHeader;
        private int m_currentEntry;
        private int m_entryDataOffset;
        public readonly bool Initialized;
        private bool skinsPopulated;
        #region Methods

        public Replay(string p_path, bool populateSkinIds = false)
        {
            skinsPopulated = populateSkinIds;
            // save the file's absolute path
            m_path = p_path;

            // instanciate a binary file stream
            try
            {
                m_stream = File.Open(m_path, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                Initialized = false;
                return;
            }

            Initialized = LoadReplay(populateSkinIds);
        }

        public Replay(byte[] bytes, bool populateSkinIds = false)
        {
            // instanciate a binary file stream
            try
            {
                m_stream = new MemoryStream(bytes);
            }
            catch
            {
                Initialized = false;
                return;
            }

            Initialized = LoadReplay(populateSkinIds);
        }

        private bool LoadReplay(bool populateSkinIds)
        {
            // instanciate the replay file header
            m_header = new ReplayHeader(m_stream);

            // instanciate the replay file payload header
            m_payloadHeader = new ReplayPayloadHeader(m_stream);

            // set state vars
            m_currentEntry = 0;
            m_entryDataOffset = m_header.PayloadOffset + (17 * (m_payloadHeader.ChunkCount + m_payloadHeader.KeyframeCount));
            bool loadedSkins = false;
            m_payloadEntryList = new List<ReplayPayloadEntry>();
            while (populateSkinIds && ReadEntry())
            {
                if (!loadedSkins)
                {
                    ReplayPayloadEntry payload = m_payloadEntryList[m_payloadEntryList.Count - 1];
                    if (payload.Type == ReplayPayloadEntryType.Keyframe)
                    {
                        //Loop through all champs and create an index for them
                        SortedList<int, string> sortedPlayerList = new SortedList<int, string>();
                        Dictionary<String, String> playerList = new Dictionary<string, string>();
                        Dictionary<String, int> playerSkins = new Dictionary<string, int>();

                        foreach (PlayerStats player in m_header.Metadata.playerStats)
                        {
                            playerList.Add(player.NAME, player.SKIN);
                            int position = payload.FindName(player.NAME, 0);

                            if (position == -1)
                                break;

                            sortedPlayerList.Add(payload.FindName(player.NAME, 0), player.NAME);
                        }

                        if (sortedPlayerList.Count == 0)
                            continue;

                        int StartIndex = 0;
                        foreach (string player in sortedPlayerList.Values)
                        {
                            String Champion = playerList[player];
                            int SkinID;
                            StartIndex = payload.FindSkinId(Champion, out SkinID, StartIndex);
                            playerSkins.Add(player, SkinID);
                        }

                        foreach (PlayerStats player in m_header.Metadata.playerStats)
                            player.SKINID = playerSkins[player.NAME];

                        //Loop through that and then populate the skin ids
                        loadedSkins = true;
                    }
                }
            }
            m_stream.Close();
            return true;
        }

        public bool Save(string filepath, bool censorNames = false)
        {
            BinaryWriter _writer = new BinaryWriter(File.Create(filepath));
            //Save Header
            m_header.Save(_writer);
            //Save Payload Header
            m_payloadHeader.Save(_writer);
            //Save All Chunk/Keyframe Data
            foreach (ReplayPayloadEntry payload in m_payloadEntryList)
                payload.SaveInfo(_writer);

            if (censorNames)
                foreach (PlayerStats player in m_header.Metadata.playerStats)
                    foreach (ReplayPayloadEntry payload in m_payloadEntryList)
                        payload.CensorNames(player.NAME);

            foreach (ReplayPayloadEntry payload in m_payloadEntryList)
                payload.SaveData(_writer, this);

            //Correct File Sizes
            _writer.Seek(m_header.PayloadOffset, SeekOrigin.Begin);
            foreach (ReplayPayloadEntry payload in m_payloadEntryList)
                payload.SaveInfo(_writer);

            m_header.UpdateLength(_writer);
            _writer.Close();
            return false;
        }

        public bool SaveToCustomReplay(string filepath)
        {
            try
            {
                BinaryWriter _writer = new BinaryWriter(File.Create(filepath));

                _writer.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ReadEntry()
        {
            // make sure we have no read beyond the bounds of the entry data
            if (m_currentEntry < (m_payloadHeader.ChunkCount + m_payloadHeader.KeyframeCount))
            {
                // seek to this entry's starting offset
                m_stream.Seek(m_header.PayloadOffset + (17 * m_currentEntry), SeekOrigin.Begin);

                // read out the payload entry
                m_payloadEntryList.Add(new ReplayPayloadEntry(this, m_stream, m_entryDataOffset));

                // set the current entry index
                m_currentEntry++;
                //File.WriteAllBytes("C:/Users/Dec/Desktop/Data/" + m_currentEntry.ToString() + ".dat", m_payloadEntryList[m_payloadEntryList.Count - 1].Data);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("<Replay file={0}>", Path.GetFileName(m_path));
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get
            {
                return m_path;
            }
        }

        public ReplayHeader Header
        {
            get
            {
                return m_header;
            }
        }

        public ReplayPayloadHeader PayloadHeader
        {
            get
            {
                return m_payloadHeader;
            }
        }

        public List<ReplayPayloadEntry> PayloadEntryList
        {
            get
            {
                return m_payloadEntryList;
            }
        }

        #endregion
    }
}