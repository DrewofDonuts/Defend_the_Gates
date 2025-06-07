using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksGroupCleanupPopup : PopupWindowContent
    {
        private readonly ConfirmEditDataView _cleanupBookmarksView;
        private readonly BookmarksGroup _group;

        public BookmarksGroupCleanupPopup(BookmarksGroup group)
        {
            _group = group;
            _cleanupBookmarksView = new ConfirmEditDataView(
                () => $"Are you sure you want \nto remove all bookmarks\nfrom group '{_group.Name}'?",
                CleanupGroup, Close
            );
            _cleanupBookmarksView.IsVisible = true;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 150);
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

        private void CleanupGroup()
        {
            BookmarksUndo.BeforeRemoveGroupBookmarks();
            _group.Cleanup();
            BookmarksStorage.Save();
            BookmarksWindow.ForceRepaintAllWindows();
            Close();
        }
    }
}