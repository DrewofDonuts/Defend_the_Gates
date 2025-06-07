using UnityEngine;

namespace DFun.Bookmarks
{
    public interface IBookmarksStorage
    {
        Bookmarks Get();
        Object GetUndoObject();
        void Save();
        void Cleanup();
    }
}