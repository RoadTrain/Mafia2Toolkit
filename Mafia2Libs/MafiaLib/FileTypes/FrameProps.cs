﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils.StringHelpers;

namespace ResourceTypes.Misc
{
    public class FrameProps
    {
        public class FrameInfo
        {
            private ulong hash;
            private byte numInts;
            private ushort[] integers;
            private string[] data;

            public ulong Hash {
                get { return hash; }
                set { hash = value; }
            }
            public byte NumIntegers {
                get { return numInts; }
                set { numInts = value; }
            }
            public ushort[] Integers {
                get { return integers; }
                set { integers = value; }
            }
            public string[] Data {
                get { return data; }
                set { data = value; }
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", Hash, NumIntegers);
            }
        }

        public class FrameInfoChunk
        {
            private ushort size;
            private FrameInfo[] frameInfos;

            public ushort Size {
                get { return size; }
                set { size = value; }
            }
            public FrameInfo[] FrameInfos {
                get { return frameInfos; }
                set { frameInfos = value; }
            }
            public ushort SizeInBytes {
                get;
                set;
            }

            public override string ToString()
            {
                return string.Format("Size {0} SizeInBytes: {1}", size, SizeInBytes);
            }
        }

        private const int Signature = 1718775152;
        private const int Version = 3;
        private int[] unks;
        private uint[] propertiesIndexes;
        private string[] properties;
        private ulong[] actorHashes;
        private uint[] unks5;
        private FrameInfoChunk[] frameInfos;

        private Dictionary<ulong, FrameInfo> frameExtraData;

        public Dictionary<ulong, FrameInfo> FrameExtraData {
            get { return frameExtraData; }
            set { frameExtraData = value; }
        }



        public FrameProps(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != Signature)
            {
                return;
            }

            if (reader.ReadInt32() != Version)
            {
                return;
            }

            unks = new int[5];
            for (int i = 0; i != 5; i++)
            {
                unks[i] = reader.ReadInt32();
            }

            propertiesIndexes = new uint[unks[0]];
            properties = new string[unks[0]];
            actorHashes = new ulong[unks[2]];
            unks5 = new uint[unks[2]];
            frameInfos = new FrameInfoChunk[unks[2]];
            frameExtraData = new Dictionary<ulong, FrameInfo>();

            for (int i = 0; i != propertiesIndexes.Length; i++)
            {
                propertiesIndexes[i] = reader.ReadUInt32();
            }

            for (int i = 0; i != properties.Length; i++)
            {
                properties[i] = StringHelpers.ReadString(reader);
            }

            for (int i = 0; i != actorHashes.Length; i++)
            {
                actorHashes[i] = reader.ReadUInt64();
            }

            for (int i = 0; i != unks5.Length; i++)
            {
                unks5[i] = reader.ReadUInt32();
            }

            for(int i = 0; i < frameInfos.Length; i++)
            {
                FrameInfoChunk chunk = new FrameInfoChunk();
                chunk.Size = reader.ReadUInt16();
                chunk.FrameInfos = new FrameInfo[chunk.Size];

                for(int y = 0; y < chunk.Size; y++)
                {
                    FrameInfo info = new FrameInfo();
                    info.Hash = reader.ReadUInt64();
                    info.NumIntegers = reader.ReadByte();                 
                    chunk.FrameInfos[y] = info;
                }

                for (int y = 0; y < chunk.Size; y++)
                {
                    FrameInfo info = chunk.FrameInfos[y];
                    info.Integers = new ushort[info.NumIntegers];
                    for(int x = 0; x < info.NumIntegers; x++)
                    {
                        info.Integers[x] = reader.ReadUInt16();
                    }

                    if (!frameExtraData.ContainsKey(info.Hash))
                    {
                        frameExtraData.Add(info.Hash, info);
                        info.Data = new string[info.NumIntegers];

                        for (int z = 0; z < info.NumIntegers; z++)
                        {
                            info.Data[z] = properties[info.Integers[z]];
                        }
                    }
                }
                frameInfos[i] = chunk;
            }

            WriteToText();
        }

        public void WriteToFile(string name)
        {

        }
        public void WriteToFile(MemoryStream stream)
        {

        }

        private void RebuildStringSection(ref string final)
        {
            StringBuilder builder = new StringBuilder();
            uint[] newIndexes = new uint[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                builder.AppendLine(properties[i]);
                newIndexes[i] = (uint)builder.Length;
            }
            final = builder.ToString();
            propertiesIndexes = newIndexes;
        }

        private void WriteToText()
        {
            List<string> file = new List<string>();
            for (int i = 0; i != unks.Length; i++)
            {
                file.Add(i + " " + unks[i].ToString());
            }
            //file.Add("");
            //for (int i = 0; i != unks2.Length; i++)
            //{
            //    file.Add(i + " " + unks2[i].ToString());
            //}
            file.Add("");
            for (int i = 0; i != properties.Length; i++)
            {
                file.Add(i + " " + properties[i].ToString());
            }
            file.Add("");
            for (int i = 0; i != actorHashes.Length; i++)
            {
                file.Add(i + " " + actorHashes[i].ToString());
            }
            //file.Add("");
            //for (int i = 0; i != unks5.Length; i++)
            //{
            //    file.Add(i + " " + unks5[i].ToString());
            //}
            file.AddRange(new string[] { "", "FrameInfoChunk Structures:" });
            for(int i = 0; i < frameInfos.Length; i++)
            {
                file.Add("Index " + i);
                FrameInfoChunk chunk = frameInfos[i];

                file.AddRange(new string[] { "", "Infos:", "" });
                for(int x = 0; x < chunk.FrameInfos.Length; x++)
                {
                    file.Add(chunk.FrameInfos[x].Hash.ToString());
                    file.Add(chunk.FrameInfos[x].NumIntegers.ToString());
                    for (int z = 0; z < chunk.FrameInfos[x].NumIntegers; z++)
                    {
                        file.Add(chunk.FrameInfos[x].Integers[z].ToString());
                    }
                    file.Add("");
                }
            }

            File.WriteAllLines("FrameProps.txt", file.ToArray());
        }
    }
}
