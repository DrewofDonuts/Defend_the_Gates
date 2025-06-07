using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class SingleDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Single);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.FloatField(
                rect,
                label,
                (float)Serializer.Deserialize(stringValue)
            );
        }
    }
}