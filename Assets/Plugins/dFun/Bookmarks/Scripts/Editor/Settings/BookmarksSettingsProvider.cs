using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DFun.Bookmarks
{
    class BookmarksSettingsProvider : SettingsProvider
    {
        public const string Label = "Easy Bookmarks";
        public const string Path = "Project/" + Label;

        internal class Content
        {
            public static readonly GUIContent StorageSectionHeader = new GUIContent("Storage");
            public static readonly GUIContent StorageType = new GUIContent("Storage Type");

            public static readonly GUIContent ViewSectionHeader = new GUIContent("View");
            public static readonly GUIContent ShowSingleGroup = new GUIContent(
                "Show Single Group?", "Show single existing group tab when toggle is enabled and hide when disabled"
            );

            public static readonly GUIContent AdvancedHeader = new GUIContent("Advanced");

            public static readonly GUIContent DynamicObjectResolverLabel = new GUIContent("Dynamic Object Resolver");

            public static readonly GUIContent DynamicObjectResolverNotAvailableHeader = new GUIContent(
                "Dynamic Object Resolver: No candidates found"
            );

            public static readonly GUIContent DynamicObjectResolverErrorHeader = new GUIContent(
                "Dynamic Object Resolver: Unknown"
            );

            public static readonly GUIContent AutoSave = new GUIContent(
                "Auto Save",
                "Auto Save asset on bookmarks modifications? Instead asset marked as dirty and will be saved during project saving"
            );

            public static GUIStyle StorageHeaderStyle => EditorStyles.boldLabel;
            public static GUIStyle DynamicObjectResolverHeaderStyle => EditorStyles.boldLabel;
        }

        private readonly BookmarksSettingsView _view;

        private BookmarksSettingsProvider() : base(Path, SettingsScope.Project)
        {
            label = Label;
            keywords = GetSearchKeywordsFromGUIContentProperties<Content>();
            _view = new BookmarksSettingsView();
        }

        [SettingsProvider]
        public static SettingsProvider CreateBookmarksSettingsProvider()
        {
            return new BookmarksSettingsProvider();
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the MyCustom element in the Settings window.
        }

        public override void OnGUI(string searchContext)
        {
            _view.Draw();
        }
    }
}