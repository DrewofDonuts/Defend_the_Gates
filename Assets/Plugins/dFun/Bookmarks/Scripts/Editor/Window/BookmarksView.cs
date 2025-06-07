using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// Visual presentation of Bookmarks
    public class BookmarksView
    {
        private readonly BookmarksViewState _state;

        private Bookmarks Bookmarks => BookmarksStorage.Get();
        private bool IsShowSingleTab => Bookmarks.Settings.ViewSettings.ShowSingleBookmarksTab;

        private readonly BookmarksToolbar _toolbar = new BookmarksToolbar();
        private BookmarksGroupView[] _tabs;

        private readonly DragAndDropZone _dragAndDropZone = new DragAndDropZone();

        private CleanupBookmarksPopup _settingsPopup;

        public BookmarksView(BookmarksViewState state)
        {
            _state = state;
        }

        public void Initialize()
        {
            InitializeTabs();
            DragAndDropHelper.Clear();
        }

        public void Draw(Rect rect)
        {
            GUILayout.BeginArea(rect);
            {
                if (Bookmarks.Dirty)
                {
                    Bookmarks.Dirty = false;
                    Initialize();
                }

                if (_tabs.Length == 0)
                {
                    _dragAndDropZone.Draw();
                    _state.Reset();
                }
                else
                {
                    DrawTabs(rect);
                }

                if (_dragAndDropZone.DoUpdate(Bookmarks))
                {
                    BookmarksStorage.Save();
                }
            }
            GUILayout.EndArea();
        }

        private void DrawTabs(Rect rect)
        {
            if (_tabs.Length == 1 && !IsShowSingleTab)
            {
                DrawTabContent(_tabs[0], rect);
                return;
            }

            int activeGroupIndex = Bookmarks.ActiveGroupIndex;
            int newActiveGroupIndex = _toolbar.Draw(activeGroupIndex, _tabs);
            if (newActiveGroupIndex != activeGroupIndex)
            {
                _state.CleanupSelectedBookmarks();
                Bookmarks.ActiveGroupIndex = newActiveGroupIndex;
                BookmarksStorage.Get().Dirty = true;
                BookmarksStorage.Save();
            }

            if (newActiveGroupIndex >= 0 && newActiveGroupIndex <= _tabs.Length - 1)
            {
                float tabsHeight = EditorGUIUtility.singleLineHeight;
                Rect contentRect = new Rect( //without tabs on top
                    rect.x, rect.y + tabsHeight, rect.width, rect.height - tabsHeight
                );
                DrawTabContent(_tabs[newActiveGroupIndex], contentRect);
            }
        }

        private void DrawTabContent(BookmarksGroupView groupView, Rect contentRect)
        {
            if (groupView.BookmarksCount == 0)
            {
                _dragAndDropZone.Draw();
                _state.Reset();
            }
            else
            {
                groupView.Draw(contentRect);
            }
        }

        private void InitializeTabs()
        {
            BookmarksGroup[] bookmarksGroups = Bookmarks.Groups;
            _tabs = new BookmarksGroupView[bookmarksGroups.Length];
            for (int i = 0, iSize = bookmarksGroups.Length; i < iSize; i++)
            {
                _tabs[i] = new BookmarksGroupView(Bookmarks, i, _state);
            }
        }
    }
}