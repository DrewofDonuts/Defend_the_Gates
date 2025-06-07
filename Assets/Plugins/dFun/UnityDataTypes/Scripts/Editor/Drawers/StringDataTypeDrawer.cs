using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class StringDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.String);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.TextField(
                rect,
                label,
                (string)Serializer.Deserialize(stringValue)
            );
        }
    }
}