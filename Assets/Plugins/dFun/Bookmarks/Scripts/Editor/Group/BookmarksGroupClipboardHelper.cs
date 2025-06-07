using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public static class BookmarksGroupClipboardHelper
    {
        public static void CopySelectedBookmarks(BookmarksGroup groupData, BookmarksViewState state, bool compressed)
        {
            List<Bookmark> selectedBookmarks = state.GetSelectedBookmarks(groupData);
            BookmarksClipboard.CopyToClipboard(selectedBookmarks, compressed);
            BookmarksWindow.ShowNotificationInAllWindows(
                $"{BookmarksClipboard.BuildBookmarksCountText(selectedBookmarks.Count)} copied to clipboard"
            );
        }
        
        public static void PasteClipboardDataToGroup(BookmarksGroup groupData)
        {
            if (!BookmarksClipboard.HasData) return;

            Bookmarks clipboardBookmarks = BookmarksClipboard.Data;
            BookmarksGroup[] clipboardGroups = clipboardBookmarks.Groups;
            if (clipboardGroups == null) return;

            BookmarksUndo.BeforePasteBookmarks();
            for (int i = 0, iSize = clipboardGroups.Length; i < iSize; i++)
            {
                BookmarksGroup clipboardGroup = clipboardGroups[i];
                if (clipboardGroup == null) continue;
                groupData.AddBookmarks(clipboardGroup.Bookmarks);
            }

            BookmarksStorage.Get().Dirty = true;
            BookmarksStorage.Save();

            BookmarksWindow.ShowNotificationInAllWindows(
                $"Pasted {BookmarksClipboard.BuildBookmarksCountText(clipboardBookmarks.TotalBookmarksCount)} to {groupData.Name}"
            );
        }

        public static void PasteClipboardDataToActiveGroup()
        {
            Bookmarks bookmarks = BookmarksStorage.Get();
            BookmarksGroup[] bookmarksGroups = bookmarks.Groups;
            if (bookmarksGroups == null || bookmarksGroups.Length == 0) return;

            int activeGroupIndex = bookmarks.ActiveGroupIndex;
            PasteClipboardDataToGroup(bookmarksGroups[activeGroupIndex]);
        }
    }
}