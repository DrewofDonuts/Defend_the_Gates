using UnityEditor;

namespace DFun.Bookmarks
{
    public static class BookmarksDocsMenu
    {
        [MenuItem("Tools/dFun/[Easy Bookmarks Online Docs]", false, 1000)]
        public static void OpenOnlineDocs()
        {
            DocumentationHelper.OpenInBrowser();
        }

        [MenuItem("Tools/dFun/[Discord Community]", false, 1001)]
        public static void OpenDiscord()
        {
            DiscordHelper.Open();
        }
    }
}