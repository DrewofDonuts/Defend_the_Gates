using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    [Serializable]
    public class BookmarksGroup
    {
        [SerializeField] private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        [SerializeField] private string description;
        public string Description
        {
            get => description;
            set => description = value;
        }

        [SerializeField] private BookmarksGroupGridSize groupGridSize = new BookmarksGroupGridSize();
        public BookmarksGroupGridSize GroupGridSize
        {
            get => groupGridSize;
            set => groupGridSize = value;
        }

        [SerializeField] private List<Bookmark> bookmarks;
        public List<Bookmark> Bookmarks
        {
            get
            {
                if (bookmarks == null)
                {
                    bookmarks = new List<Bookmark>(0);
                }

                return bookmarks;
            }
        }

        public bool HasAnyBookmark => Bookmarks.Count > 0;

        public BookmarksGroup(string groupName)
        {
            Name = groupName;
            bookmarks = new List<Bookmark>(0);
        }

        public BookmarksGroup(BookmarksGroup copyFrom)
        {
            name = copyFrom.Name;
            description = copyFrom.Description;
            groupGridSize = new BookmarksGroupGridSize(copyFrom.GroupGridSize);
            bookmarks = new List<Bookmark>(copyFrom.Bookmarks.Count);
            for (int i = 0, iSize = copyFrom.Bookmarks.Count; i < iSize; i++)
            {
                bookmarks.Add(new Bookmark(copyFrom.Bookmarks[i]));
            }
        }

        public void AddItems(Object[] objectReferences)
        {
            int maxCustomSortIndex = DefineMaxCustomSortIndex();
            for (int i = 0, iSize = objectReferences.Length; i < iSize; i++)
            {
                Bookmark newBookmark = new Bookmark(objectReferences[i]);
                if (newBookmark.ContainsData)
                {
                    newBookmark.CustomSortIndex = ++maxCustomSortIndex;
                    Bookmarks.Add(newBookmark);
                }
                else
                {
                    Debug.Log("Can't create bookmark from this object", objectReferences[i]);
                }
            }
        }

        public void AddBookmark(Bookmark newBookmark)
        {
            int maxCustomSortIndex = DefineMaxCustomSortIndex();
            if (newBookmark.ContainsData)
            {
                newBookmark.CustomSortIndex = ++maxCustomSortIndex;
                Bookmarks.Add(newBookmark);
            }
            else
            {
                Debug.Log($"Can't add  bookmark {newBookmark.BookmarkName}");
            }
        }

        public void AddBookmarks(List<Bookmark> newBookmarks)
        {
            int maxCustomSortIndex = DefineMaxCustomSortIndex();
            for (int i = 0, iSize = newBookmarks.Count; i < iSize; i++)
            {
                Bookmark newBookmark = newBookmarks[i];
                if (newBookmark != null && newBookmark.ContainsData)
                {
                    newBookmark.CustomSortIndex = ++maxCustomSortIndex;
                    Bookmarks.Add(newBookmark);
                }
                else
                {
                    Debug.Log($"Can't add bookmark {newBookmark.BookmarkName}");
                }
            }
        }

        private int DefineMaxCustomSortIndex()
        {
            int maxIndex = 0;
            List<Bookmark> bookmarksList = Bookmarks;
            for (int i = 0, iSize = bookmarksList.Count; i < iSize; i++)
            {
                maxIndex = Math.Max(maxIndex, bookmarksList[i].CustomSortIndex);
            }
            return maxIndex;
        }

        public void Cleanup()
        {
            Bookmarks.Clear();
        }

        public void NormalizeName(string defaultName)
        {
            if (string.IsNullOrEmpty(Name))
            {
                Name = defaultName;
            }
        }

        public void UpdateCustomSortIndices()
        {
            List<Bookmark> bookmarksList = Bookmarks;
            for (int i = 0, iSize = bookmarksList.Count; i < iSize; i++)
            {
                bookmarksList[i].CustomSortIndex = i;
            }
        }

        public void RemoveByIndices(List<int> bookmarksIndicesToRemove)
        {
            bookmarksIndicesToRemove.Sort();
            List<Bookmark> bookmarksToRemove = new List<Bookmark>(bookmarksIndicesToRemove.Count);
            for (int i = 0, iSize = bookmarksIndicesToRemove.Count; i < iSize; i++)
            {
                int bookmarkIndex = bookmarksIndicesToRemove[i];
                if (IsValidBookmarkIndex(bookmarkIndex))
                {
                    bookmarksToRemove.Add(Bookmarks[bookmarkIndex]);
                }
            }

            for (int i = 0, iSize = bookmarksToRemove.Count; i < iSize; i++)
            {
                Bookmarks.Remove(bookmarksToRemove[i]);
            }
        }

        public bool IsValidBookmarkIndex(int bookmarkIndex)
        {
            return bookmarkIndex >= 0 && bookmarkIndex <= Bookmarks.Count - 1;
        }
    }
}