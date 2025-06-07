using UnityEngine;

namespace DFun.Bookmarks
{
    public static class ScrollHelper
    {
        /// Based on UnityEditor.IMGUI.Controls.TreeViewState:EnsureRowIsVisible(...)
        /// <returns>Updated scroll pos</returns>
        public static Vector2 BringChildIntoViewVertical(Rect childRect, Rect viewRect, Vector2 currentScrollPos)
        {
            float viewHeight = viewRect.height;

            float scrollTop = childRect.y;
            float scrollBottom = childRect.yMax - viewHeight;

            if (currentScrollPos.y < scrollBottom)
            {
                return new Vector2(currentScrollPos.x, scrollBottom);
            }

            if (currentScrollPos.y > scrollTop)
            {
                return new Vector2(currentScrollPos.x, scrollTop);
            }

            return currentScrollPos;
        }
    }
}