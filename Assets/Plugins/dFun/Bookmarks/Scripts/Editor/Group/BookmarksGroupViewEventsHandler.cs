using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class BookmarksGroupViewEventsHandler
    {
        private readonly BookmarksGroup _groupData;
        private readonly BookmarksViewState _state;

        public BookmarksGroupViewEventsHandler(BookmarksGroup groupData, BookmarksViewState state)
        {
            _groupData = groupData;
            _state = state;
        }

        public void SubscribeToBookmarkEvents(BookmarkViewEvents bookmarkViewEvents)
        {
            bookmarkViewEvents.onCopyInput -= OnCopy;
            bookmarkViewEvents.onCopyInput += OnCopy;

            bookmarkViewEvents.onCopyCompressedInput -= OnCopyCompressed;
            bookmarkViewEvents.onCopyCompressedInput += OnCopyCompressed;

            bookmarkViewEvents.onPasteInput -= OnPaste;
            bookmarkViewEvents.onPasteInput += OnPaste;

            bookmarkViewEvents.onRemoveInput -= OnRemove;
            bookmarkViewEvents.onRemoveInput += OnRemove;
        }

        private void OnCopy()
        {
            OnCopy(false);
        }

        private void OnCopyCompressed()
        {
            OnCopy(true);
        }

        private void OnCopy(bool compressed)
        {
            BookmarksGroupClipboardHelper.CopySelectedBookmarks(_groupData, _state, compressed);
        }

        private void OnPaste()
        {
            BookmarksGroupClipboardHelper.PasteClipboardDataToGroup(_groupData);
        }

        private void OnRemove()
        {
            if (!_state.HasAnySelectedBookmark()) return;

            List<int> stateSelectedBookmarksIndices = _state.SelectedBookmarksIndices;

            BookmarksUndo.BeforeBookmarkRemoved();
            BookmarksStorage.Get().RemoveBookmarksFromGroup(_groupData, stateSelectedBookmarksIndices);

            _state.CleanupSelectedBookmarks();
        }
    }
}