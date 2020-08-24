using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace PostProcessingToolkit
{
    class PostProcessLoader : MonoBehaviour
    {
        public static List<PostProcessEffect> EffectInstances = new List<PostProcessEffect>();

        public static PostProcessLoader Instance;

        public Material Material;

        public bool EffectEnabled
        {
            set
            {
                foreach (PostProcessEffect effect in EffectInstances)
                {
                    if(effect) effect.enabled = value;
                }
            }
        }

        public FloatMatProp Saturation;
        public FloatMatProp Vignette;
        public FloatMatProp Posterize;
        public FloatMatProp PosterizePower;
		public FloatMatProp Rain;
        public FloatMatProp LUTStength;

        public TexMatProp LUTTexture;

        public ColorMatProp ColorShift;
		//{Prop}

        public static void Create()
        {
            GameObject go = new GameObject("PostProcessingLoader");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<PostProcessLoader>();
        }

        void Start()
        {
            Material = new Material(Plugin.PostShader);

            Saturation = new FloatMatProp(Material, nameof(Saturation));
            Vignette = new FloatMatProp(Material, nameof(Vignette));
            Posterize = new FloatMatProp(Material, nameof(Posterize));
            PosterizePower = new FloatMatProp(Material, nameof(PosterizePower));
			Rain = new FloatMatProp(Material, nameof(Rain));
            ColorShift = new ColorMatProp(Material, nameof(ColorShift));
            LUTStength = new FloatMatProp(Material, nameof(LUTStength));
            LUTTexture = new TexMatProp(Material, nameof(LUTTexture));
            //LUTTexture = new TexMatProp(Material, "smth");
            //{Prop definition}

            Saturation.Value = PluginConfig.Instance.Saturation;
            Vignette.Value = PluginConfig.Instance.Vignette;
            Posterize.Value = PluginConfig.Instance.Posterize;
            PosterizePower.Value = PluginConfig.Instance.PosterizePower;
			Rain.Value = PluginConfig.Instance.Rain;
            ColorShift.Value = PluginConfig.Instance.ColorShift.ToColor();
            LUTStength.Value = PluginConfig.Instance.LUTStrength;
            var lutTexture = LUTDatabase.FindLut(PluginConfig.Instance.LUTName);
            if (lutTexture == null) LUTStength.Value = 0;
            LUTTexture.Value = lutTexture;
            //Material.SetTexture("_LUTTexture", Plugin.LUT);
			//{Prop config}

            Material.SetTexture("_RainTex", Plugin.LoadResourceTexture("PostProcessingToolkit.resources.rain.png"));

            Plugin.Log.Debug($"Initialized {nameof(PostProcessLoader)}");
        }

        public void AddEffect(Camera cam)
        {
            if (cam.GetComponent<PostProcessEffect>() != null) return;
            var effect = cam.gameObject.AddComponent<PostProcessEffect>();
            effect.Init(Material);
            EffectInstances.Add(effect);
            if (!PluginConfig.Instance.Enabled) effect.enabled = false;
        }

        void OnDestroy()
        {
            DestroyEffects();
        }

        public void DestroyEffects()
        {
            foreach (PostProcessEffect effect in EffectInstances)
            {
                if (effect) Destroy(effect);
            }
        }
    }

    [HarmonyPatch("Awake")]
    [HarmonyPatch(typeof(MainCamera))]
    public class MainCameraPatch
    {
        public static void Postfix(Camera ____camera)
        {
            if(!____camera.name.Contains(".cfg") && ____camera.stereoEnabled) PostProcessLoader.Instance.AddEffect(____camera);
        }
    }
}
