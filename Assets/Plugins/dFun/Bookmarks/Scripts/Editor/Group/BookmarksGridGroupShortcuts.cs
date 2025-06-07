using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksGridGroupShortcuts
    {
        public static void HandleShortcuts(
            BookmarksGroup groupData, BookmarkGridView[] bookmarkViews, BookmarksViewState state, int columnsAmount)
        {
            HandleKeyboardShortcuts(groupData, bookmarkViews, state, columnsAmount);
        }

        private static void HandleKeyboardShortcuts(
            BookmarksGroup groupData, BookmarkGridView[] bookmarkViews, BookmarksViewState state, int columnsAmount)
        {
            if (!BookmarksGroupShortcuts.HandleKeyboardSelectAllShortcuts(bookmarkViews, state))
            {
                HandleKeyboardArrowKeys(bookmarkViews, state, columnsAmount);
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

        private static void HandleKeyboardArrowKeys(
            BookmarkGridView[] bookmarkViews, BookmarksViewState state, int columnsAmount)
        {
            const bool needRepaint = true;
            int bookmarksAmount = bookmarkViews.Length;

            if (bookmarksAmount == 0)
            {
                return;
            }

            if (KeyboardHelper.WasRightButtonPressed())
            {
                int newSelectedBookmark = state.HasAnySelectedBookmark()
                    ? state.GetLastSelectedBookmark() + 1
                    : 0;
                BookmarksGroupShortcuts.TryToSelectBookmarkByIndex(
                    newSelectedBookmark, bookmarksAmount, state, needRepaint
                );
                return;
            }

            if (KeyboardHelper.WasLeftButtonPressed())
            {
                bool hasAnySelectedBookmark = state.HasAnySelectedBookmark();
                if (!hasAnySelectedBookmark) return;

                int newSelectedBookmark = Mathf.Clamp(
                    state.GetLastSelectedBookmark() - 1, 0, bookmarksAmount - 1
                );
                BookmarksGroupShortcuts.TryToSelectBookmarkByIndex(
                    newSelectedBookmark, bookmarksAmount, state, needRepaint
                );
                return;
            }

            if (KeyboardHelper.WasDownButtonPressed())
            {
                int newSelectedBookmark = state.HasAnySelectedBookmark()
                    ? state.GetLastSelectedBookmark() + columnsAmount
                    : 0;

                newSelectedBookmark = Mathf.Clamp(
                    newSelectedBookmark, 0, bookmarksAmount - 1
                );
                BookmarksGroupShortcuts.TryToSelectBookmarkByIndex(
                    newSelectedBookmark, bookmarksAmount, state, needRepaint
                );
                return;
            }

            if (KeyboardHelper.WasUpButtonPressed())
            {
                bool hasAnySelectedBookmark = state.HasAnySelectedBookmark();
                if (!hasAnySelectedBookmark) return;

                int newSelectedBookmark = state.GetLastSelectedBookmark() - columnsAmount;

                BookmarksGroupShortcuts.TryToSelectBookmarkByIndex(
                    newSelectedBookmark, bookmarksAmount, state, needRepaint
                );
                return;
            }
        }
    }
}