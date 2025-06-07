using UnityEditor;

namespace DFun.Bookmarks
{
    public static class ActiveBookmarksGroup
    {
        private static string PrefsKey => BookmarksEditorPrefs.ActiveGroupKey;

        public static int GetActiveGroupIndex(BookmarksGroup[] groups)
        {
            return NormalizeActiveGroupIndex(EditorPrefs.GetInt(PrefsKey, 0), groups);
        }

        public static void SetActiveGroupIndex(int indexValue)
        {
            EditorPrefs.SetInt(PrefsKey, indexValue);
        }

        // [MenuItem("Tools/dFun/Remove EditorPrefs Active Group")]
        private static void Reset()
        {
            EditorPrefs.DeleteKey(PrefsKey);
        }

        private static int NormalizeActiveGroupIndex(int indexValue, BookmarksGroup[] groups)
        {
            if (indexValue > 0 && groups != null && indexValue <= groups.Length - 1)
            {
                return indexValue;
            }

            return 0;
        }
    }
}