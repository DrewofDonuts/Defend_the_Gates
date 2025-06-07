using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class DateCreatedComparerDescending : IComparer<Bookmark>
    {
        public int Compare(Bookmark x, Bookmark y)
        {
            if (x == null || y == null) return 0;
            return y.CreationTime.CompareTo(x.CreationTime);
        }
    }
}