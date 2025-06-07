using System;
using System.Collections.Generic;
using System.IO;
using DFun.GameObjectResolver;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    public static class AssetBookmarkHelper
    {
        private static readonly List<Action> ScheduledActions = new List<Action>(1);

        static AssetBookmarkHelper()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            InvokeScheduledActions();
        }

        private static void InvokeScheduledActions()
        {
            for (int i = 0, iSize = ScheduledActions.Count; i < iSize; i++)
            {
                ScheduledActions[i]?.Invoke();
            }

            ScheduledActions.Clear();
        }

        public static bool IsAsset(Object obj)
        {
            return obj != null
                   && (AssetDatabase.IsMainAsset(obj) || AssetDatabase.IsSubAsset(obj));
        }

        public static bool IsFolder(Object obj)
        {
            return obj != null
                   && AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(obj));
        }

        public static bool IsAsset(Bookmark bookmark)
        {
            AssetReference assetReference = bookmark.ObjectReference?.AssetReference;
            if (assetReference == null)
            {
                return false;
            }

            Object bookmarkObject = assetReference.AssetObject;
            if (bookmarkObject == null)
            {
                bookmarkObject = FindAsset(assetReference.AssetGuid);
            }

            return IsAsset(bookmarkObject);
        }

        public static bool OpenBookmark(
            Bookmark bookmark, bool select, out Object bookmarkedObject,
            SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            ObjectReference objectReference = bookmark.ObjectReference;
            AssetReference assetReference = objectReference?.AssetReference;
            return OpenBookmark(assetReference, objectReference, select, out bookmarkedObject, selectionFlags);
        }

        public static bool OpenBookmark(
            AssetReference assetReference, ObjectReference objectReference, bool select,
            out Object bookmarkedObject, SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            if (assetReference == null)
            {
                bookmarkedObject = default;
                return false;
            }

            bookmarkedObject = assetReference.AssetObject;
            if (bookmarkedObject == null)
            {
                bookmarkedObject = FindAsset(assetReference.AssetGuid);
            }

            if (bookmarkedObject == null)
            {
                return false;
            }

            if (IsFolder(bookmarkedObject))
            {
                if (select)
                {
                    SelectFolder(bookmarkedObject, selectionFlags);
                }

                return true;
            }

            if (select)
            {
                SelectAsset(bookmarkedObject, selectionFlags);
            }

            bool isComponent = ComponentBookmarkHelper.TryHighlightComponent(
                bookmarkedObject, objectReference, select, out Component resolvedComponent
            );
            if (isComponent)
            {
                bookmarkedObject = resolvedComponent;
            }

            return true;
        }

        private static void SelectFolder(Object folder, SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            bool showFolderContents = !selectionFlags.IsFlagSet(SelectionFlags.Append);
            if (showFolderContents)
            {
                BookmarkSelectionHelper.ResetSelection();
            }
            if (showFolderContents && ProjectBrowserHelper.ShowFolderContents(folder))
            {
                return;
            }

            if (showFolderContents && FindFolderFirstValidFile(folder, out Object firstFolderFile))
            {
                SelectAsset(firstFolderFile, selectionFlags);
            }
            else
            {
                SelectAsset(folder, selectionFlags);
            }
        }

        private static bool FindFolderFirstValidFile(Object folder, out Object firstValidFile)
        {
            string folderPath = AssetDatabase.GetAssetPath(folder);
            string[] subFolders = Directory.GetDirectories(folderPath);
            for (int i = 0, iSize = subFolders.Length; i < iSize; i++)
            {
                firstValidFile = AssetDatabase.LoadAssetAtPath<Object>(subFolders[i]);
                if (firstValidFile != null)
                {
                    return true;
                }
            }

            string[] folderFiles = Directory.GetFiles(folderPath);
            for (int i = 0, iSize = folderFiles.Length; i < iSize; i++)
            {
                firstValidFile = AssetDatabase.LoadAssetAtPath<Object>(folderFiles[i]);
                if (firstValidFile != null)
                {
                    return true;
                }
            }

            firstValidFile = default;
            return false;
        }

        private static void SelectAsset(Object assetAtPath, SelectionFlags selection = SelectionFlags.PingAndActive)
        {
            // ScheduledActions.Add(EditorUtility.FocusProjectWindow);
            BookmarkSelectionHelper.SelectBookmarkedObject(assetAtPath, selection);
        }

        public static Object FindAsset(string assetGuid)
        {
            if (assetGuid == null)
            {
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<Object>(
                AssetDatabase.GUIDToAssetPath(assetGuid)
            );
        }

        public static string TryGetAssetGuid(Object obj)
        {
            return TryGetAssetGuid(
                AssetDatabase.GetAssetPath(obj)
            );
        }

        public static string TryGetAssetGuid(string assetPath)
        {
            if (assetPath == null)
            {
                return null;
            }

            return AssetDatabase.AssetPathToGUID(assetPath);
        }

        public static void SelectAssetObject(
            Object assetObject, SelectionFlags selection = SelectionFlags.PingAndActive)
        {
            if (assetObject == null)
            {
                return;
            }

            if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(assetObject)))
            {
                SelectFolder(assetObject, selection);
            }
            else
            {
                SelectAsset(assetObject, selection);
            }
        }

        public static bool TryToGetParentScene(Object o, out Object sceneAsset)
        {
            if (o is Component component)
            {
                return TryToGetParentScene(component.gameObject, out sceneAsset);
            }

            if (o is GameObject gameObject)
            {
                return TryToGetParentScene(gameObject, out sceneAsset);
            }

            sceneAsset = default;
            return false;
        }

        public static bool TryToGetParentScene(GameObject o, out Object sceneAsset)
        {
            sceneAsset = AssetDatabase.LoadAssetAtPath<Object>(o.scene.path);
            return sceneAsset != null;
        }

        public static bool TryToSelectContextPrefab(
            Bookmark bookmark, SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            ObjectReference objectReference = bookmark.ObjectReference;
            AssetReference contextPrefabReference = objectReference?.ContextAssetReference;
            return OpenBookmark(contextPrefabReference, objectReference, true, out Object _, selectionFlags);
        }

        public static void OpenAsset(Object asset)
        {
            AssetDatabase.OpenAsset(asset);
        }
    }
}