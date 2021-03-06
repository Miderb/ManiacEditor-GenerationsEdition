﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RSDKv5
{
    public class GameConfig : CommonConfig
    {
        public String GameName;
        public String GameSubname;
        public String Version;

        bool _scenesHaveModeFilter;

        public byte StartSceneCategoryIndex;
        public ushort StartSceneIndex;


        public class SceneInfo
        {
            public string Name;
            public string Zone;
            public string SceneID;
            public byte ModeFilter;
            public int LevelID; //For GameConfig Position; Used for Auto Booting

            public SceneInfo()
            {
            }

            internal SceneInfo(Reader reader, bool scenesHaveModeFilter, int currentConfigID = 0)
            {
                Name = reader.ReadRSDKString();
                Zone = reader.ReadRSDKString();
                SceneID = reader.ReadRSDKString();
                LevelID = currentConfigID; //For GameConfig Position; Used for Auto Booting

                if (scenesHaveModeFilter) ModeFilter = reader.ReadByte();
            }

            internal void Write(Writer writer, bool scenesHaveModeFilter = false)
            {
                writer.WriteRSDKString(Name);
                writer.WriteRSDKString(Zone);
                writer.WriteRSDKString(SceneID);

                if (scenesHaveModeFilter) writer.Write(ModeFilter);
            }
        }

        public class Category
        {
            public string Name;
            public List<SceneInfo> Scenes = new List<SceneInfo>();

            internal Category(Reader reader, bool scenesHaveModeFilter)
            {
                Name = reader.ReadRSDKString();

                byte scenes_count = reader.ReadByte();
                for (int i = 0; i < scenes_count; ++i)
                {
                    Scenes.Add(new SceneInfo(reader, scenesHaveModeFilter, RSDKv5.GameConfig.CurrentLevelID));
                    RSDKv5.GameConfig.CurrentLevelID++;
                }

            }

            internal void Write(Writer writer, bool scenesHaveModeFilter = false)
            {
                writer.WriteRSDKString(Name);

                writer.Write((byte)Scenes.Count);
                foreach (SceneInfo scene in Scenes)
                    scene.Write(writer, scenesHaveModeFilter);
            }
        }

        public class ConfigurableMemoryEntry
        {
            public uint Index;
            public int[] Data;

            internal ConfigurableMemoryEntry(Reader reader)
            {
                Index = reader.ReadUInt32();
                uint Count = reader.ReadUInt32();
                Data = new int[Count];
                for (int i = 0; i < Count; ++i)
                    Data[i] = reader.ReadInt32();
            }

            internal void Write(Writer writer)
            {
                writer.Write(Index);
                writer.Write((uint)Data.Length);
                foreach (uint val in Data)
                    writer.Write(val);
            }
        }

        public List<Category> Categories = new List<Category>();

        public List<ConfigurableMemoryEntry> ConfigMemory = new List<ConfigurableMemoryEntry>();

        public GameConfig(string filename)
        {
            using (var reader = new Reader(filename))
                Read(reader);
        }

        public GameConfig(Stream stream)
        {
            using (var reader = new Reader(stream))
                Read(reader);
        }

        private void Read(Reader reader)
        {
            ReadMagic(reader);

            GameName = reader.ReadRSDKString();
            GameSubname = reader.ReadRSDKString();
            Version = reader.ReadRSDKString();

            InterpretVersion();

            StartSceneCategoryIndex = reader.ReadByte();
            StartSceneIndex = reader.ReadUInt16();

            ReadCommonConfig(reader);

            ushort TotalScenes = reader.ReadUInt16();
            if (CurrentLevelID >= TotalScenes)
            {
                CurrentLevelID = 0;
            }

            byte categories_count = reader.ReadByte();
            for (int i = 0; i < categories_count; ++i)
            {
                Categories.Add(new Category(reader, _scenesHaveModeFilter));
            }


            byte config_memory_count = reader.ReadByte();
            for (int i = 0; i < config_memory_count; ++i)
                ConfigMemory.Add(new ConfigurableMemoryEntry(reader));
        }

        private void InterpretVersion()
        {
            string[] versionParts = Version.Split('.');
            int midVersion = Int32.Parse(versionParts[1]);
            if (midVersion >= 5)
            {
                _scenesHaveModeFilter = true;
            }
        }

        public void Write(string filename)
        {
            using (Writer writer = new Writer(filename))
                Write(writer);
        }

        public void Write(Stream stream)
        {
            using (Writer writer = new Writer(stream))
                Write(writer);
        }

        internal void Write(Writer writer)
        {
            WriteMagic(writer);

            writer.WriteRSDKString(GameName);
            writer.WriteRSDKString(GameSubname);
            writer.WriteRSDKString(Version);

            writer.Write(StartSceneCategoryIndex);
            writer.Write(StartSceneIndex);

            WriteCommonConfig(writer);

            writer.Write((ushort)Categories.Select(x => x.Scenes.Count).Sum());

            writer.Write((byte)Categories.Count);
            foreach (Category cat in Categories)
                cat.Write(writer, _scenesHaveModeFilter);

            writer.Write((byte)ConfigMemory.Count);
            foreach (ConfigurableMemoryEntry c in ConfigMemory)
                c.Write(writer);
        }
    }
}
