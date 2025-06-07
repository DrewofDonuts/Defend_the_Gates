using System;
using System.Collections.Generic;
using DFun.GameObjectResolver;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    public static class SceneBookmarkHelper
    {
        private static readonly List<GameObject> TemporaryObjectCandidates = new List<GameObject>();

        private static SceneObjectResolver _dynamicObjectResolver;

        private static SceneObjectResolver DynamicObjectResolver
        {
            get
            {
                if (_dynamicObjectResolver == null)
                {
                    _dynamicObjectResolver = CreateNewDynamicObjectResolver();
                }

                return _dynamicObjectResolver;
            }
        }

        private static SceneObjectResolver CreateNewDynamicObjectResolver()
        {
            string targetClass = BookmarksStorage.Get().Settings.DynamicObjectResolverClass;
            if (TryCreateDynamicObjectResolverFromClass(targetClass, out SceneObjectResolver dynamicObjectResolver))
            {
                return dynamicObjectResolver;
            }

            return DefaultSceneObjectResolver.Instance;
        }

        private static bool TryCreateDynamicObjectResolverFromClass(
            string targetClass, out SceneObjectResolver dynamicObjectResolver)
        {
            if (string.IsNullOrEmpty(targetClass))
            {
                dynamicObjectResolver = default;
                return false;
            }

            if (!TypeUtils.TryParseClassType(targetClass, out Type dynamicObjectResolverType))
            {
                dynamicObjectResolver = default;
                return false;
            }

            try
            {
                dynamicObjectResolver = (SceneObjectResolver)Activator.CreateInstance(dynamicObjectResolverType);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Can't create object from class '{targetClass}'. Error: {e.Message}");
            }

            dynamicObjectResolver = default;
            return false;
        }

        public static bool OpenBookmark(Bookmark bookmark, bool select, out Object bookmarkObject)
        {
            SceneObjectReference sceneObjectReference = bookmark.ObjectReference?.SceneObjectReference;
            if (sceneObjectReference == null)
            {
                bookmarkObject = default;
                return false;
            }

            TemporaryObjectCandidates.Clear();
            if (DynamicObjectResolver.TryResolveNonAlloc(sceneObjectReference, false, TemporaryObjectCandidates))
            {
                bookmarkObject = TemporaryObjectCandidates.Count > 0 ? TemporaryObjectCandidates[0] : default;
                if (select)
                {
                    SelectObjects(TemporaryObjectCandidates);
                }

                return true;
            }

            bookmarkObject = default;
            return false;
        }

        public static bool TrySelectScene(Bookmark bookmark)
        {
            SceneObjectReference sceneObjectReference = bookmark.ObjectReference?.SceneObjectReference;
            if (sceneObjectReference == null)
            {
                return false;
            }

            return TrySelectScene(sceneObjectReference.ParentScenePath);
        }

        public static bool TrySelectScene(string scenePath)
        {
            if (scenePath == null)
            {
                return false;
            }

            if (IsSceneActive(scenePath))
            {
                return false;
            }

            Object sceneAsset = AssetDatabase.LoadAssetAtPath<Object>(scenePath);
            if (sceneAsset != null)
            {
                SelectObject(sceneAsset);
                return true;
            }

            return false;
        }

        private static bool IsSceneActive(string scenePath)
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.path == scenePath)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool SelectObjects(
            List<GameObject> objectsToSelect, SelectionFlags selection = SelectionFlags.PingAndActive)
        {
            if (objectsToSelect.Count > 0)
            {
                SelectObject(objectsToSelect[0], selection);
                return true;
            }

            return false;
        }

        public static void SelectObject(Object objectToSelect, SelectionFlags selection = SelectionFlags.PingAndActive)
        {
            BookmarkSelectionHelper.SelectBookmarkedObject(objectToSelect, selection);
        }

        public static void ResetDynamicObjectResolver()
        {
            _dynamicObjectResolver = null;
        }

        public static void FocusOnLastSelectedObject()
        {
            SceneView.FrameLastActiveSceneView();
        }
    }
}