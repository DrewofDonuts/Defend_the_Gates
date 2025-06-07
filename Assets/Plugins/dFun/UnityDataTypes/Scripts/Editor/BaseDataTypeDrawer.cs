using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public abstract class BaseDataTypeDrawer : IDataTypeDrawer
    {
        public virtual float ElementHeight => EditorGUIUtility.singleLineHeight;
        public abstract IDataTypeSerializer Serializer { get; }
        public abstract object Draw(Rect rect, string label, string stringValue);

        public virtual void ShowCustomContextMenu(ContextMenuData contextMenuData)
        {
        }
    }
}