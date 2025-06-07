using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// List presentation of BookmarksGroup
    public class BookmarksListGroupView : IBookmarksGroupView
    {
        private readonly BookmarksGroup _groupData;
        private readonly BookmarksViewState _state;
        private readonly BookmarksGroupViewEventsHandler _eventsHandler;

        private ReorderableList _bookmarksList;
        private BookmarkListView[] _bookmarkViews;

        public int BookmarksCount => _groupData.Bookmarks.Count;

        public BookmarksListGroupView(BookmarksGroup groupData, BookmarksViewState state)
        {
            _groupData = groupData;
            _state = state;
            _eventsHandler = new BookmarksGroupViewEventsHandler(groupData, _state);

            InitializeBookmarksList();
            InitializeBookmarksViews();
        }

        private void InitializeBookmarksViews()
        {
            List<Bookmark> bookmarks = _groupData.Bookmarks;
            _bookmarkViews = new BookmarkListView[bookmarks.Count];
            for (int i = 0, iSize = bookmarks.Count; i < iSize; i++)
            {
                BookmarkListView bookmarkView = new BookmarkListView(bookmarks[i]);
                _eventsHandler.SubscribeToBookmarkEvents(bookmarkView.Events);
                _bookmarkViews[i] = bookmarkView;
            }
        }

        private void InitializeBookmarksList()
        {
            _bookmarksList = new ReorderableList(
                _groupData.Bookmarks, typeof(Bookmark), true, false, false, false
            )
            {
                drawElementCallback = DrawBookmark,
                onReorderCallbackWithDetails = OnBookmarksReorder,
                onSelectCallback = OnBookmarkSelected
            };
        }

        private void OnBookmarksReorder(ReorderableList list, int oldIndex, int newIndex)
        {
            BookmarksStorage.Save();
            Bookmarks bookmarks = BookmarksStorage.Get();

            BookmarksUndo.BeforeSortBookmarks();
            _groupData.UpdateCustomSortIndices();
            bookmarks.SortType = SortType.Custom;

            bookmarks.Dirty = true;
        }

        private void OnBookmarkSelected(ReorderableList list)
        {
            int selectedBookmarkIndex = list.index;
            if (ReorderableListHelper.IsElementSelected(list, selectedBookmarkIndex))
            {
                _state.HandleBookmarkClick(selectedBookmarkIndex);
            }
        }

        private void DrawBookmark(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (index < 0 || index > _bookmarkViews.Length - 1) return;

            BookmarkListView bookmarkView = _bookmarkViews[index];
            bookmarkView.Draw(rect);

            // ReSharper disable once CoVariantArrayConversion
            BookmarkViewInteractionHandler.Handle(
                bookmarkView, index, rect, _bookmarkViews, _state
            );
        }

        public void Draw(Rect groupContentRect)
        {
            BookmarksListGroupShortcuts.HandleShortcuts(_groupData, _bookmarkViews, _state);
            UpdateSelectionState();
            _bookmarksList.DoLayoutList();
        }

        private void UpdateSelectionState()
        {
            if (!ReorderableListHelper.SelectionAllowed)
            {
                _state.CleanupSelectedBookmarks();
                return;
            }

            if (!_state.HasAnySelectedBookmark())
            {
                ReorderableListHelper.ClearSelection(_bookmarksList);
                return;
            }

            List<int> selectedIndices = _state.SelectedBookmarksIndices;
            ReorderableListHelper.Select(_bookmarksList, selectedIndices[0], false);

            for (int i = 1, iSize = selectedIndices.Count; i < iSize; i++)
            {
                ReorderableListHelper.Select(_bookmarksList, selectedIndices[i], true);
            }
        }
    }
}