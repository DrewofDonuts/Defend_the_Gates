using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector2IntDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 2.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Vector2Int);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.Vector2IntField(
                rect,
                label,
                (Vector2Int)Serializer.Deserialize(stringValue)
            );
        }
    }
}