using System;
using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class EnumDataTypeDrawer : BaseDataTypeDrawer
    {
        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Enum);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            object enumValue = Serializer.Deserialize(stringValue);
            if (enumValue == null) return EnumDataTypeSerializer.None;

            return EditorGUI.EnumPopup(
                rect,
                label,
                (Enum)enumValue
            );
        }
    }
}