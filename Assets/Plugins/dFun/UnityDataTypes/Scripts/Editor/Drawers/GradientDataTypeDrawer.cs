using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class GradientDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Gradient);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.GradientField(
                rect,
                label,
                (Gradient)Serializer.Deserialize(stringValue)
            );
        }
    }
}