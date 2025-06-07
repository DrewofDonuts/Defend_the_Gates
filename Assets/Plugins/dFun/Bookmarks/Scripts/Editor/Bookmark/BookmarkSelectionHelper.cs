using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    public static class BookmarkSelectionHelper
    {
        public static void HandleBookmarkClick(
            BaseBookmarkView[] bookmarkViews, int bookmarkIndex, BookmarksViewState state)
        {
            bool wasBookmarkSelected = state.HandleBookmarkClick(bookmarkIndex);
            state.NeedRepaint = true;
            if (wasBookmarkSelected)
            {
                OpenBookmarks(bookmarkViews, state);
            }
        }

        public static void OpenBookmarks<T>(T[] bookmarkViews, BookmarksViewState state) where T : BaseBookmarkView
        {
            if (bookmarkViews.Length == 0) return;
            if (!state.HasAnySelectedBookmark()) return;

            List<int> selectedBookmarksIndices = state.SelectedBookmarksIndices;
            if (selectedBookmarksIndices.Count == 1)
            {
                TryToOpenBookmark(state.GetFirstSelectedBookmark(), SelectionFlags.PingAndActive, bookmarkViews);
                return;
            }

            //open multiple bookmarks: set active only last selected bookmark, and append rest of the bookmarks
            ResetSelection();
            for (int i = 0, iSize = selectedBookmarksIndices.Count - 1; i < iSize; i++)
            {
                TryToOpenBookmark(selectedBookmarksIndices[i], SelectionFlags.Append, bookmarkViews);
            }

            int lastSelectedBookmark = state.GetLastSelectedBookmark();
            TryToOpenBookmark(lastSelectedBookmark, SelectionFlags.PingAndActiveAndAppend, bookmarkViews);
        }

        private static bool TryToOpenBookmark<T>(int bookmarkIndex, SelectionFlags selectionFlags, T[] bookmarkViews)
            where T : BaseBookmarkView
        {
            if (IsValidBookmarkIndex(bookmarkIndex, bookmarkViews))
            {
                bookmarkViews[bookmarkIndex].BookmarkData.Open(selectionFlags);
                return true;
            }
            return false;
        }

        public static bool IsValidBookmarkIndex<T>(int bookmarkIndex, T[] bookmarkViews) where T : BaseBookmarkView
        {
            return bookmarkIndex >= 0 && bookmarkIndex <= bookmarkViews.Length - 1;
        }

        public static void OpenSingleBookmark(Bookmark bookmarkData)
        {
            bookmarkData.Open();
        }

        public static void SelectBookmarkedObject(
            Object bookmarkedObject, SelectionFlags selection = SelectionFlags.PingAndActive)
        {
            if (selection.IsFlagSet(SelectionFlags.Ping))
            {
                EditorGUIUtility.PingObject(bookmarkedObject);
            }

            if (!TryToAppendSelection(bookmarkedObject, selection))
            {
                if (selection.IsFlagSet(SelectionFlags.Active))
                {
                    Selection.activeObject = bookmarkedObject;
                }
            }
        }

        /// <returns>Appended</returns>
        public static bool TryToAppendSelection(Object bookmarkedObject, SelectionFlags selection)
        {
            if (selection.IsFlagSet(SelectionFlags.Append))
            {
                Object[] selectedObjects = Selection.objects;
                Object[] newSelectedObjects = new Object[selectedObjects.Length + 1];
                Array.Copy(selectedObjects, newSelectedObjects, selectedObjects.Length);
                newSelectedObjects[newSelectedObjects.Length - 1] = bookmarkedObject;

                Selection.objects = newSelectedObjects;
                return true;
            }
            return false;
        }

        public static void SetSelected(Object selectedObject)
        {
            Selection.objects = new[] { selectedObject };
        }

        public static void ResetSelection()
        {
            Selection.objects = Array.Empty<Object>();
        }

        public static void HandleDoubleClick(Object obj)
        {
            if (AssetBookmarkHelper.IsAsset(obj))
            {
                AssetBookmarkHelper.OpenAsset(obj);
                return;
            }

            if (obj is Component)
            {
                return;
            }

            //scene object
            SceneBookmarkHelper.FocusOnLastSelectedObject();
        }
    }
}