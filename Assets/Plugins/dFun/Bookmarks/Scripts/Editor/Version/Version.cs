using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class Version
    {
        public static string Value => GetVersion();

        private const string VersionAssetPath = "Assets/Plugins/dFun/Bookmarks/version.txt";
        private const string DefaultVersion = "0.0.0";

        private static string GetVersion()
        {
            if (ReadVersionFromFile(out string versionFromFile) && !string.IsNullOrEmpty(versionFromFile))
            {
                return versionFromFile;
            }

            return DefaultVersion;
        }

        private static bool ReadVersionFromFile(out string fileContent)
        {
            TextAsset onlineDocsAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(VersionAssetPath);
            if (onlineDocsAsset == null)
            {
                fileContent = default;
                return false;
            }

            fileContent = onlineDocsAsset.text;
            return true;
        }
    }
}