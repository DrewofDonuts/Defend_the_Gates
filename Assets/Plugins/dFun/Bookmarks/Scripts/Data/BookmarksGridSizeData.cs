using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksGridSizeData
    {
        private static string PrefsKey => BookmarksEditorPrefs.GridSizeKey;

        public static float GetGridSizeNormalized()
        {
            return Mathf.Clamp01(EditorPrefs.GetFloat(PrefsKey, 0));
        }

        public static void SetGridSizeNormalized(float value)
        {
            EditorPrefs.SetFloat(PrefsKey, value);
        }

        // [MenuItem("Tools/dFun/Reset EditorPrefs BookmarksGridSize")]
        private static void Reset()
        {
            EditorPrefs.DeleteKey(PrefsKey);
        }
    }
}