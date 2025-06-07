using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksEditorPrefs
    {
        private static string PrefsKeyPrefix { get; } = $"DFun.Bookmarks.{Application.dataPath.GetHashCode():X}";

        public static string DataStorageKey { get; } = $"{PrefsKeyPrefix}.Data";
        public static string ActiveGroupKey { get; } = $"{PrefsKeyPrefix}.ActiveGroup";
        public static string StorageTypeKey { get; } = $"{PrefsKeyPrefix}.StorageType";
        public static string GridSizeKey { get; } = $"{PrefsKeyPrefix}.GridSize";
    }
}