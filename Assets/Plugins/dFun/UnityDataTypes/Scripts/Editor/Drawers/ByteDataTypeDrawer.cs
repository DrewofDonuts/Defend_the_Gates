using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class ByteDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Byte);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return (byte)EditorGUI.IntField(
                rect,
                label,
                (byte)Serializer.Deserialize(stringValue)
            );
        }
    }
}