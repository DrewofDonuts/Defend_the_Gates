using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksSettingsMenu
    {
        [MenuItem("Tools/dFun/[Easy Bookmarks Settings]", false, 1)]
        public static void OpenProjectSettings()
        {
            EditorWindow settings = SettingsService.OpenProjectSettings(BookmarksSettingsProvider.Path);
            if (settings == null)
            {
                Debug.LogError($"Could not find {BookmarksSettingsProvider.Path}");
            }
        }
    }
}