using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class DocumentationHelper
    {
        private const string OnlineDocsAssetPath = "Assets/Plugins/dFun/Bookmarks/Documentation/OnlineDocs.txt";

        private const string DynamicObjectResolverHeader = "/edit#heading=h.rj2kkd2hyjh";

        public enum DocsSection
        {
            None,
            DynamicObjectResolver
        }

        public static void OpenInBrowser(DocsSection section = DocsSection.None)
        {
            if (!FindDocsLinkBase(out string linkBase))
            {
                Debug.Log($"Can't find link to online docs: {OnlineDocsAssetPath}");
                return;
            }

            string fullLink = linkBase;
            switch (section)
            {
                case DocsSection.DynamicObjectResolver:
                    fullLink += DynamicObjectResolverHeader;
                    break;
            }

            OpenLink(fullLink);
        }

        private static bool FindDocsLinkBase(out string linkBase)
        {
            TextAsset onlineDocsAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(OnlineDocsAssetPath);
            if (onlineDocsAsset == null)
            {
                linkBase = default;
                return false;
            }

            linkBase = onlineDocsAsset.text;
            return true;
        }

        private static void OpenLink(string link)
        {
            Application.OpenURL(link);
        }
    }
}