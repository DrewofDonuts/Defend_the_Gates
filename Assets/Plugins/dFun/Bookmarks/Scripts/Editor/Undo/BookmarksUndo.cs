using UnityEditor;

namespace DFun.Bookmarks
{
    public static class BookmarksUndo
    {
        static BookmarksUndo()
        {
            Undo.undoRedoPerformed -= UpdateState;
            Undo.undoRedoPerformed += UpdateState;
        }

        public static void BeforeBookmarkCreated()
        {
            RecordState("Bookmark create");
        }

        public static void BeforeBookmarkRemoved()
        {
            RecordState("Bookmark remove");
        }

        public static void BeforeBookmarksGroupCreated()
        {
            RecordState("Bookmarks group create");
        }

        public static void BeforeBookmarksGroupRemoved()
        {
            RecordState("Bookmarks group remove");
        }

        public static void BeforeRemoveGroupBookmarks()
        {
            RecordState("Remove group bookmarks");
        }

        public static void BeforeRemoveAllBookmarks()
        {
            RecordState("Remove all bookmarks");
        }

        public static void BeforeSortBookmarks()
        {
            RecordState("Sort bookmarks");
        }

        public static void BeforePasteBookmarks()
        {
            RecordState("Paste bookmarks");
        }

        public static void RecordState(string message)
        {
            Undo.RecordObject(BookmarksStorage.GetUndoObject(), message);
        }

        private static void UpdateState()
        {
            BookmarksStorage.Save();
            BookmarksStorage.Get().Dirty = true;
            BookmarksWindow.ForceRepaintAllWindows();
        }
    }
}