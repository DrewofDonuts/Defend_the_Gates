using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarkViewContextMenu
    {
        private readonly BaseBookmarkView _bookmarkView;

        private BookmarkSettingsPopup _settingsPopup;
        private BookmarkSettingsPopup SettingsPopup
        {
            get
            {
                if (_settingsPopup == null) _settingsPopup = new BookmarkSettingsPopup(_bookmarkView.BookmarkData);
                return _settingsPopup;
            }
        }

        public bool IsOpened => SettingsPopup.IsOpened;

        public BookmarkViewContextMenu(BaseBookmarkView bookmarkView)
        {
            _bookmarkView = bookmarkView;
        }

        public void Show(Rect buttonScreenRect)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("Edit"), false, () => ShowContextMenuSettingsPopup(buttonScreenRect, true)
            );
            menu.AddItem(
                new GUIContent("Manage Invokes"), false, () => ShowContextMenuSettingsPopup(buttonScreenRect, false)
            );
            menu.AddSeparator(string.Empty);
            menu.AddItem(
                new GUIContent("Copy"), false, _bookmarkView.Events.FireCopyInputEvent
            );
            menu.AddItem(
                new GUIContent("Copy (compressed)"), false, _bookmarkView.Events.FireCopyCompressedInputEvent
            );

            AddPasteMenuItems(menu);

            menu.AddSeparator(string.Empty);
            menu.AddItem(
                new GUIContent("Remove"), false, _bookmarkView.Events.FireRemoveInputEvent
            );

            menu.ShowAsContext();
        }

        private void ShowContextMenuSettingsPopup(Rect buttonScreenRect, bool focusOnNameField)
        {
            ShowSettingsPopup(GUIUtility.ScreenToGUIRect(buttonScreenRect), focusOnNameField);
        }

        public void ShowSettingsPopup(Rect buttonGuiRect, bool focusOnNameField)
        {
            SettingsPopup.FocusOnNameField = focusOnNameField;
            SettingsPopup.FocusOnInvokesField = !focusOnNameField;
            PopupWindow.Show(buttonGuiRect, SettingsPopup);
        }

        private void AddPasteMenuItems(GenericMenu menu)
        {
            if (BookmarksClipboard.HasData)
            {
                GUIContent guiContent = new GUIContent(
                    BuildPasteText(BookmarksClipboard.Data)
                );
                menu.AddItem(guiContent, false, _bookmarkView.Events.FirePasteInputEvent);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }
        }

        private static string BuildPasteText(Bookmarks bookmarksData)
        {
            return $"Paste [{BookmarksClipboard.BuildBookmarksCountText(bookmarksData.TotalBookmarksCount)}]";
        }
    }
}