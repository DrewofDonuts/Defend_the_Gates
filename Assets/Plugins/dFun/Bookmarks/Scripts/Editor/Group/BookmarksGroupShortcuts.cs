using System.Collections.Generic;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksGroupShortcuts
    {
        public static bool HandleKeyboardSelectAllShortcuts<T>(T[] bookmarkViews, BookmarksViewState state)
            where T : BaseBookmarkView
        {
            if (KeyboardHelper.WasKeyPressed(KeyCode.A) && KeyboardHelper.IsControlModifierPressed())
            {
                state.CleanupSelectedBookmarks();

                for (int i = 0, iSize = bookmarkViews.Length; i < iSize; i++)
                {
                    state.AddSelectedBookmark(i);
                }

                state.NeedRepaint = true;

                return true;
            }

            return false;
        }

        public static bool HandleKeyboardCopyPasteShortcuts(BookmarksGroup groupData, BookmarksViewState state)
        {
            if (WasKeyboardCopyPressed())
            {
                BookmarksGroupClipboardHelper.CopySelectedBookmarks(groupData, state, false);
                return true;
            }

            if (WasKeyboardPastePressed())
            {
                BookmarksGroupClipboardHelper.PasteClipboardDataToGroup(groupData);
                BookmarksWindow.ForceRepaintAllWindows();
                return true;
            }

            return false;
        }

        public static bool WasKeyboardCopyPressed()
        {
            return KeyboardHelper.WasKeyPressed(KeyCode.C) && KeyboardHelper.IsControlModifierPressed();
        }

        public static bool WasKeyboardPastePressed()
        {
            return KeyboardHelper.WasKeyPressed(KeyCode.V) && KeyboardHelper.IsControlModifierPressed();
        }

        public static bool HandleKeyboardDeleteShortcuts<T>(T[] bookmarkViews, BookmarksViewState state)
            where T : BaseBookmarkView
        {
            if (!state.HasAnySelectedBookmark()) return false;

            if (KeyboardHelper.WasDeleteButtonPressed())
            {
                DeleteSelectedBookmarks(bookmarkViews, state, 0);
                return true;
            }

            if (KeyboardHelper.WasBackspaceButtonPressed())
            {
                DeleteSelectedBookmarks(bookmarkViews, state, -1);
                return true;
            }

            return false;
        }

        public static bool HandleKeyboardRenameShortcuts<T>(T[] bookmarkViews, BookmarksViewState state)
            where T : BaseBookmarkView
        {
            if (!state.HasAnySelectedBookmark())
            {
                return false;
            }

            if (KeyboardHelper.WasRenameButtonPressed())
            {
                int selectedBookmarkIndex = state.GetLastSelectedBookmark();
                if (BookmarkSelectionHelper.IsValidBookmarkIndex(selectedBookmarkIndex, bookmarkViews))
                {
                    bookmarkViews[selectedBookmarkIndex].ShowBookmarkContextMenuForRenaming();
                }

                Event.current.Use();
                return true;
            }

            return false;
        }

        public static bool HandleKeyboardEnterShortcuts<T>(T[] bookmarkViews, BookmarksViewState state)
            where T : BaseBookmarkView
        {
            if (!state.HasAnySelectedBookmark()) return false;

            if (KeyboardHelper.WasEnterButtonPressed())
            {
                BookmarkSelectionHelper.OpenBookmarks(bookmarkViews, state);
                return true;
            }

            return false;
        }

        private static void DeleteSelectedBookmarks<T>(T[] bookmarkViews, BookmarksViewState state, int selectionOffset)
            where T : BaseBookmarkView
        {
            if (!state.HasAnySelectedBookmark()) return;

            HashSet<BaseBookmarkView> bookmarksToDelete = new HashSet<BaseBookmarkView>();
            List<int> bookmarksIndicesToDelete = state.SelectedBookmarksIndices;
            for (int i = 0, iSize = bookmarksIndicesToDelete.Count; i < iSize; i++)
            {
                int bookmarkIndex = bookmarksIndicesToDelete[i];
                if (BookmarkSelectionHelper.IsValidBookmarkIndex(bookmarkIndex, bookmarkViews))
                {
                    bookmarksToDelete.Add(bookmarkViews[bookmarkIndex]);
                }
            }

            int newBookmarksAmount = bookmarkViews.Length;
            foreach (BaseBookmarkView bookmarkView in bookmarksToDelete)
            {
                bookmarkView.RemoveBookmark();
                newBookmarksAmount--;
            }

            int bookmarkIndexToReselect = Mathf.Clamp(
                bookmarksIndicesToDelete[0] + selectionOffset, 0, newBookmarksAmount - 1
            );
            state.CleanupSelectedBookmarks();
            TryToSelectBookmarkByIndex(
                bookmarkIndexToReselect, newBookmarksAmount, state, true
            );
        }

        public static void TryToSelectBookmarkByIndex(
            int bookmarkIndex, int bookmarksAmount, BookmarksViewState state, bool needRepaint)
        {
            state.NeedRepaint = needRepaint;

            if (bookmarkIndex < 0) return;
            if (bookmarkIndex > bookmarksAmount - 1) return;

            state.HandleSelectedKeyboardSelect(bookmarkIndex);
        }
    }
}