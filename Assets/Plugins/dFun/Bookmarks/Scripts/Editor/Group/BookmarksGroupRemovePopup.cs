using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksGroupRemovePopup : PopupWindowContent
    {
        private readonly ConfirmEditDataView _cleanupBookmarksView;
        private readonly BookmarksGroup _group;

        public BookmarksGroupRemovePopup(BookmarksGroup group)
        {
            _group = group;
            _cleanupBookmarksView = new ConfirmEditDataView(
                () => $"Are you sure you want\nto remove group\n'{_group.Name}'?",
                RemoveCurrentGroup, Close
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

        private void RemoveCurrentGroup()
        {
            RemoveGroup(_group);
            Close();
        }

        public static void RemoveGroup(BookmarksGroup group)
        {
            BookmarksUndo.BeforeBookmarksGroupRemoved();
            BookmarksStorage.Get().RemoveGroup(group);
            BookmarksStorage.Save();
            BookmarksWindow.ForceRepaintAllWindows();

            BookmarksWindow.ShowNotificationInAllWindows($"Group {group.Name} was removed");
        }
    }
}