using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// Visual presentation of Bookmark in List view
    public class BookmarkListView : BaseBookmarkView
    {
        private GUIStyle BookmarkButtonStyle =>
            IsEditing ? Styles.BookmarkButtonStyleEditListView : Styles.BookmarkButtonStyleListView;

        private bool IsDrawIcon => BookmarksStorage.Get().Settings.ViewSettings.DrawIconsInListView;

        public BookmarkListView(Bookmark bookmark) : base(bookmark)
        {
        }

        public void Draw(Rect rect)
        {
            SetDrawRect(rect);
            UpdateEditingState();
            InteractionState.Reset();
            const float removeButtonWidth = 30;
            const float wideViewMinWidth = 110;
            const float verticalOffset = 1;

            bool isWideView = rect.width > wideViewMinWidth;

            Rect bookmarksButtonRect = rect;

            float xPosOffset = 0f;

            if (IsDrawIcon)
            {
                const float iconSize = 20;
                bookmarksButtonRect.x += iconSize;
                bookmarksButtonRect.width -= iconSize;
                Rect iconButtonRect = new Rect(
                    rect.x, rect.y + verticalOffset, iconSize, iconSize
                );
                xPosOffset += iconSize;
                GUI.Label(iconButtonRect, BookmarkData.Icon);
            }

            if (BookmarkData.ObjectReference.InvokesReference.ContainsData)
            {
                const float invokesIconSize = 15f;
                const float offsetMultiplier = 0.9f;
                bookmarksButtonRect.x += invokesIconSize * offsetMultiplier;
                bookmarksButtonRect.width -= invokesIconSize * offsetMultiplier;
                Rect iconButtonRect = new Rect(
                    rect.x + xPosOffset * offsetMultiplier,
                    rect.y + (rect.height - invokesIconSize) / 2f,
                    invokesIconSize,
                    invokesIconSize
                );
                // xPosOffset += invokesIconSize;

                GUI.Label(iconButtonRect, Icons.InvokesIcon);
            }

            if (isWideView)
            {
                bookmarksButtonRect.width -= removeButtonWidth;
            }

            Rect removeButtonRect = new Rect(
                rect.x + rect.width - removeButtonWidth, rect.y + verticalOffset,
                removeButtonWidth, EditorGUIUtility.singleLineHeight
            );

            HandleDragAndDropOut(bookmarksButtonRect);
            if (InteractionState.DragStarted)
            {
                return;
            }

            if (GUI.Button(bookmarksButtonRect, BookmarkData.BookmarkName, BookmarkButtonStyle))
            {
                OnBookmarkButtonClick();
            }

            if (isWideView)
            {
                if (GUI.Button(removeButtonRect, Icons.RemoveIcon, Styles.BookmarkRemoveButtonStyleListView))
                {
                    RemoveBookmark();
                }
            }
        }
    }
}