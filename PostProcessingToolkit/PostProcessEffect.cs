using UnityEngine;

namespace PostProcessingToolkit
{
    class PostProcessEffect : MonoBehaviour
    {

        private Material _mat;

        void Awake()
        {
            if(name.Contains("(Clone)"))Destroy(this);
        }

        void Update()
        {
            if (_mat)
            {
                var t = transform;
                _mat.SetVector("_CamPos", new Vector2(t.localPosition.x, t.localPosition.y));
                _mat.SetVector("_CamRot", new Vector2(t.rotation.x, t.rotation.y));
            }
        }

        public void Init(Material mat)
        {
            _mat = mat;
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (_mat == null) Graphics.Blit(src, dst);
            else Graphics.Blit(src, dst, _mat);
        }

        private float Remap(float inMin, float inMax, float outMin, float outMax, float value)
        {
            var t = Mathf.InverseLerp(inMin, inMax, value);
            return Mathf.Lerp(outMin, outMax, t);
        }
    }
}
