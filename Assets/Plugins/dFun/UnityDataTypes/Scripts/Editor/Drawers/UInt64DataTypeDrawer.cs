using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class UInt64DataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.UInt64);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            ulong ulongValue = (ulong)Serializer.Deserialize(stringValue);

            EditorGUI.BeginChangeCheck();

            string ulongString = EditorGUI.TextField(
                rect,
                label,
                ulongValue.ToString()
            );

            if (EditorGUI.EndChangeCheck())
            {
                if (ulong.TryParse(ulongString, out ulong parsedValue))
                {
                    return parsedValue;
                }
                else
                {
                    return ulongValue;
                }
            }
            else
            {
                return ulongValue;
            }
        }
    }
}