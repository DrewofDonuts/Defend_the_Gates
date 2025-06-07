using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class DragAndDropHelper
    {
        private static readonly List<Object> DraggingBookmarkObjects = new List<Object>();

        public static void SetDraggingObjects(IEnumerable<Object> objects)
        {
            DraggingBookmarkObjects.Clear();
            DraggingBookmarkObjects.AddRange(objects);
        }

        public static bool IsDraggingAnyBookmarkObject()
        {
            return IsDraggingAnyBookmarkObject(DragAndDrop.objectReferences);
        }

        public static bool IsDraggingAnyBookmarkObject(Object[] objectsToCheck)
        {
            if (objectsToCheck == null || objectsToCheck.Length == 0)
            {
                return false;
            }

            for (int i = 0, iSize = objectsToCheck.Length; i < iSize; i++)
            {
                if (DraggingBookmarkObjects.Contains(objectsToCheck[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static void Clear()
        {
            DraggingBookmarkObjects.Clear();
        }

        public static void StartDrag<T>(int draggedBookmarkIndex, T[] views, BookmarksViewState state)
            where T : BaseBookmarkView
        {
            CleanupBookmarksStateForDrag(draggedBookmarkIndex, state);
            List<Object> objectsToDrag = DefineObjectsForDrag(draggedBookmarkIndex, views, state);

            if (objectsToDrag.Count > 0)
            {
                GUIUtility.hotControl = 0;
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = objectsToDrag.ToArray();
                SetDraggingObjects(objectsToDrag);
                DragAndDrop.StartDrag($"Drag {objectsToDrag.Count} bookmarks");
            }
        }

        private static void CleanupBookmarksStateForDrag(int draggedBookmarkIndex, BookmarksViewState state)
        {
            if (KeyboardHelper.IsControlModifierPressed() || KeyboardHelper.IsShiftModifierPressed())
            {
                return;
            }

            if (!state.IsBookmarkSelected(draggedBookmarkIndex))
            {
                //select this bookmark only
                state.SetSelectedBookmark(draggedBookmarkIndex);
            }
        }

        private static List<Object> DefineObjectsForDrag<T>(
            int draggedBookmarkIndex, T[] views, BookmarksViewState state) where T : BaseBookmarkView
        {
            List<int> selectedBookmarksIndices = state.SelectedBookmarksIndices;
            List<Object> objectsToDrag = new List<Object>(selectedBookmarksIndices.Count + 1);

            if (selectedBookmarksIndices.Contains(draggedBookmarkIndex))
            {
                //drag all selected bookmarks
                for (int i = 0, iSize = selectedBookmarksIndices.Count; i < iSize; i++)
                {
                    int selectedBookmarkIndex = selectedBookmarksIndices[i];
                    if (CanDragAndDrop(selectedBookmarkIndex, views, out Object bookmarkedObject))
                    {
                        objectsToDrag.Add(bookmarkedObject);
                    }
                }
            }
            else
            {
                //drag only bookmark above a mouse pointer
                if (CanDragAndDrop(draggedBookmarkIndex, views, out Object bookmarkedObject))
                {
                    objectsToDrag.Add(bookmarkedObject);
                }
            }

            return objectsToDrag;
        }

        private static bool CanDragAndDrop<T>(int bookmarkIndex, T[] views, out Object bookmarkedObject)
            where T : BaseBookmarkView
        {
            if (!BookmarkSelectionHelper.IsValidBookmarkIndex(bookmarkIndex, views))
            {
                bookmarkedObject = default;
                return false;
            }

            if (CanDragAndDrop(views[bookmarkIndex].BookmarkData, out bookmarkedObject))
            {
                return true;
            }

            return false;
        }

        public static bool CanDragAndDrop(Bookmark data, out Object bookmarkedObject)
        {
            if (!data.Resolve(out bookmarkedObject))
            {
                return false;
            }

            return (AssetBookmarkHelper.IsAsset(bookmarkedObject) && !AssetBookmarkHelper.IsFolder(bookmarkedObject))
                   || bookmarkedObject is Component;
        }
    }
}