using System.IO;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class AssetBookmarksStorage : IBookmarksStorage
    {
        private const string DataFolder = "Editor Default Resources/";
        private const string DataAssetName = "EaseBookmarks";

        private BookmarksWrapper _bookmarksWrapper;

        public Bookmarks Get()
        {
            return GetWrapper().Bookmarks;
        }

        public Object GetUndoObject()
        {
            return GetWrapper();
        }

        public void Save()
        {
            BookmarksWrapper bookmarksWrapper = GetWrapper();
            EditorUtility.SetDirty(bookmarksWrapper);
            if (bookmarksWrapper.Bookmarks.AutoSave)
            {
#if UNITY_2021_2_OR_NEWER
                AssetDatabase.SaveAssetIfDirty(bookmarksWrapper);
#else
                AssetDatabase.SaveAssets();
#endif
            }
        }

        public void Cleanup()
        {
            if (_bookmarksWrapper != null)
            {
                AssetDatabase.DeleteAsset(
                    AssetDatabase.GetAssetPath(_bookmarksWrapper)
                );
                _bookmarksWrapper = null;
            }
        }

        private BookmarksWrapper GetWrapper()
        {
            if (_bookmarksWrapper == null)
            {
                _bookmarksWrapper = FindOrCreateBookmarksInstance();
            }

            return _bookmarksWrapper;
        }

        private static BookmarksWrapper FindOrCreateBookmarksInstance()
        {
            string[] candidates = FindBookmarksInstanceCandidates();
            if (candidates.Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath<BookmarksWrapper>(
                    AssetDatabase.GUIDToAssetPath(candidates[0])
                );
            }

            return CreateBookmarksAsset();
        }

        private static string[] FindBookmarksInstanceCandidates()
        {
            return AssetDatabase.FindAssets("t:" + nameof(BookmarksWrapper));
        }

        private static BookmarksWrapper CreateBookmarksAsset()
        {
            string dirPath = Path.GetDirectoryName("Assets/" + DataFolder);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                AssetDatabase.Refresh();
            }

            BookmarksWrapper asset = ScriptableObject.CreateInstance<BookmarksWrapper>();
            string assetPath = dirPath + "/" + DataAssetName + ".asset";

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            return AssetDatabase.LoadAssetAtPath<BookmarksWrapper>(assetPath);
        }

        public static bool HasAnyData()
        {
            return FindBookmarksInstanceCandidates().Length > 0;
        }
    }
}