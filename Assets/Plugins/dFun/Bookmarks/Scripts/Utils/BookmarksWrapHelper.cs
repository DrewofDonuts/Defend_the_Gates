using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public static class BookmarksWrapHelper
    {
        public static Bookmarks WrapBookmarks(Bookmark bookmark)
        {
            Bookmarks wrappedBookmarks = CreateBookmarksWithSingleGroup();
            wrappedBookmarks.Groups[0].AddBookmark(bookmark);

            return wrappedBookmarks;
        }

        public static Bookmarks WrapBookmarks(List<Bookmark> bookmarks)
        {
            Bookmarks wrappedBookmarks = CreateBookmarksWithSingleGroup();
            wrappedBookmarks.Groups[0].AddBookmarks(bookmarks);
            return wrappedBookmarks;
        }

        public static Bookmarks WrapBookmarks(BookmarksGroup group)
        {
            Bookmarks wrappedBookmarks = CreateBookmarksWithNoGroups();
            wrappedBookmarks.AddGroup(group, false);
            return wrappedBookmarks;
        }

        private static Bookmarks CreateBookmarksWithSingleGroup()
        {
            Bookmarks wrappedBookmarks = Bookmarks.CreateInstance();

            if (wrappedBookmarks.Groups.Length == 1)
            {
                return wrappedBookmarks;
            }

            if (wrappedBookmarks.Groups.Length == 0)
            {
                wrappedBookmarks.AddNewGroup(false);
                return wrappedBookmarks;
            }

            while (wrappedBookmarks.Groups.Length > 1)
            {
                wrappedBookmarks.RemoveGroupAt(wrappedBookmarks.Groups.Length - 1);
            }

            return wrappedBookmarks;
        }

        private static Bookmarks CreateBookmarksWithNoGroups()
        {
            Bookmarks wrappedBookmarks = Bookmarks.CreateInstance();

            if (wrappedBookmarks.Groups.Length == 0)
            {
                return wrappedBookmarks;
            }

            while (wrappedBookmarks.Groups.Length > 0)
            {
                wrappedBookmarks.RemoveGroupAt(0);
            }

            return wrappedBookmarks;
        }
    }
}