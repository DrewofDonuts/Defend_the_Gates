using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class DoubleDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Double);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.DoubleField(
                rect,
                label,
                (double)Serializer.Deserialize(stringValue)
            );
        }
    }
}