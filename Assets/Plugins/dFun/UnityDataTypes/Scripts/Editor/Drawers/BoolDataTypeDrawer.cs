using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class BoolDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Bool);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.ToggleLeft(
                rect,
                label,
                (bool)Serializer.Deserialize(stringValue)
            );
        }
    }
}