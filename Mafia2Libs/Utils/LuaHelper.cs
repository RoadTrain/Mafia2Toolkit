﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnluacNET;

namespace Utils.Lua
{
    public class LuaHelper
    {
        private static LFunction FileToFunction(string fn)
        {
            using (var fs = File.Open(fn, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BHeader header = new BHeader(fs);
                return header.Function.Parse(fs, header);
            }
        }

        public static void ReadFile(FileInfo info)
        {
            LFunction main = null;
            main = FileToFunction(info.FullName);
            Decompiler decompile = new Decompiler(main);
            decompile.Decompile();
            bool isAP = (info.Extension == ".AP" ? true : false);
            string name = info.FullName.Remove(info.FullName.Length - (isAP ? 7 : 4));
            name += "_d.lua"/* + (isAP ? ".ap " : "")*/;
            using (var writer = new StreamWriter(name, false, Encoding.ASCII))
            {
                Output output = new Output(writer);
                decompile.Print(output);
                writer.Flush();
            }
        }
    }
}
