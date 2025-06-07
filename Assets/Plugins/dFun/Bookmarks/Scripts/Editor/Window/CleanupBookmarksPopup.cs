using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class CleanupBookmarksPopup : PopupWindowContent
    {
        private readonly ConfirmEditDataView _cleanupBookmarksView;

        public CleanupBookmarksPopup()
        {
            _cleanupBookmarksView = new ConfirmEditDataView(
                () => "Are you sure you want\nto remove\nall groups and bookmarks?",
                RemoveAllBookmarks, Close
            );
            _cleanupBookmarksView.IsVisible = true;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(230, 130);
        }

        public override void OnGUI(Rect rect)
        {
            _cleanupBookmarksView.DrawIfVisible();
            PopupHelper.HandlePopupKeysEventsExt(Close);
        }

        private void Close()
        {
            editorWindow.Close();
        }

        private void RemoveAllBookmarks()
        {
            BookmarksUndo.BeforeRemoveAllBookmarks();
            BookmarksStorage.Get().RemoveAll();
            BookmarksStorage.Save();
            Close();
        }
    }
}