using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public abstract class BaseBookmarkView
    {
        public readonly Bookmark BookmarkData;

        protected bool IsEditing { get; private set; }

        private BookmarkViewContextMenu _contextMenu;
        private BookmarkViewContextMenu ContextMenu
        {
            get
            {
                if (_contextMenu == null) _contextMenu = new BookmarkViewContextMenu(this);
                return _contextMenu;
            }
        }

        private Vector2 _lastMouseDownPos;
        private Rect _lastDrawRect;

        private double _lastClickTime;

        public readonly BookmarkInteractionState InteractionState = new BookmarkInteractionState();
        public readonly BookmarkViewEvents Events = new BookmarkViewEvents();

        public BaseBookmarkView(Bookmark bookmark)
        {
            BookmarkData = bookmark;
        }

        protected void SetDrawRect(Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                _lastDrawRect = rect;
            }
        }

        protected void HandleDragAndDropOut(Rect buttonRect)
        {
            Event e = Event.current;
            EventType eType = e.type;

            if (eType == EventType.MouseDown)
            {
                _lastMouseDownPos = e.mousePosition;
            }

            if (eType != EventType.MouseDrag) return;

            if (buttonRect.Contains(_lastMouseDownPos) && buttonRect.Contains(e.mousePosition))
            {
                InteractionState.DragStarted = true;
            }
        }

        protected void UpdateEditingState()
        {
            if (!IsEditing)
            {
                return;
            }

            if (_contextMenu == null)
            {
                return;
            }

            if (!ContextMenu.IsOpened)
            {
                IsEditing = false;
            }
        }

        protected void OnBookmarkButtonClick()
        {
            bool isDoubleClick = MouseHelper.WasDoubleClicked(ref _lastClickTime);
            if (Event.current.button == 0)
            {
                InteractionState.WasClicked = true;
                if (isDoubleClick && BookmarkData.Resolve(out Object _))
                {
                    InteractionState.WasDoubleClicked = true;
                }

                return;
            }

            if (Event.current.button == 1)
            {
                InteractionState.WasClicked = true;
                InteractionState.WasContextClicked = true;
            }
        }

        public void ShowBookmarkContextMenu(Rect buttonScreenRect)
        {
            IsEditing = true;
            ContextMenu.Show(buttonScreenRect);
        }

        public void ShowBookmarkContextMenuForRenaming()
        {
            IsEditing = true;
            ContextMenu.ShowSettingsPopup(_lastDrawRect, true);
        }

        public void RemoveBookmark()
        {
            BookmarksUndo.BeforeBookmarkRemoved();
            BookmarksStorage.Get().Remove(BookmarkData);
            BookmarksStorage.Save();
        }
    }
}