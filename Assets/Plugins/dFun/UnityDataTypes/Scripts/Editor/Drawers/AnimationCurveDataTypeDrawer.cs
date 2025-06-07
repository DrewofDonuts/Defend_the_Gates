using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class AnimationCurveDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.AnimationCurve);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.CurveField(
                rect,
                label,
                (AnimationCurve)Serializer.Deserialize(stringValue)
            );
        }
    }
}