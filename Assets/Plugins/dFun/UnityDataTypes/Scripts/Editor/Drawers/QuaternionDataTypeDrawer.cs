using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class QuaternionDataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 2.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Quaternion);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            Quaternion value = (Quaternion)Serializer.Deserialize(stringValue);
            Vector4 newValue = EditorGUI.Vector4Field(
                rect,
                label,
                ToVector4(value)
            );
            return ToQuaternion(newValue);
        }

        public override void ShowCustomContextMenu(ContextMenuData contextMenuData)
        {
            QuaternionContextMenu.ShowContextMenu(contextMenuData);
            contextMenuData.WasShown = true;
        }

        private static Vector4 ToVector4(Quaternion quaternion)
        {
            return new Vector4(
                quaternion.x,
                quaternion.y,
                quaternion.z,
                quaternion.w
            );
        }

        private static Quaternion ToQuaternion(Vector4 vector4)
        {
            return new Quaternion(
                vector4.x,
                vector4.y,
                vector4.z,
                vector4.w
            );
        }
    }
}