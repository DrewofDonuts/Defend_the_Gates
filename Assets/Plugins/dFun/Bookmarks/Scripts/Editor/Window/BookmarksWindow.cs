using System;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    [Serializable]
    public class BookmarksWindow : EditorWindow, IHasCustomMenu
    {
        private BookmarksView _bookmarksView;
        private BookmarksView BookmarksView
        {
            get
            {
                if (_bookmarksView == null) _bookmarksView = new BookmarksView(BookmarksViewState);
                return _bookmarksView;
            }
        }

        private BookmarksViewState _bookmarksViewState;
        private BookmarksViewState BookmarksViewState
        {
            get
            {
                if (_bookmarksViewState == null) _bookmarksViewState = new BookmarksViewState(this);
                return _bookmarksViewState;
            }
        }

        private BottomToolbarView _bottomToolbarView;
        private BottomToolbarView BottomToolbarView
        {
            get
            {
                if (_bottomToolbarView == null) _bottomToolbarView = new BottomToolbarView();
                return _bottomToolbarView;
            }
        }

        private BookmarksWindowContextMenu _contextMenu;
        private BookmarksWindowContextMenu ContextMenu
        {
            get
            {
                if (_contextMenu == null) _contextMenu = new BookmarksWindowContextMenu(this);
                return _contextMenu;
            }
        }

        private bool _initialized;

        [MenuItem("Tools/dFun/[Easy Bookmarks]", false, 0)]
        [MenuItem("Window/[Easy Bookmarks]", false, 0)]
        private static void OpenWindow()
        {
            BookmarksWindow[] windows = (BookmarksWindow[])Resources.FindObjectsOfTypeAll(typeof(BookmarksWindow));
            if (windows.Length == 0)
            {
                BookmarksWindow window = (BookmarksWindow)GetWindow(typeof(BookmarksWindow));
                window.InitializeWindow();
            }
            else
            {
                FocusWindowIfItsOpen(typeof(BookmarksWindow));
            }
        }

        [MenuItem("Tools/dFun/[Force Reopen Easy Bookmarks]", false, 100)]
        private static void ForceReopenWindow()
        {
            BookmarksWindow[] windows = (BookmarksWindow[])Resources.FindObjectsOfTypeAll(typeof(BookmarksWindow));
            for (int i = 0, iSize = windows.Length; i < iSize; i++)
            {
                DestroyImmediate(windows[i]);
            }

            OpenWindow();
        }

        private void InitializeWindow()
        {
            titleContent = new GUIContent(Icons.BookmarksWindowIcon)
            {
                text = "Bookmarks",
                tooltip = "Drag&Drop any assets and objects here"
            };
        }

        private void OnEnable()
        {
            _initialized = false;
        }

        private void OnGUI()
        {
            if (!_initialized)
            {
                _initialized = true;
                InitializeView();
            }

            GUI.FocusControl(null);

            float bottomToolbarHeight = BottomToolbarView.ToolbarHeight;
            Rect bottomToolbarRect = new Rect(
                0, position.height - bottomToolbarHeight, position.width, bottomToolbarHeight
            );
            BottomToolbarView.Draw(bottomToolbarRect);

            BookmarksViewState.NeedRepaint = false;
            {
                Rect bookmarksViewRect = new Rect(0, 0, position.width, position.height - bottomToolbarHeight);
                BookmarksView.Draw(bookmarksViewRect);
            }
            if (BookmarksViewState.NeedRepaint)
            {
                Repaint();
            }
        }

        public void InitializeView()
        {
            BookmarksView.Initialize();
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            ContextMenu.AddItemsToMenu(menu);
        }

        public Rect ToScreenSpaceRect(Rect rect)
        {
            Rect windowPosition = position;
            return new Rect(
                windowPosition.x + rect.x,
                windowPosition.y + rect.y,
                rect.width,
                rect.height
            );
        }

        public static void ForceRepaintAllWindows()
        {
            BookmarksWindow[] windows = (BookmarksWindow[])Resources.FindObjectsOfTypeAll(typeof(BookmarksWindow));
            for (int i = 0, iSize = windows.Length; i < iSize; i++)
            {
                windows[i].Repaint();
            }
        }
        
        public static bool FindInstance(out BookmarksWindow instance)
        {
            BookmarksWindow[] windows = (BookmarksWindow[])Resources.FindObjectsOfTypeAll(typeof(BookmarksWindow));
            if (windows.Length > 0)
            {
                instance = windows[0];
                return true;
            }

            instance = default;
            return false;
        }

        public static void ShowNotificationInAllWindows(string message, double fadeoutWait = 1.5f)
        {
            ShowNotificationInAllWindows(new GUIContent(message), fadeoutWait);
        }

        public static void ShowNotificationInAllWindows(GUIContent content, double fadeoutWait = 1.5f)
        {
            BookmarksWindow[] windows = (BookmarksWindow[])Resources.FindObjectsOfTypeAll(typeof(BookmarksWindow));
            for (int i = 0, iSize = windows.Length; i < iSize; i++)
            {
                windows[i].ShowNotification(content, fadeoutWait);
            }
        }
    }
}