using System;
using System.Collections.Generic;

namespace DFun.Bookmarks
{
    public static class SortUtils
    {
        public static void Sort(this Bookmarks bookmarks)
        {
            SortByType(bookmarks, bookmarks.SortType);
        }

        public static void SortGroup(this BookmarksGroup group, SortType type)
        {
            SortGroup(group, GetComparer(type));
        }

        public static void SortByType(this Bookmarks bookmarks, SortType type)
        {
            Sort(bookmarks, GetComparer(type));
        }

        private static IComparer<Bookmark> GetComparer(SortType type)
        {
            switch (type)
            {
                case SortType.None:
                    break;

                case SortType.Custom:
                    return new CustomComparer();

                case SortType.NameAscending:
                    return new NameComparerAscending();

                case SortType.NameDescending:
                    return new NameComparerDescending();

                case SortType.DateCreatedAscending:
                    return new DateCreatedComparerAscending();

                case SortType.DateCreatedDescending:
                    return new DateCreatedComparerDescending();

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return new NoneComparer();
        }

        private static void Sort(Bookmarks bookmarks, IComparer<Bookmark> comparer)
        {
            BookmarksGroup[] groups = bookmarks.Groups;
            for (int i = 0, iSize = groups.Length; i < iSize; i++)
            {
                SortGroup(groups[i], comparer);
            }
        }

        private static void SortGroup(BookmarksGroup group, IComparer<Bookmark> comparer)
        {
            List<Bookmark> bookmarksList = group.Bookmarks;
            bookmarksList.Sort(comparer);
        }
    }
}