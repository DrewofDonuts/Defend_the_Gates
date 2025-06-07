using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class RectIntDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 3.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.RectInt);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.RectIntField(
                rect,
                label,
                (RectInt)Serializer.Deserialize(stringValue)
            );
        }
    }
}