using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksWindowContextMenu
    {
        private readonly BookmarksWindow _parent;

        public BookmarksWindowContextMenu(BookmarksWindow parent)
        {
            _parent = parent;
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Copy"), false, Copy);
            menu.AddItem(new GUIContent("Copy (compressed)"), false, CopyCompressed);
            AddPasteMenuItems(menu);

            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Add New Group"), false, AddNewGroup);
            menu.AddItem(new GUIContent("Remove all bookmarks"), false, ShowRemoveAllBookmarksPopup);

            menu.AddSeparator(string.Empty);
            menu.AddItem(new GUIContent("Bookmarks Settings"), false, BookmarksSettingsMenu.OpenProjectSettings);
            menu.AddItem(new GUIContent("Open Online Docs"), false, OpenOnlineDocs);
            menu.AddItem(new GUIContent("Open Discord"), false, OpenDiscord);
            menu.AddItem(new GUIContent("Refresh Window"), false, _parent.InitializeView);
        }

        private void AddPasteMenuItems(GenericMenu menu)
        {
            if (BookmarksClipboard.HasData)
            {
                GUIContent guiContent = new GUIContent(
                    BuildPasteText(BookmarksClipboard.Data)
                );
                menu.AddItem(guiContent, false, Paste);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }
        }

        private static string BuildPasteText(Bookmarks bookmarksData)
        {
            return $"Paste [{BookmarksClipboard.BuildGroupsCountText(bookmarksData.TotalGroupsCount)}, " +
                   $"{BookmarksClipboard.BuildBookmarksCountText(bookmarksData.TotalBookmarksCount)}]";
        }

        private void Copy()
        {
            Copy(false);
        }

        private void CopyCompressed()
        {
            Copy(true);
        }

        private void Copy(bool compressed)
        {
            BookmarksClipboard.CopyToClipboard(
                BookmarksStorage.Get(), compressed
            );
            BookmarksWindow.ShowNotificationInAllWindows("All bookmarks copied to clipboard");
        }

        private void Paste()
        {
            if (!BookmarksClipboard.HasData) return;

            Bookmarks clipboardBookmarks = BookmarksClipboard.Data;
            BookmarksGroup[] clipboardGroups = clipboardBookmarks.Groups;
            if (clipboardGroups == null) return;

            BookmarksUndo.BeforePasteBookmarks();
            Bookmarks bookmarks = BookmarksStorage.Get();
            for (int i = 0, iSize = clipboardGroups.Length; i < iSize; i++)
            {
                BookmarksGroup clipboardGroup = clipboardGroups[i];
                if (clipboardGroup == null) continue;
                bookmarks.AddGroup(new BookmarksGroup(clipboardGroup));
            }

            bookmarks.Dirty = true;
            BookmarksStorage.Save();

            BookmarksWindow.ShowNotificationInAllWindows(
                $"Pasted {BookmarksClipboard.BuildGroupsCountText(clipboardBookmarks.TotalGroupsCount)}, " +
                $"{BookmarksClipboard.BuildBookmarksCountText(clipboardBookmarks.TotalBookmarksCount)}"
            );
        }

        private static void OpenOnlineDocs()
        {
            DocumentationHelper.OpenInBrowser();
        }

        private static void OpenDiscord()
        {
            DiscordHelper.Open();
        }

        private void AddNewGroup()
        {
            BookmarksUndo.BeforeBookmarksGroupCreated();
            BookmarksStorage.Get().AddNewGroup();
            BookmarksStorage.Save();
        }

        private void ShowRemoveAllBookmarksPopup()
        {
            Rect guiRect = new Rect(0, 0, 20, 20);
            Rect screenSpaceRect = _parent.ToScreenSpaceRect(guiRect);

            PopupWindow.Show(
                GUIUtility.ScreenToGUIRect(screenSpaceRect),
                new CleanupBookmarksPopup()
            );
        }
    }
}