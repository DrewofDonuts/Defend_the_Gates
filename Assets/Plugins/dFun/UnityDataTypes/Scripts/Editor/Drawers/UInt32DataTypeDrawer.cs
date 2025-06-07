using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class UInt32DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.UInt32);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return (uint)EditorGUI.LongField(
                rect,
                label,
                (uint)Serializer.Deserialize(stringValue)
            );
        }
    }
}