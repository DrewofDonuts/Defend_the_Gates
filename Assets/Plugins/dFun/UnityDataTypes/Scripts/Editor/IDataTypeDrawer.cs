using UnityEngine;

namespace DFun.UnityDataTypes
{
    public interface IDataTypeDrawer
    {
        float ElementHeight { get; }
        IDataTypeSerializer Serializer { get; }

        /// <returns>Not serialized new value</returns>
        object Draw(Rect rect, string label, string stringValue);

        void ShowCustomContextMenu(ContextMenuData contextMenuData);
    }
}