using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Int32DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Int32);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.IntField(
                rect,
                label,
                (int)Serializer.Deserialize(stringValue)
            );
        }
    }
}