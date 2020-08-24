using System.Collections;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using UnityEngine;
using UnityEngine.UI;

namespace PostProcessingToolkit
{
    class SetupUI : ViewController
    {
        [UIValue("Enabled")]
        public bool Enabled
        {
            get => PluginConfig.Instance.Enabled;
            set
            {
                PluginConfig.Instance.Enabled = value;
                PostProcessLoader.Instance.EffectEnabled = value;
            }
        }

        [UIValue(nameof(Vignette))]
        public float Vignette
        {
            get => PluginConfig.Instance.Vignette;
            set
            {
                PluginConfig.Instance.Vignette = value;
                PostProcessLoader.Instance.Vignette.Value = value;
            }
        }

        [UIValue(nameof(Saturation))]
        public float Saturation
        {
            get => PluginConfig.Instance.Saturation;
            set
            {
                PluginConfig.Instance.Saturation = value;
                PostProcessLoader.Instance.Saturation.Value = value;
            }
        }

        [UIValue(nameof(Posterize))]
        public float Posterize
        {
            get => PluginConfig.Instance.Posterize;
            set
            {
                PluginConfig.Instance.Posterize = value;
                PostProcessLoader.Instance.Posterize.Value = value;
            }
        }

        [UIValue(nameof(PosterizePower))]
        public float PosterizePower
        {
            get => PluginConfig.Instance.PosterizePower;
            set
            {
                PluginConfig.Instance.PosterizePower = value;
                PostProcessLoader.Instance.PosterizePower.Value = value;
            }
        }

		[UIValue(nameof(Rain))]
        public float Rain
		{
			get => PluginConfig.Instance.Rain;
			set
			{
				PluginConfig.Instance.Rain = value;
				PostProcessLoader.Instance.Rain.Value = value;
			}
		}

        [UIValue(nameof(LUTStrength))]
        public float LUTStrength
        {
            get => PluginConfig.Instance.LUTStrength;
            set
            {
                PluginConfig.Instance.LUTStrength = value;
                PostProcessLoader.Instance.LUTStength.Value = value;
            }
        }

        [UIComponent("TexList")] private CustomListTableData _texTable;

        [UIComponent("TexModal")] private ModalView _texModal;

        [UIComponent("color-shift-preview")]
        private RawImage _colorShiftPreview;

        //{Prop}

        [UIComponent("colorpicker")] private ModalColorPicker _colorpicker;

        [UIAction("#post-parse")]
        private void Setup()
        {
            // Make color shift preview smaller in height
            (_colorShiftPreview.transform as RectTransform).localScale = new Vector2(1, 0.5f);
            _colorShiftPreview.texture = null;
            _colorShiftPreview.color = PluginConfig.Instance.ColorShift.ToColor();

            _texTable.data.Clear();
            foreach (Texture2D texture in LUTDatabase.Luts)
            {
                CustomListTableData.CustomCellInfo info = new CustomListTableData.CustomCellInfo(texture.name, null, texture);
                _texTable.data.Add(info);
            }
            _texTable.tableView.ReloadData();

        }

        [UIAction("change-color-shift")]
        private void ChangeColorShift()
        {
            _colorpicker.CurrentColor = _colorShiftPreview.color;
            _colorpicker.modalView.Show(true, true);
        }

        [UIAction("color-change")]
        private void OnColorChange(Color color)
        {
            PostProcessLoader.Instance.ColorShift.Value = color;
        }

        [UIAction("color-done")]
        private void OnColorDone(Color color)
        {
            PluginConfig.Instance.ColorShift.FromColor(color);
            _colorShiftPreview.color = color;
        }

        [UIAction("color-cancel")]
        private void OnColorCancel()
        {
            var color = _colorShiftPreview.color;
            PluginConfig.Instance.ColorShift.FromColor(color);
            PostProcessLoader.Instance.ColorShift.Value = color;
        }

        [UIAction("tex-selected")]
        private void TexSelected(TableView _, int row)
        {
            var selectedLUT = LUTDatabase.Luts[row];
            PluginConfig.Instance.LUTName = selectedLUT.name;
            PostProcessLoader.Instance.LUTTexture.Value = selectedLUT;
            _texModal.Hide(true);
        }
    }
}
