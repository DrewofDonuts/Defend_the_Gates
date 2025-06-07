using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Int16DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Int16);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return (short)EditorGUI.IntField(
                rect,
                label,
                (short)Serializer.Deserialize(stringValue)
            );
        }
    }
}