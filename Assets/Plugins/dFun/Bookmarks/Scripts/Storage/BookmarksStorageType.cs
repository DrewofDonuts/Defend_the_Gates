using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksStorageType
    {
        public enum StorageType
        {
            EditorPrefs = 10,
            Asset = 20
        }

        private static string StorageTypePrefsKey => BookmarksEditorPrefs.StorageTypeKey;
        private const StorageType DefaultStorageType = StorageType.EditorPrefs;

        private static bool _initialized;
        private static StorageType _currentStorageType;

        public static StorageType CurrentStorageType
        {
            get
            {
                TryToInit();
                return _currentStorageType;
            }
            set
            {
                TryToInit();
                if (_currentStorageType != value)
                {
                    _currentStorageType = value;
                    SaveToPrefs();
                }
            }
        }

        private static void TryToInit()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            _currentStorageType = HasInPrefs()
                ? GetFromPrefs()
                : InitStorageType();
        }

        private static StorageType InitStorageType()
        {
            if (DefaultStorageType == StorageType.EditorPrefs && AssetBookmarksStorage.HasAnyData())
            {
                return StorageType.Asset;
            }

            if (DefaultStorageType == StorageType.Asset && EditorPrefsBookmarksStorage.HasAnyData())
            {
                return StorageType.EditorPrefs;
            }

            return DefaultStorageType;
        }

        private static bool HasInPrefs()
        {
            return EditorPrefs.HasKey(StorageTypePrefsKey);
        }

        private static StorageType GetFromPrefs()
        {
            return (StorageType) EditorPrefs.GetInt(
                StorageTypePrefsKey, (int) DefaultStorageType
            );
        }

        private static void SaveToPrefs()
        {
            EditorPrefs.SetInt(StorageTypePrefsKey, (int) _currentStorageType);
        }

        // [MenuItem("Tools/dFun/Remove EditorPrefs StorageType")]
        private static void Reset()
        {
            EditorPrefs.DeleteKey(StorageTypePrefsKey);
            Debug.Log("BookmarksStorageType.Reset: DONE");
        }
    }
}