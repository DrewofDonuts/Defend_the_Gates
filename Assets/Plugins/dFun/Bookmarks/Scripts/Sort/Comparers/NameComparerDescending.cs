using System;
using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class NameComparerDescending : IComparer<Bookmark>
    {
        public int Compare(Bookmark x, Bookmark y)
        {
            if (x == null || y == null) return 0;
            return string.Compare(y.BookmarkName, x.BookmarkName, StringComparison.CurrentCulture);
        }
    }
}