using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// Visual presentation of Bookmark in Grid view
    public class BookmarkGridView : BaseBookmarkView
    {
        public BookmarkGridView(Bookmark bookmark) : base(bookmark)
        {
        }

        public Rect Draw(float size, bool isSelected)
        {
            UpdateEditingState();
            InteractionState.Reset();

            Rect rect = EditorGUILayout.BeginVertical();
            SetDrawRect(rect);
            {
                HandleDragAndDropOut(rect);
                HandleClick(rect);

                GetStyles(IsEditing, isSelected, out GUIStyle labelStyle, out GUIStyle previewStyle);

                EditorGUILayout.LabelField(
                    new GUIContent(BookmarkData.PreviewIcon, BookmarkData.BookmarkName), previewStyle,
                    GUILayout.Width(size), GUILayout.Height(size)
                );
                EditorGUILayout.LabelField(
                    new GUIContent(DefineVisibleName(size), BookmarkData.BookmarkName),
                    labelStyle,
                    GUILayout.Width(size)
                );

                if (BookmarkData.ObjectReference.InvokesReference.ContainsData)
                {
                    const float minIconSize = 30f;
                    const float maxIconSize = 40f;
                    float invokesIconSize = Mathf.Clamp(size / 2f, minIconSize, maxIconSize);

                    const float minIconOffset = 0f;
                    const float maxIconOffset = 15f;
                    float addOffset = MathUtils.RemapLinear(
                        size,
                        BookmarksGridGroupView.MinIconSize, BookmarksGridGroupView.MaxGridSize,
                        minIconOffset, maxIconOffset
                    );

                    Rect iconButtonRect = new Rect(
                        rect.x + rect.width - invokesIconSize - addOffset,
                        rect.y + rect.height - invokesIconSize - EditorGUIUtility.singleLineHeight - addOffset,
                        invokesIconSize,
                        invokesIconSize
                    );

                    GUI.Label(iconButtonRect, Icons.InvokesIconBg);
                    GUI.Label(iconButtonRect, Icons.InvokesIcon);
                }
            }
            EditorGUILayout.EndVertical();

            return rect;
        }

        private string DefineVisibleName(float rectSize)
        {
            string bookmarkName = BookmarkData.BookmarkName;
            int showSymbolsAmount = (int)(rectSize / 10);

            if (bookmarkName.Length <= showSymbolsAmount + 3)
            {
                return bookmarkName;
            }

            return bookmarkName.Substring(0, showSymbolsAmount) + "...";
        }

        private void HandleClick(Rect bookmarksButtonRect)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type != EventType.MouseDown) return;

            if (bookmarksButtonRect.Contains(currentEvent.mousePosition))
            {
                OnBookmarkButtonClick();
            }
        }

        private void GetStyles(bool isEditing, bool isSelected, out GUIStyle labelStyle, out GUIStyle previewStyle)
        {
            if (isEditing)
            {
                labelStyle = Styles.BookmarkLabelStyleEditGridView;
                previewStyle = Styles.BookmarkPreviewStyleEditGridView;
                return;
            }

            if (isSelected)
            {
                labelStyle = Styles.BookmarkLabelStyleSelectedGridView;
                previewStyle = Styles.BookmarkPreviewStyleSelectedGridView;
                return;
            }

            labelStyle = Styles.BookmarkLabelStyleGridView;
            previewStyle = Styles.BookmarkPreviewStyleGridView;
        }
    }
}