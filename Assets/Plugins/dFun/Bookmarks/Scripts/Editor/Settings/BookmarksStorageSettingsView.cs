using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksStorageSettingsView
    {
        private static Bookmarks Bookmarks => BookmarksStorage.Get();

        private static GUIContent[] _storageTypeDisplayedOptions;
        private static GUIContent[] StorageTypeDisplayedOptions
        {
            get
            {
                if (_storageTypeDisplayedOptions == null)
                {
                    _storageTypeDisplayedOptions = new[]
                    {
                        new GUIContent("Personal", "Data is stored in EditorPrefs"),
                        new GUIContent("Project",
                            "Data is stored as an Asset (by default in 'Assets/Editor Default Resources' folder)"),
                    };
                }

                return _storageTypeDisplayedOptions;
            }
        }

        private static int[] _storageTypeOptions;
        private static int[] StorageTypeOptions
        {
            get
            {
                if (_storageTypeOptions == null)
                {
                    _storageTypeOptions = new[]
                    {
                        (int) BookmarksStorageType.StorageType.EditorPrefs,
                        (int) BookmarksStorageType.StorageType.Asset
                    };
                }

                return _storageTypeOptions;
            }
        }

        public static void Draw()
        {
            DrawStorageSection();
            EditorGUILayout.Space();
            DrawViewSection();
        }

        private static void DrawStorageSection()
        {
            EditorGUILayout.LabelField(
                BookmarksSettingsProvider.Content.StorageSectionHeader,
                BookmarksSettingsProvider.Content.StorageHeaderStyle
            );

            BookmarksStorage.CurrentStorageType = (BookmarksStorageType.StorageType) EditorGUILayout.IntPopup(
                BookmarksSettingsProvider.Content.StorageType,
                (int) BookmarksStorage.CurrentStorageType,
                StorageTypeDisplayedOptions,
                StorageTypeOptions
            );

            DrawAutoSave();
        }

        private static void DrawViewSection()
        {
            EditorGUILayout.LabelField(
                BookmarksSettingsProvider.Content.ViewSectionHeader,
                BookmarksSettingsProvider.Content.StorageHeaderStyle
            );

            DrawShowSingleGroup();
        }

        private static void DrawShowSingleGroup()
        {
            EditorGUI.BeginChangeCheck();
            {
                Bookmarks bookmarks = Bookmarks;
                bool toggleValue = EditorGUILayout.Toggle(
                    BookmarksSettingsProvider.Content.ShowSingleGroup,
                    bookmarks.Settings.ViewSettings.ShowSingleBookmarksTab
                );
                if (EditorGUI.EndChangeCheck())
                {
                    bookmarks.Settings.ViewSettings.ShowSingleBookmarksTab = toggleValue;
                    bookmarks.Dirty = true;
                    Save();
                    BookmarksWindow.ForceRepaintAllWindows();
                }
            }
        }

        private static void DrawAutoSave()
        {
            if (BookmarksStorage.CurrentStorageType == BookmarksStorageType.StorageType.Asset)
            {
                EditorGUI.BeginChangeCheck();
                {
                    Bookmarks bookmarks = Bookmarks;
                    bool toggleValue = EditorGUILayout.Toggle(
                        BookmarksSettingsProvider.Content.AutoSave, bookmarks.AutoSave
                    );

                    if (EditorGUI.EndChangeCheck())
                    {
                        bookmarks.AutoSave = toggleValue;
                        Save();
                    }
                }
            }
        }

        private static void Save()
        {
            BookmarksStorage.Save();
        }
    }
}