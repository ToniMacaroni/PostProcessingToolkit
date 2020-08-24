using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PostProcessingToolkit
{
    public static class LUTDatabase
    {
        public static Action LoadingFinished;

        public static List<Texture2D> Luts = new List<Texture2D>();

        public static void LoadTextures()
        {
            foreach (var lutFile in Plugin.PPTDir.CreateSubdirectory("LUTs").GetFiles("*.png"))
            {
                if(lutFile.Name=="Default.png")continue;
                Texture2D tex = Plugin.LoadTex(lutFile);
                if(tex) Luts.Add(tex);
            }
        }

        public static Texture2D FindLut(string name)
        {
            return Luts.FirstOrDefault(l => l.name == name);
        }
    }
}
