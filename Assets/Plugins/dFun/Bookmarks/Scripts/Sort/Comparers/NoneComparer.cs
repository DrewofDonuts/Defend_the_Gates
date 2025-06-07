using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public class NoneComparer : IComparer<Bookmark>
    {
        public int Compare(Bookmark x, Bookmark y)
        {
            return 0;
        }
    }
}