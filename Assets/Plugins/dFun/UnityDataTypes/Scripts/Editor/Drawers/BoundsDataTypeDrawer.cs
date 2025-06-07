using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class BoundsDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 4;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Bounds);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.BoundsField(
                rect,
                label,
                (Bounds)Serializer.Deserialize(stringValue)
            );
        }
    }
}