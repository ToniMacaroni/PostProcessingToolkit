using UnityEngine;

namespace PostProcessingToolkit
{
    public class FloatMatProp : MaterialProperty
    {
        public FloatMatProp(Material mat, string propertyName) : base(mat, propertyName)
        {
        }

        public float Value
        {
            get => _mat.GetFloat(_propertyName);
            set => _mat.SetFloat(_propertyName, value);
        }
    }

    public class TexMatProp : MaterialProperty
    {
        public TexMatProp(Material mat, string propertyName) : base(mat, propertyName)
        {
        }

        public Texture Value
        {
            get => _mat.GetTexture(_propertyName);
            set => _mat.SetTexture(_propertyName, value);
        }
    }

    public class ColorMatProp : MaterialProperty
    {
        public ColorMatProp(Material mat, string propertyName) : base(mat, propertyName)
        {
        }

        public Color Value
        {
            get => _mat.GetColor(_propertyName);
            set => _mat.SetColor(_propertyName, value);
        }
    }

    public abstract class MaterialProperty
    {
        protected string _propertyName;
        protected Material _mat;

        protected MaterialProperty(Material mat, string propertyName)
        {
            _mat = mat;
            _propertyName = "_" + propertyName;
        }
    }
}
