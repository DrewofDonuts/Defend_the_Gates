using UnityEditor;

namespace DFun.Bookmarks
{
    public static class MouseHelper
    {
        private const double DoubleClickTime = 0.3;
        
        /// Based on https://discussions.unity.com/t/how-to-detect-double-clicked-button-in-editor-window/10437/3
        public static bool WasDoubleClicked(ref double lastClickTime)
        {
            bool isDoubleClick = EditorApplication.timeSinceStartup - lastClickTime < DoubleClickTime;
            lastClickTime = isDoubleClick ? 0 : EditorApplication.timeSinceStartup;

            return isDoubleClick;
        }
    }
}