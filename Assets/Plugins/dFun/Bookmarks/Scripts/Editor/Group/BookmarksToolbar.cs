using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksToolbar
    {
        private Rect _tabsRect;
        private int _tabsAmount;

        private const float NewTabButtonWidth = 25f;
        private double _lastClickTime;

        public int Draw(int activeTabIndex, BookmarksGroupView[] tabs)
        {
            activeTabIndex = BookmarksToolbarShortcuts.HandleShortcuts(activeTabIndex, tabs);

            _tabsAmount = tabs.Length;
            _tabsRect = EditorGUILayout.BeginHorizontal();
            {
                activeTabIndex = DrawTabs(activeTabIndex, tabs);
            }

            DrawNewTabButton();

            EditorGUILayout.EndHorizontal();
            return activeTabIndex;
        }

        private void DrawNewTabButton()
        {
            if (GUILayout.Button("+", Styles.TabHeaderStyle, GUILayout.Width(NewTabButtonWidth)))
            {
                BookmarksUndo.BeforeBookmarksGroupCreated();
                BookmarksStorage.Get().AddNewGroup();
                BookmarksStorage.Save();
            }
        }

        private int DrawTabs(int activeTabIndex, BookmarksGroupView[] tabs)
        {
            int newActiveTabIndex = activeTabIndex;
            for (int i = 0, iSize = tabs.Length; i < iSize; i++)
            {
                bool isClicked = DrawTab(tabs[i], activeTabIndex == i, i);
                if (isClicked)
                {
                    newActiveTabIndex = i;
                }
            }

            return newActiveTabIndex;
        }

        /// returns is tab clicked
        private bool DrawTab(BookmarksGroupView tab, bool isActive, int tabIndex)
        {
            GUIStyle style = isActive ? Styles.TabHeaderStyleActive : Styles.TabHeaderStyle;
            if (!GUILayout.Button(tab.TabHeader, style, GUILayout.ExpandWidth(true)))
            {
                return false;
            }

            if (Event.current.button == 0) //left click
            {
                bool isDoubleClick = MouseHelper.WasDoubleClicked(ref _lastClickTime);
                if (isDoubleClick)
                {
                    tab.ShowSettingsPopup(
                        CalculatePopupButtonScreenRect(_tabsRect, tabIndex, tab.State)
                    );
                }
                return true;
            }

            if (Event.current.button == 1) //right click
            {
                tab.ShowContextMenu(
                    CalculatePopupButtonScreenRect(_tabsRect, tabIndex, tab.State)
                );
                return false;
            }

            return true;
        }

        private Rect CalculatePopupButtonScreenRect(Rect layoutRect, int tabIndex, BookmarksViewState viewState)
        {
            float oneTabWidth = (layoutRect.width - NewTabButtonWidth) / _tabsAmount;

            Rect tabRect = new Rect(
                layoutRect.xMin + tabIndex * oneTabWidth, layoutRect.yMin,
                oneTabWidth, layoutRect.height
            );

            Rect guiRect = Rect.MinMaxRect(
                Event.current.mousePosition.x, tabRect.min.y,
                Event.current.mousePosition.x + tabRect.height, tabRect.max.y
            );
            return viewState.ParentWindow.ToScreenSpaceRect(guiRect);
        }
    }
}