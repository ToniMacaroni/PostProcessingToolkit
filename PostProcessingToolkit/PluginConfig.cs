using UnityEngine;

namespace PostProcessingToolkit
{
    public class PluginConfig
    {
        public static PluginConfig Instance { get; set; }

        public bool Enabled { get; set; } = true;

        public float OpacityMultiplier { get; set; } = 0.35f;

        public float Saturation { get; set; } = 0f;

        public float Vignette { get; set; } = 0f;

        public float Posterize { get; set; } = 0f;

        public float PosterizePower { get; set; } = 100f;

		public float Rain { get; set; } = 0f;

        public SerializableColor ColorShift { get; set; } = new SerializableColor();

        public float LUTStrength { get; set; } = 0;

        public string LUTName { get; set; } = "Vapor.png";

        //{Prop}
    }

    public class SerializableColor
    {
        public float r { get; set; } = 1f;
        public float g { get; set; } = 1f;
        public float b { get; set; } = 1f;
        public float a { get; set; } = 1f;

        public void FromColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public Color ToColor()
        {
            return new Color(r, g, b, a);
        }
    }
}
