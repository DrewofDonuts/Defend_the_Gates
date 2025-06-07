using UnityEngine;

namespace DFun.Bookmarks
{
    public interface IBookmarksGroupView
    {
        int BookmarksCount { get; }
        void Draw(Rect groupContentRect);
    }
}