using DFun.GameObjectResolver;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class GlobalObjectIdBookmarkHelper
    {
        public const int NullIdentifier = 0;
        public const int ImportedAssetIdentifier = 1;
        public const int SceneObjectIdentifier = 2;
        public const int SourceAssetIdentifier = 3;

        public static readonly GlobalObjectId NoneGlobalObjectId = new GlobalObjectId();

        public static GlobalObjectId GetObjectId(Object o)
        {
            return GlobalObjectIdCached.GetObjectId(o);
        }

        public static bool IsNone(string idString)
        {
            if (TryParseGlobalObjectId(idString, out GlobalObjectId id))
            {
                return IsNone(id);
            }

            return true;
        }

        public static bool IsNone(this GlobalObjectId id)
        {
            return id.Equals(NoneGlobalObjectId);
        }

        private static Object GetObject(GlobalObjectId objectId)
        {
            return GlobalObjectIdCached.GetObject(objectId);
        }

        public static bool OpenBookmark(
            Bookmark bookmark, bool select, out Object bookmarkedObject,
            SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            if (ResolveBookmarkedObject(bookmark, out bookmarkedObject))
            {
                if (select)
                {
                    SelectBookmarkedObject(bookmark, bookmarkedObject, selectionFlags);
                }

                return true;
            }

            return false;
        }

        private static void SelectBookmarkedObject(Bookmark bookmark, Object bookmarkedObject,
            SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            if (!TryParseGlobalObjectId(bookmark.ObjectReference.GlobalObjectId, out GlobalObjectId globalObjectId))
            {
                return;
            }

            switch (globalObjectId.identifierType)
            {
                case ImportedAssetIdentifier:
                case SourceAssetIdentifier:
                    AssetBookmarkHelper.SelectAssetObject(bookmarkedObject, selectionFlags);
                    break;

                case SceneObjectIdentifier:
                    SceneBookmarkHelper.SelectObject(bookmarkedObject, selectionFlags);
                    break;
            }
        }

        public static bool ResolveBookmarkedObject(Bookmark bookmark, out Object bookmarkedObject)
        {
            ObjectReference objectReference = bookmark.ObjectReference;
            if (objectReference == null)
            {
                bookmarkedObject = default;
                return false;
            }

            return ResolveBookmarkedObject(objectReference.GlobalObjectId, out bookmarkedObject);
        }

        public static bool ResolveBookmarkedObject(string globalObjectIdString, out Object bookmarkedObject)
        {
            if (!TryParseGlobalObjectId(globalObjectIdString, out GlobalObjectId globalObjectId))
            {
                bookmarkedObject = default;
                return false;
            }

            return ResolveBookmarkedObject(globalObjectId, out bookmarkedObject);
        }

        public static bool TryParseGlobalObjectId(string globalObjectIdString, out GlobalObjectId globalObjectId)
        {
            if (string.IsNullOrEmpty(globalObjectIdString))
            {
                globalObjectId = NoneGlobalObjectId;
                return false;
            }

            return GlobalObjectId.TryParse(globalObjectIdString, out globalObjectId);
        }

        public static bool ResolveBookmarkedObject(GlobalObjectId globalObjectId, out Object bookmarkedObject)
        {
            if (globalObjectId.IsNone())
            {
                bookmarkedObject = default;
                return false;
            }

            bookmarkedObject = GetObject(globalObjectId);
            return bookmarkedObject != null;
        }
    }
}