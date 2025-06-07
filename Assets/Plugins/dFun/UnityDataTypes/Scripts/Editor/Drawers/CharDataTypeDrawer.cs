using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class CharDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Char);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            char value = (char)Serializer.Deserialize(stringValue);
            string newValueStr = EditorGUI.TextField(
                rect,
                label,
                value.ToString()
            );
            return newValueStr.Length >= 1 ? newValueStr[0] : value;
        }
    }
}