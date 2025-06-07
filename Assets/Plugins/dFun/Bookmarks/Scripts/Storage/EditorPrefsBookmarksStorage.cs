using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class EditorPrefsBookmarksStorage : IBookmarksStorage
    {
        private static string PrefsKey => BookmarksEditorPrefs.DataStorageKey;

        private BookmarksWrapper _bookmarksWrapper;

        public Bookmarks Get()
        {
            return GetWrapper().Bookmarks;
        }

        private BookmarksWrapper GetWrapper()
        {
            if (_bookmarksWrapper == null)
            {
                _bookmarksWrapper = ScriptableObject.CreateInstance<BookmarksWrapper>();
                _bookmarksWrapper.hideFlags = HideFlags.HideAndDontSave;
                string dataJson = EditorPrefs.GetString(PrefsKey);
                EditorJsonUtility.FromJsonOverwrite(dataJson, _bookmarksWrapper.Bookmarks);
            }

            return _bookmarksWrapper;
        }

        public Object GetUndoObject()
        {
            return GetWrapper();
        }

        public void Save()
        {
            EditorPrefs.SetString(PrefsKey, EditorJsonUtility.ToJson(Get()));
        }

        // [MenuItem("Tools/dFun/Remove EditorPrefs Bookmarks")]
        private static void Reset()
        {
            EditorPrefs.DeleteKey(PrefsKey);
        }

        public void Cleanup()
        {
            _bookmarksWrapper = null;
            Reset();
        }

        public static bool HasAnyData()
        {
            return PlayerPrefs.HasKey(PrefsKey);
        }
    }
}