using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class ColorDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Color);

        private readonly GUIContent _labelContent = new GUIContent();

        private const bool ShowEyedropper = true;
        private const bool ShowAlpha = true;
        private const bool HDR = true;

        public override object Draw(Rect rect, string label, string stringValue)
        {
            _labelContent.text = label;
            return EditorGUI.ColorField(
                rect,
                _labelContent,
                (Color)Serializer.Deserialize(stringValue),
                ShowEyedropper,
                ShowAlpha,
                HDR
            );
        }
    }
}