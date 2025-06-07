using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector3IntDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 2.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Vector3Int);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.Vector3IntField(
                rect,
                label,
                (Vector3Int)Serializer.Deserialize(stringValue)
            );
        }
    }
}