using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector2DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Vector2);

        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 2.5f;

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.Vector2Field(
                rect,
                label,
                (Vector2)Serializer.Deserialize(stringValue)
            );
        }
    }
}