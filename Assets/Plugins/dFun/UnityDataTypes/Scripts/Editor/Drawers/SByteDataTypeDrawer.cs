using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class SByteDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.SByte);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return (sbyte)EditorGUI.IntField(
                rect,
                label,
                (sbyte)Serializer.Deserialize(stringValue)
            );
        }
    }
}