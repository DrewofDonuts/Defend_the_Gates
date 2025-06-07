using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class CustomComparer : IComparer<Bookmark>
    {
        public int Compare(Bookmark x, Bookmark y)
        {
            if (x == null || y == null) return 0;
            return x.CustomSortIndex.CompareTo(y.CustomSortIndex);
        }
    }
}