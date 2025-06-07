using System;
using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class NameComparerAscending : IComparer<Bookmark>
    {
        public int Compare(Bookmark x, Bookmark y)
        {
            if (x == null || y == null) return 0;
            return string.Compare(x.BookmarkName, y.BookmarkName, StringComparison.CurrentCulture);
        }
    }
}