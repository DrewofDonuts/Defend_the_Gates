using System;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    public static class BookmarksStorage
    {
        public static BookmarksStorageType.StorageType CurrentStorageType
        {
            get => BookmarksStorageType.CurrentStorageType;
            set
            {
                if (CurrentStorageType == value)
                {
                    return;
                }

                IBookmarksStorage oldStorage = Storage;
                {
                    _storage = null;
                    BookmarksStorageType.CurrentStorageType = value;
                }
                IBookmarksStorage newStorage = Storage;

                Bookmarks bookmarks = newStorage.Get();
                bookmarks.DeepCopy(oldStorage.Get());
                bookmarks.Dirty = true;
                newStorage.Save();

                oldStorage.Cleanup();
            }
        }

        private static IBookmarksStorage _storage;
        private static IBookmarksStorage Storage
        {
            get
            {
                if (_storage == null)
                {
                    _storage = InitStorage(CurrentStorageType);
                }

                return _storage;
            }
        }

        public static Bookmarks Get()
        {
            return Storage.Get();
        }

        public static Object GetUndoObject()
        {
            return Storage.GetUndoObject();
        }
        
        public static void Save()
        {
            Storage.Save();
        }

        private static IBookmarksStorage InitStorage(BookmarksStorageType.StorageType storageType)
        {
            switch (storageType)
            {
                case BookmarksStorageType.StorageType.Asset:
                    return new AssetBookmarksStorage();

                case BookmarksStorageType.StorageType.EditorPrefs:
                    return new EditorPrefsBookmarksStorage();

                default:
                    throw new ArgumentOutOfRangeException(nameof(storageType), storageType, null);
            }
        }
    }
}