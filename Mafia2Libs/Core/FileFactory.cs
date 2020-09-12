﻿using System;
using System.IO;

namespace Core.IO
{
    public class FileFactory
    {
        public static FileBase ConstructFromFileInfo(FileInfo info)
        {
            string extension = info.Extension.Replace(".", "").ToUpper();
            switch (extension)
            {
                case "CUT":
                    return new FileCutscene(info);
                case "DDS":
                    return new FileTextureDDS(info);
                case "SDS":
                    return new FileSDS(info);
                case "FR":
                    return new FileFrameResource(info);
                case "MTL":
                    return new FileMaterialLibrary(info);
                case "LUA":
                case "SHP":
                case "AP":
                    return new FileLua(info);
                case "EDS":
                    return new FileEntityDataStorage(info);
                case "SPE":
                    return new FileSpeech(info);
                case "TBL":
                    return new FileTable(info);
                case "TRA":
                    return new FileTranslokator(info);
                case "ACT":
                    return new FileActor(info);
                case "XML":
                    return new FileXML(info);
                case "BIN":
                    return new FileBin(info);
                case "XBIN":
                    return new FileXBin(info);
                case "PRF":
                    return new FilePrefab(info);
                default:
                    return new FileBase(info);
            }
        }

    }
}
