using System.Collections.Generic;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksViewState
    {
        public bool NeedRepaint { get; set; }

        private readonly List<int> _selectedBookmarksIndices = new List<int>(1);
        public List<int> SelectedBookmarksIndices => _selectedBookmarksIndices;

        private int _forceScrollToRectIndex;
        public OptionalRect ForceScrollToRect { get; private set; }

        public readonly BookmarksWindow ParentWindow;

        public BookmarksViewState(BookmarksWindow parentWindow)
        {
            ParentWindow = parentWindow;
        }

        /// <returns>Was bookmark selected?</returns>
        public bool HandleBookmarkClick(int bookmarkIndex)
        {
            bool wasBookmarkSelected;

            if (KeyboardHelper.IsShiftModifierPressed())
            {
                wasBookmarkSelected = HandleBookmarkClickWithShiftModifier(bookmarkIndex);
            }
            else if (KeyboardHelper.IsControlModifierPressed())
            {
                wasBookmarkSelected = HandleBookmarkClickWithControlModifier(bookmarkIndex);
            }
            else
            {
                wasBookmarkSelected = HandleBookmarkClickWithoutModifiers(bookmarkIndex);
            }

            SetForceScrollToRect(bookmarkIndex);
            return wasBookmarkSelected;
        }

        private bool HandleBookmarkClickWithShiftModifier(int bookmarkIndex)
        {
            if (!HasAnySelectedBookmark())
            {
                return HandleBookmarkClickWithoutModifiers(bookmarkIndex);
            }

            int firstSelectedBookmark = GetFirstSelectedBookmark();
            int lastSelectedBookmark = bookmarkIndex;
            if (lastSelectedBookmark == firstSelectedBookmark)
            {
                return HandleBookmarkClickWithoutModifiers(lastSelectedBookmark);
            }

            SetSelectedBookmark(firstSelectedBookmark);

            if (lastSelectedBookmark > firstSelectedBookmark)
            {
                for (int index = firstSelectedBookmark + 1; index <= lastSelectedBookmark; index++)
                {
                    AddSelectedBookmark(index);
                }
            }
            else
            {
                for (int index = firstSelectedBookmark - 1; index >= lastSelectedBookmark; index--)
                {
                    AddSelectedBookmark(index);
                }
            }

            return true;
        }

        private bool HandleBookmarkClickWithControlModifier(int bookmarkIndex)
        {
            bool wasBookmarkSelected = false;
            if (IsBookmarkSelected(bookmarkIndex))
            {
                RemoveSelectedBookmark(bookmarkIndex);
            }
            else
            {
                AddSelectedBookmark(bookmarkIndex);
                wasBookmarkSelected = true;
            }

            return wasBookmarkSelected;
        }

        private bool HandleBookmarkClickWithoutModifiers(int bookmarkIndex)
        {
            SetSelectedBookmark(bookmarkIndex);
            return true;
        }

        public void HandleSelectedKeyboardSelect(int bookmarkIndex)
        {
            if (KeyboardHelper.IsShiftModifierPressed())
            {
                HandleSelectedKeyboardAppend(bookmarkIndex);
            }
            else
            {
                SetSelectedBookmark(bookmarkIndex);
            }
            SetForceScrollToRect(bookmarkIndex);
        }

        private void HandleSelectedKeyboardAppend(int bookmarkIndex)
        {
            if (IsBookmarkSelected(bookmarkIndex))
            {
                int lastSelectedBookmark = GetLastSelectedBookmark();
                if (lastSelectedBookmark != bookmarkIndex)
                {
                    RemoveSelectedBookmark(lastSelectedBookmark);
                }
            }
            else
            {
                AddSelectedBookmark(bookmarkIndex);
            }
        }

        public void SetSelectedBookmark(int bookmarkIndex)
        {
            CleanupSelectedBookmarks();
            _selectedBookmarksIndices.Add(bookmarkIndex);
        }

        public bool HasAnySelectedBookmark()
        {
            return _selectedBookmarksIndices.Count > 0;
        }

        public int GetFirstSelectedBookmark()
        {
            return _selectedBookmarksIndices[0];
        }

        public int GetLastSelectedBookmark()
        {
            return _selectedBookmarksIndices[_selectedBookmarksIndices.Count - 1];
        }

        public bool IsBookmarkSelected(int bookmarkIndex)
        {
            return _selectedBookmarksIndices.Contains(bookmarkIndex);
        }

        public void AddSelectedBookmark(int bookmarkIndex)
        {
            if (!_selectedBookmarksIndices.Contains(bookmarkIndex))
            {
                _selectedBookmarksIndices.Add(bookmarkIndex);
            }
        }

        public void RemoveSelectedBookmark(int bookmarkIndex)
        {
            _selectedBookmarksIndices.Remove(bookmarkIndex);
        }

        public void CleanupSelectedBookmarks()
        {
            _selectedBookmarksIndices.Clear();
        }

        private void SetForceScrollToRect(int bookmarkIndex)
        {
            _forceScrollToRectIndex = bookmarkIndex;
        }

        public void OnDrawBookmark(int bookmarkIndex, Rect bookmarkRect)
        {
            if (bookmarkIndex == _forceScrollToRectIndex)
            {
                ForceScrollToRect = new OptionalRect(bookmarkRect);
            }
        }

        public void Reset()
        {
            ResetForceScrollToRect();
            CleanupSelectedBookmarks();
        }

        public void ResetForceScrollToRect()
        {
            _forceScrollToRectIndex = -1;
            ForceScrollToRect = OptionalRect.None;
        }

        public List<Bookmark> GetSelectedBookmarks(BookmarksGroup group)
        {
            List<int> selectedBookmarksIndices = SelectedBookmarksIndices;
            List<Bookmark> selectedBookmarks = new List<Bookmark>(selectedBookmarksIndices.Count);

            for (int i = 0, iSize = selectedBookmarksIndices.Count; i < iSize; i++)
            {
                int selectedBookmarkIndex = selectedBookmarksIndices[i];
                if (group.IsValidBookmarkIndex(selectedBookmarkIndex))
                {
                    Bookmark selectedBookmark = group.Bookmarks[selectedBookmarkIndex];
                    selectedBookmarks.Add(selectedBookmark);
                }
            }

            return selectedBookmarks;
        }
    }
}