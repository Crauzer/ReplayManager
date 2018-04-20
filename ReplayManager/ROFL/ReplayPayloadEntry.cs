using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ReplayManager.ROFL
{
    public enum ReplayPayloadEntryType
    {
        Chunk,
        Keyframe,
        Unknown
    }

    public class ReplayPayloadEntry
    {
        private int m_id;
        private int m_length;
        private int m_nextChunkId;
        private int m_offset;
        private byte m_type;
        private byte[] m_data;
        #region Methods

        public ReplayPayloadEntry(Replay p_replay, Stream p_stream, int p_payloadDataStartOffset)
        {
            using (BinaryReader r = new BinaryReader(p_stream, Encoding.UTF8, true))
            {
                m_id = r.ReadInt32();
                m_type = r.ReadByte();
                m_length = r.ReadInt32();
                m_nextChunkId = r.ReadInt32();
                m_offset = r.ReadInt32();
            }

            // seek to the entry's data location
            p_stream.Seek(p_payloadDataStartOffset + m_offset, SeekOrigin.Begin);

            // init the byte array to appropriate length
            m_data = new byte[m_length];

            // the entry data chunk
            p_stream.Read(m_data, 0, m_length);

            // store the decrypted data
            m_data = GetDecryptedData(p_replay, m_data);
        }

        private byte[] GetDecryptedData(Replay p_replay, byte[] p_data)
        {
            // string represenation of the game id
            string gameId = Convert.ToString(p_replay.PayloadHeader.GameId);

            // obtaining the chunk encryption key
            byte[] chunkEncryptionKey = DepadBytes(DecryptBytes(Encoding.UTF8.GetBytes(gameId), p_replay.PayloadHeader.EncryptionKey));

            // obtaining the decrypted chunk
            byte[] decryptedChunk = DepadBytes(DecryptBytes(chunkEncryptionKey, p_data));

            return DecompressBytes(decryptedChunk);
        }

        /// <summary>
        /// http://tools.ietf.org/html/rfc2898
        /// </summary>
        private byte[] DepadBytes(byte[] p_data)
        {
            int paddingLength = Convert.ToInt32(p_data[p_data.Length - 1]);
            return p_data.Take(p_data.Length - paddingLength).ToArray();
        }

        private byte[] PadBytes(byte[] p_data)
        {
            int paddingLen = 8 - (p_data.Length % 8);

            Byte[] padding = new Byte[paddingLen];

            for (int i = 0; i < padding.Length; i++)
            {
                padding[i] = (Byte)(paddingLen);
            }

            Byte[] data_with_padding = new byte[p_data.Length + paddingLen];
            Buffer.BlockCopy(p_data, 0, data_with_padding, 0, p_data.Length);
            Buffer.BlockCopy(padding, 0, data_with_padding, p_data.Length, paddingLen);
            return data_with_padding;
        }

        public static void Populate<T>(T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        private byte[] DecryptBytes(byte[] p_key, byte[] p_data)
        {
            BlowFish _blowfish = new BlowFish(p_key);
            return _blowfish.Decrypt_ECB(p_data);
        }

        private byte[] EncryptBytes(byte[] p_key, byte[] p_data)
        {
            BlowFish _blowfish = new BlowFish(p_key);
            return _blowfish.Encrypt_ECB(p_data);
        }

        private byte[] DecompressBytes(byte[] p_data)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(p_data), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];

                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;

                    do
                    {
                        count = stream.Read(buffer, 0, size);

                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);

                    return memory.ToArray();
                }
            }
        }

        private byte[] CompressBytes(byte[] p_data)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream stream = new GZipStream(memory, CompressionLevel.Optimal))
                {
                    stream.Write(p_data, 0, p_data.Length);
                }
                return memory.ToArray();
            }
        }

        public void SaveInfo(BinaryWriter _writer)
        {
            _writer.Write(m_id);
            _writer.Write(m_type);
            _writer.Write(m_length);
            _writer.Write(m_nextChunkId);
            _writer.Write(m_offset); //Needs Adjusting like all previous ones
        }

        public void FindAndReplaceName(String NameToFind, String ReplacementName)
        {
            //m_data = ReplaceBytes(m_data, Encoding.UTF8.GetBytes("SkinSpotlightsYT"), Encoding.UTF8.GetBytes("SpotlightsSkinTV")) ?? m_data;
            byte[] bNameToFind = Encoding.UTF8.GetBytes(NameToFind);
            byte[] bNameToReplace = Encoding.UTF8.GetBytes(ReplacementName);

            int index = FindBytes(m_data, bNameToFind);
            while (index != -1)
            {
                byte[] chunk1 = new byte[index - 4];
                Buffer.BlockCopy(m_data, 0, chunk1, 0, chunk1.Length);

                int chunk2offset = (index + NameToFind.Length);
                byte[] chunk2 = new byte[m_data.Length - chunk2offset];
                Buffer.BlockCopy(m_data, chunk2offset, chunk2, 0, chunk2.Length);

                byte[] newDataChunk = new byte[chunk2.Length + chunk1.Length + ReplacementName.Length + 4];

                Buffer.BlockCopy(chunk1, 0, newDataChunk, 0, chunk1.Length);
                Buffer.BlockCopy(BitConverter.GetBytes(bNameToReplace.Length), 0, newDataChunk, chunk1.Length, 4);
                Buffer.BlockCopy(bNameToReplace, 0, newDataChunk, chunk1.Length + 4, bNameToReplace.Length);
                Buffer.BlockCopy(chunk2, 0, newDataChunk, chunk1.Length + 4 + bNameToReplace.Length, chunk2.Length);
                m_data = newDataChunk;
                index = FindBytes(m_data, bNameToFind, index);
            }
        }

        public int FindName(String NameToFind, int startIndex = 0)
        {
            return FindBytes(m_data, Encoding.UTF8.GetBytes(NameToFind), startIndex);
        }

        public int FindSkinId(String NameToFind, out int SkinId, int startIndex = 0)
        {
            int index = FindBytes(m_data, Encoding.UTF8.GetBytes(NameToFind), startIndex);
            SkinId = m_data[index - 8];
            index++;
            return index;
        }

        public void SaveData(BinaryWriter _writer, Replay p_replay)
        {
            byte[] encryptedBytes = EncryptReplayData(p_replay);
            m_length = encryptedBytes.Length;
            m_offset = (int)_writer.BaseStream.Position - (p_replay.Header.PayloadOffset + 17 * (p_replay.PayloadHeader.ChunkCount + p_replay.PayloadHeader.KeyframeCount));
            _writer.Write(encryptedBytes);
        }

        public void CensorNames(String name)
        {
            //Due to packet constraints have to have it at same length:
            FindAndReplaceName(name, new String('?', name.Length));
        }

        private Byte[] EncryptReplayData(Replay p_replay)
        {
            string gameId = Convert.ToString(p_replay.PayloadHeader.GameId);
            // obtaining the chunk encryption key
            byte[] chunkEncryptionKey = DepadBytes(DecryptBytes(Encoding.UTF8.GetBytes(gameId), p_replay.PayloadHeader.EncryptionKey));
            byte[] compressedData = CompressBytes(m_data);
            return EncryptBytes(chunkEncryptionKey, PadBytes(compressedData));
        }

        public override string ToString()
        {
            return string.Format("<ReplayPayloadEntry id={0} type={1} len={2} next={3}>", m_id, Type, m_length, m_nextChunkId);
        }

        #endregion

        #region Properties

        public byte[] Data
        {
            get
            {
                return m_data;
            }
        }

        public int ID
        {
            get
            {
                return m_id;
            }
        }

        public ReplayPayloadEntryType Type
        {
            get
            {
                if (m_type == 1)
                {
                    return ReplayPayloadEntryType.Chunk;
                }
                else if (m_type == 2)
                {
                    return ReplayPayloadEntryType.Keyframe;
                }

                return ReplayPayloadEntryType.Unknown;
            }
        }

        public int Length
        {
            get
            {
                return m_length;
            }
        }

        public int Offset
        {
            get
            {
                return m_offset;
            }
        }

        public int NextChunkID
        {
            get
            {
                return m_nextChunkId;
            }
        }

        public int FindBytes(byte[] src, byte[] find, int startIndex = 0)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = startIndex; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }

        #endregion
    }
}