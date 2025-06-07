using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksWrapper : ScriptableObject
    {
        [SerializeField] private Bookmarks bookmarks;

        public Bookmarks Bookmarks
        {
            get
            {
                if (bookmarks == null)
                {
                    bookmarks = Bookmarks.CreateInstance();
                }

                return bookmarks;
            }
        }
    }
}