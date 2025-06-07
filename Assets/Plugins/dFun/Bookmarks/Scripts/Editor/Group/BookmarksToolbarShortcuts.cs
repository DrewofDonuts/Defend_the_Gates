namespace DFun.Bookmarks
{
    public static class BookmarksToolbarShortcuts
    {
        public static int HandleShortcuts(int activeTabIndex, BookmarksGroupView[] tabs)
        {
            return HandleKeyboardShortcuts(activeTabIndex, tabs);
        }

        private static int HandleKeyboardShortcuts(int activeTabIndex, BookmarksGroupView[] tabs)
        {
            if (KeyboardHelper.WasTabButtonPressed())
            {
                return (activeTabIndex + 1) % tabs.Length;
            }

            return activeTabIndex;
        }
    }
}