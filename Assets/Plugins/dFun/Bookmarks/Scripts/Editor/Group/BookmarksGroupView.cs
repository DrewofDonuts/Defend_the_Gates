using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// Visual presentation of BookmarksGroup
    public class BookmarksGroupView : IBookmarksGroupView
    {
        private readonly BookmarksGroup _groupData;
        private readonly BookmarksViewState _state;
        public BookmarksViewState State => _state;

        private readonly BookmarksListGroupView _listGroupView;
        private readonly BookmarksGridGroupView _gridGroupView;

        private Bookmarks Bookmarks => BookmarksStorage.Get();
        private IBookmarksGroupView ActiveView
        {
            get
            {
                if (Bookmarks.GridSizeNormalized <= 0f)
                {
                    return _listGroupView;
                }

                return _gridGroupView;
            }
        }

        private BookmarksGroupViewContextMenu _contextMenu;
        private BookmarksGroupViewContextMenu ContextMenu
        {
            get
            {
                if (_contextMenu == null) _contextMenu = new BookmarksGroupViewContextMenu(_groupData);
                return _contextMenu;
            }
        }

        public int BookmarksCount => ActiveView.BookmarksCount;
        public GUIContent TabHeader { get; }

        private Vector2 _scrollPos;

        public BookmarksGroupView(Bookmarks bookmarks, int groupIndex, BookmarksViewState state)
        {
            _groupData = bookmarks.Groups[groupIndex];
            string groupViewName = _groupData.Name;
            if (string.IsNullOrEmpty(groupViewName))
            {
                groupViewName = (groupIndex + 1).ToString();
            }

            TabHeader = new GUIContent(groupViewName, _groupData.Description);

            _listGroupView = new BookmarksListGroupView(_groupData, state);
            _gridGroupView = new BookmarksGridGroupView(_groupData, state);

            _state = state;
        }

        public void Draw(Rect groupContentRect)
        {
            _scrollPos = EditorGUILayout.BeginScrollView(
                CalculateScrollPosition(groupContentRect), GUIStyle.none, GUI.skin.verticalScrollbar
            );

            ActiveView.Draw(groupContentRect);
            EditorGUILayout.EndScrollView();
        }

        private Vector2 CalculateScrollPosition(Rect contentRect)
        {
            Vector2 targetScrollRect;

            OptionalRect forceScrollToPosition = _state.ForceScrollToRect;
            if (forceScrollToPosition == null || !forceScrollToPosition.Exists)
            {
                targetScrollRect = _scrollPos;
            }
            else
            {
                targetScrollRect = ScrollHelper.BringChildIntoViewVertical(
                    forceScrollToPosition.Data, contentRect, _scrollPos
                );
            }

            _state.ResetForceScrollToRect();
            return targetScrollRect;
        }

        public void ShowContextMenu(Rect buttonScreenRect)
        {
            ContextMenu.Show(buttonScreenRect);
        }
        
        public void ShowSettingsPopup(Rect buttonScreenRect)
        {
            ContextMenu.ShowSettingsPopup(buttonScreenRect);
        }
    }
}