using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class RectDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 3.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Rect);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.RectField(
                rect,
                label,
                (Rect)Serializer.Deserialize(stringValue)
            );
        }
    }
}