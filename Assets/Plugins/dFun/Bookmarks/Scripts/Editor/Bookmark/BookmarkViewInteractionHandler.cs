using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarkViewInteractionHandler
    {
        public static void Handle(BaseBookmarkView bookmarkView,
            int bookmarkIndex,
            Rect bookmarkRect,
            BaseBookmarkView[] groupBookmarkViews,
            BookmarksViewState bookmarksViewState)
        {
            BookmarkInteractionState interactionState = bookmarkView.InteractionState;

            if (interactionState.DragStarted)
            {
                DragAndDropHelper.StartDrag(bookmarkIndex, groupBookmarkViews, bookmarksViewState);
                return;
            }

            if (interactionState.WasDoubleClicked)
            {
                if (bookmarkView.BookmarkData.Resolve(out Object resolvedObject))
                {
                    bookmarksViewState.SetSelectedBookmark(bookmarkIndex);
                    BookmarkSelectionHelper.HandleDoubleClick(resolvedObject);
                    return;
                }
            }

            if (interactionState.ReadyToShowContext
                || (interactionState.WasContextClicked && bookmarksViewState.IsBookmarkSelected(bookmarkIndex)))
            {
                interactionState.OnShowContext();
                Rect guiRect = Rect.MinMaxRect(
                    Event.current.mousePosition.x, bookmarkRect.min.y,
                    Event.current.mousePosition.x + bookmarkRect.height, bookmarkRect.max.y
                );
                Rect screenRect = bookmarksViewState.ParentWindow.ToScreenSpaceRect(guiRect);
                bookmarkView.ShowBookmarkContextMenu(screenRect);
                return;
            }

            if (interactionState.WasClicked && !interactionState.WasContextClicked)
            {
                BookmarkSelectionHelper.HandleBookmarkClick(groupBookmarkViews, bookmarkIndex, bookmarksViewState);
            }

            bookmarksViewState.OnDrawBookmark(bookmarkIndex, bookmarkRect);
        }
    }
}