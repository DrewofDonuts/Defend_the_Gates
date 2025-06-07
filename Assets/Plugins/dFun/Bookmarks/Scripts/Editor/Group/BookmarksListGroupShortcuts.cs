using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksListGroupShortcuts
    {
        public static void HandleShortcuts(
            BookmarksGroup groupData, BookmarkListView[] bookmarkViews, BookmarksViewState state)
        {
            HandleKeyboardShortcuts(groupData, bookmarkViews, state);
        }

        private static void HandleKeyboardShortcuts(
            BookmarksGroup groupData, BookmarkListView[] bookmarkViews, BookmarksViewState state)
        {
            if (!BookmarksGroupShortcuts.HandleKeyboardSelectAllShortcuts(bookmarkViews, state))
            {
                HandleKeyboardArrowKeys(bookmarkViews, state);
            }
            
            if (BookmarksGroupShortcuts.HandleKeyboardCopyPasteShortcuts(groupData, state))
            {
                return;
            }

            if (BookmarksGroupShortcuts.HandleKeyboardDeleteShortcuts(bookmarkViews, state))
            {
                return;
            }

            if (BookmarksGroupShortcuts.HandleKeyboardRenameShortcuts(bookmarkViews, state))
            {
                return;
            }

            BookmarksGroupShortcuts.HandleKeyboardEnterShortcuts(bookmarkViews, state);
        }

        private static void HandleKeyboardArrowKeys(BookmarkListView[] bookmarkViews, BookmarksViewState state)
        {
            const bool needRepaint = true;
            int bookmarksAmount = bookmarkViews.Length;

            if (bookmarksAmount == 0)
            {
                return;
            }

            if (KeyboardHelper.WasDownButtonPressed())
            {
                int newSelectedBookmark = state.HasAnySelectedBookmark()
                    ? state.GetLastSelectedBookmark() + 1
                    : 0;

                newSelectedBookmark = Mathf.Clamp(
                    newSelectedBookmark, 0, bookmarksAmount - 1
                );
                BookmarksGroupShortcuts.TryToSelectBookmarkByIndex(
                    newSelectedBookmark, bookmarksAmount, state, needRepaint
                );
                Event.current.Use();
                return;
            }

            if (KeyboardHelper.WasUpButtonPressed())
            {
                bool hasAnySelectedBookmark = state.HasAnySelectedBookmark();
                if (!hasAnySelectedBookmark) return;

                int newSelectedBookmark = state.GetLastSelectedBookmark() - 1;

                BookmarksGroupShortcuts.TryToSelectBookmarkByIndex(
                    newSelectedBookmark, bookmarksAmount, state, needRepaint
                );
                Event.current.Use();
                return;
            }
        }
    }
}