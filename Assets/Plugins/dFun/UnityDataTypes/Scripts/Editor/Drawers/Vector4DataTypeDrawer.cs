using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector4DataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 2.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Vector4);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.Vector4Field(
                rect,
                label,
                (Vector4)Serializer.Deserialize(stringValue)
            );
        }
    }
}