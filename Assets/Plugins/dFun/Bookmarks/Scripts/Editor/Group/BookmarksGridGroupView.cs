using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// Grid presentation of BookmarksGroup
    public class BookmarksGridGroupView : IBookmarksGroupView
    {
        public const int MinIconSize = 32 + 18 + GridColumnSpace; //18 == EditorGUIUtility.singleLineHeight;
        public const int MaxGridSize = 96 + 18 + GridColumnSpace;
        private const int GridColumnSpace = 5;
        private const float GridRowSpaceMultiplier = 0.075f;

        private readonly BookmarksGroup _groupData;
        private readonly BookmarksViewState _state;
        private readonly BookmarksGroupViewEventsHandler _eventsHandler;

        private BookmarkGridView[] _bookmarkViews;

        public int BookmarksCount => _groupData.Bookmarks.Count;

        private int GridSize
        {
            get
            {
                float normalizedGridSize = BookmarksStorage.Get().GridSizeNormalized;
                return (int)(Mathf.Lerp(MinIconSize, MaxGridSize, normalizedGridSize));
            }
        }

        public BookmarksGridGroupView(BookmarksGroup groupData, BookmarksViewState state)
        {
            _groupData = groupData;
            _state = state;
            _eventsHandler = new BookmarksGroupViewEventsHandler(groupData, _state);
            InitializeBookmarksViews();
        }

        private void InitializeBookmarksViews()
        {
            List<Bookmark> bookmarks = _groupData.Bookmarks;
            _bookmarkViews = new BookmarkGridView[bookmarks.Count];
            for (int i = 0, iSize = bookmarks.Count; i < iSize; i++)
            {
                BookmarkGridView bookmarkGridView = new BookmarkGridView(bookmarks[i]);
                _eventsHandler.SubscribeToBookmarkEvents(bookmarkGridView.Events);
                _bookmarkViews[i] = bookmarkGridView;
            }
        }

        public void Draw(Rect groupContentRect)
        {
            float gridWidth = GridSize;

            int bookmarksAmount = _bookmarkViews.Length;
            float contentWidth = groupContentRect.width;

            float gridWidthWidthSpace = gridWidth + GridColumnSpace;
            const float verticalScrollBarWidth = 20f;
            contentWidth -= verticalScrollBarWidth;
            int columnsAmount = (int)(contentWidth / gridWidthWidthSpace);
            int rowsAmount = Mathf.CeilToInt(bookmarksAmount / (float)columnsAmount);

            BookmarksGridGroupShortcuts.HandleShortcuts(_groupData, _bookmarkViews, _state, columnsAmount);

            EditorGUILayout.BeginVertical();
            for (int row = 0; row < rowsAmount; row++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int column = 0; column < columnsAmount; column++)
                {
                    int bookmarkIndex = row * columnsAmount + column;
                    if (bookmarkIndex > bookmarksAmount - 1)
                    {
                        break;
                    }

                    EditorGUILayout.Space(GridColumnSpace);
                    DrawBookmark(gridWidth, bookmarkIndex);
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                if (row < rowsAmount - 1)
                {
                    GUILayout.Space(gridWidth * GridRowSpaceMultiplier);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawBookmark(float size, int index)
        {
            bool isSelected = _state.IsBookmarkSelected(index);
            BookmarkGridView bookmarkView = _bookmarkViews[index];
            Rect rect = bookmarkView.Draw(size, isSelected);

            // ReSharper disable once CoVariantArrayConversion
            BookmarkViewInteractionHandler.Handle(
                bookmarkView, index, rect, _bookmarkViews, _state
            );
        }
    }
}