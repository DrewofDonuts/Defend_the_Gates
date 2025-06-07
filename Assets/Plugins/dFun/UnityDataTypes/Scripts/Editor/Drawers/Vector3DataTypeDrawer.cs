using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector3DataTypeDrawer : BaseDataTypeDrawer
    {
        public override float ElementHeight => EditorGUIUtility.singleLineHeight * 2.5f;

        public override IDataTypeSerializer Serializer => DataTypeSerialization
            .GetDataTypeSerializer(SupportedDataTypes.Vector3);

        public override object Draw(Rect rect, string label, string stringValue)
        {
            return EditorGUI.Vector3Field(
                rect,
                label,
                (Vector3)Serializer.Deserialize(stringValue)
            );
        }

        public override void ShowCustomContextMenu(ContextMenuData contextMenuData)
        {
            Vector3ContextMenu.ShowContextMenu(contextMenuData);
            contextMenuData.WasShown = true;
        }
    }
}