using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksGroupViewContextMenu
    {
        private readonly BookmarksGroup _groupData;

        private BookmarksGroupSettingsPopup _settingsPopup;
        private BookmarksGroupSettingsPopup SettingsPopup
        {
            get
            {
                if (_settingsPopup == null) _settingsPopup = new BookmarksGroupSettingsPopup(_groupData);
                return _settingsPopup;
            }
        }

        private BookmarksGroupCleanupPopup _bookmarksGroupCleanupPopup;
        private BookmarksGroupCleanupPopup BookmarksGroupCleanupPopup
        {
            get
            {
                if (_bookmarksGroupCleanupPopup == null)
                    _bookmarksGroupCleanupPopup = new BookmarksGroupCleanupPopup(_groupData);
                return _bookmarksGroupCleanupPopup;
            }
        }

        private BookmarksGroupRemovePopup _bookmarksGroupRemovePopup;
        private BookmarksGroupRemovePopup BookmarksGroupRemovePopup
        {
            get
            {
                if (_bookmarksGroupRemovePopup == null)
                    _bookmarksGroupRemovePopup = new BookmarksGroupRemovePopup(_groupData);
                return _bookmarksGroupRemovePopup;
            }
        }

        public BookmarksGroupViewContextMenu(BookmarksGroup groupData)
        {
            _groupData = groupData;
        }

        public void Show(Rect buttonScreenRect)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("Edit group"), false, () => { ShowSettingsPopup(buttonScreenRect); }
            );
            menu.AddSeparator(string.Empty);
            menu.AddItem(
                new GUIContent("Copy group"), false, CopyGroup
            );

            menu.AddItem(
                new GUIContent("Copy group (compressed)"), false, CopyGroupCompressed
            );

            AddPasteToThisGroupMenuItems(menu);

            menu.AddSeparator(string.Empty);
            menu.AddItem(
                new GUIContent("Add special/Script Bookmark [HEAVY]"), false, () => AddScriptBookmark(buttonScreenRect)
            );

            menu.AddSeparator(string.Empty);
            AddCleanupGroupItems(buttonScreenRect, menu);
            menu.AddItem(
                new GUIContent("Remove group"), false, () => ShowRemoveGroupPopup(buttonScreenRect)
            );

            menu.ShowAsContext();
        }

        private void AddCleanupGroupItems(Rect buttonScreenRect, GenericMenu menu)
        {
            GUIContent guiContent = new GUIContent("Remove all bookmarks from group");
            if (_groupData.HasAnyBookmark)
            {
                menu.AddItem(
                    guiContent, false,
                    () => ShowRemoveAllGroupBookmarksPopup(buttonScreenRect)
                );
            }
            else
            {
                menu.AddDisabledItem(guiContent);
            }
        }

        public void ShowSettingsPopup(Rect buttonScreenRect)
        {
            PopupWindow.Show(
                ConvertToPopupGuiRect(buttonScreenRect),
                SettingsPopup
            );
        }

        private void CopyGroup()
        {
            CopyGroup(false);
        }

        private void CopyGroupCompressed()
        {
            CopyGroup(true);
        }

        private void CopyGroup(bool compressed)
        {
            BookmarksClipboard.CopyToClipboard(_groupData, compressed);
            BookmarksWindow.ShowNotificationInAllWindows($"Group '{_groupData.Name}' copied to clipboard");
        }

        private void AddPasteToThisGroupMenuItems(GenericMenu menu)
        {
            if (BookmarksClipboard.HasData)
            {
                GUIContent guiContent = new GUIContent(
                    BuildPasteText(BookmarksClipboard.Data)
                );
                menu.AddItem(guiContent, false, PasteToThisGroup);
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

        private void PasteToThisGroup()
        {
            BookmarksGroupClipboardHelper.PasteClipboardDataToGroup(_groupData);
        }

        private void ShowRemoveAllGroupBookmarksPopup(Rect buttonScreenRect)
        {
            PopupWindow.Show(
                ConvertToPopupGuiRect(buttonScreenRect),
                BookmarksGroupCleanupPopup
            );
        }

        private void ShowRemoveGroupPopup(Rect buttonScreenRect)
        {
            if (_groupData.HasAnyBookmark)
            {
                PopupWindow.Show(
                    ConvertToPopupGuiRect(buttonScreenRect),
                    BookmarksGroupRemovePopup
                );
            }
            else
            {
                BookmarksGroupRemovePopup.RemoveGroup(_groupData);
            }
        }

        private Rect ConvertToPopupGuiRect(Rect buttonScreenRect)
        {
            Rect buttonGUIRect = GUIUtility.ScreenToGUIRect(buttonScreenRect);
            buttonGUIRect.min = buttonGUIRect.center;
            return buttonGUIRect;
        }

        private void AddScriptBookmark(Rect buttonScreenRect)
        {
            ScriptBookmarkHelper.ShowAddScriptBookmarkPopup(
                _groupData, ConvertToPopupGuiRect(buttonScreenRect)
            );
        }
    }
}