using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class ContextMenuActions
    {
        [MenuItem("GameObject/[Save As Bookmark]", false, 0)]
        private static void SaveSceneObjectsAsBookmarks()
        {
            SaveObjectsAsBookmarks(Selection.objects);
        }

        [MenuItem("Assets/[Save As Bookmark]")]
        private static void SaveAssetsAsABookmarks()
        {
            SaveObjectsAsBookmarks(Selection.GetFiltered(typeof(Object), SelectionMode.Assets));
        }

        public static void SaveObjectsAsBookmarks(Object[] selected)
        {
            if (selected != null && selected.Length > 0)
            {
                BookmarksUndo.BeforeBookmarkCreated();
                BookmarksStorage.Get().AddItems(selected);
                BookmarksStorage.Save();
                BookmarksWindow.ForceRepaintAllWindows();
                Debug.Log($"{selected.Length} object[s] bookmarked");
            }
            else
            {
                Debug.Log("No objects for bookmarking");
            }
        }
    }
}