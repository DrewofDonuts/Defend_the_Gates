using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Int64DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Int64);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.LongField(
                rect,
                label,
                (long)Serializer.Deserialize(stringValue)
            );
        }
    }
}