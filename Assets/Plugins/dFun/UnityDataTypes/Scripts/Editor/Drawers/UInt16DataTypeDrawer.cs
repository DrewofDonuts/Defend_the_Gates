using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class UInt16DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.UInt16);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return (ushort)EditorGUI.IntField(
                rect,
                label,
                (ushort)Serializer.Deserialize(stringValue)
            );
        }
    }
}