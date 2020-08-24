using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using BeatSaberMarkupLanguage.GameplaySetup;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using UnityEngine;
using Config = IPA.Config.Config;
using Logger = IPA.Logging.Logger;

namespace PostProcessingToolkit
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    internal class Plugin
    {
        public static Logger Log;

        public static Shader PostShader;
        public static Harmony Harmony;

        public static DirectoryInfo MainDir;
        public static DirectoryInfo PPTDir;

        [Init]
        public Plugin(Logger logger, Config config)
        {
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();

        }

        [OnStart]
        public void OnStart()
        {
            MainDir = new DirectoryInfo(Environment.CurrentDirectory);
            PPTDir = MainDir.CreateSubdirectory("UserData\\PostProcessingToolkit");

            Harmony = new Harmony("com.tonimacaroni.postprocessingtoolkit");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            LoadShader();
            GameplaySetup.instance.AddTab("Post", "PostProcessingToolkit.resources.setup.bsml", new SetupUI());
            PostProcessLoader.Create();
            LUTDatabase.LoadTextures();
        }

        private void LoadShader()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PostProcessingToolkit.resources.shaderpack");
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            stream.Close();
            AssetBundle bundle = AssetBundle.LoadFromMemory(buffer);
            PostShader = bundle.LoadAsset<Shader>("PostProcess");
        }

        public static Texture2D LoadTex(FileInfo file)
        {
            try
            {
                Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false, true);
                tex.LoadImage(File.ReadAllBytes(file.FullName));

                tex.wrapMode = TextureWrapMode.Clamp;
                tex.filterMode = FilterMode.Bilinear;
                tex.anisoLevel = 0;

                tex.name = file.Name.Replace(file.Extension, "");

                return tex;
            }
            catch (Exception e)
            {
                //ignore
            }

            return null;
        }

        public static Texture2D LoadResourceTexture(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
            tex.LoadImage(buffer);
            return tex;
        }

        [OnExit]
        public void OnExit()
        {
        }
    }
}
