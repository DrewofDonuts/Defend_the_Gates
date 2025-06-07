using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class DateCreatedComparerAscending : IComparer<Bookmark>
    {
        public int Compare(Bookmark x, Bookmark y)
        {
            if (x == null || y == null) return 0;
            return x.CreationTime.CompareTo(y.CreationTime);
        }
    }
}