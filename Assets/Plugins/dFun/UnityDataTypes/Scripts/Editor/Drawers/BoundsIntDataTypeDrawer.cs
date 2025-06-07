using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class BoundsIntDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 4;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.BoundsInt);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.BoundsIntField(
                rect,
                label,
                (BoundsInt)Serializer.Deserialize(stringValue)
            );
        }
    }
}