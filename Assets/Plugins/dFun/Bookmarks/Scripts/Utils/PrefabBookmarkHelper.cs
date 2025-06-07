using System.Collections.Generic;
using DFun.GameObjectResolver;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_2021_2_OR_NEWER
using UnityEditor.SceneManagement;
#elif UNITY_2018_3_OR_NEWER
using UnityEditor.Experimental.SceneManagement;
#endif

namespace DFun.Bookmarks
{
    public static class PrefabBookmarkHelper
    {
        private static readonly List<GameObject> TemporaryObjectCandidates = new List<GameObject>();

        public static bool OpenBookmark(Bookmark bookmark, bool select, out Object bookmarkObject,
            SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (prefabStage == null)
            {
                bookmarkObject = default;
                return false;
            }

            if (TryToOpenBookmark(
                    bookmark, prefabStage.prefabContentsRoot, select, out bookmarkObject, selectionFlags
                ))
            {
                return true;
            }

#if UNITY_2021_2_OR_NEWER
            if (TryToOpenBookmark(
                    bookmark, prefabStage.openedFromInstanceObject, select, out bookmarkObject, selectionFlags
                ))
            {
                return true;
            }

            if (TryToOpenBookmark(
                    bookmark, prefabStage.openedFromInstanceRoot, select, out bookmarkObject, selectionFlags
                ))
            {
                return true;
            }
#endif

            bookmarkObject = default;
            return false;
        }

        public static bool TryToGetContextPrefab(Object o, out string contextPrefabAssetPath)
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null)
            {
                contextPrefabAssetPath = default;
                return false;
            }

            contextPrefabAssetPath = GetPrefabAssetPath(prefabStage);
            return !string.IsNullOrEmpty(contextPrefabAssetPath);
        }

        private static string GetPrefabAssetPath(PrefabStage prefabStage)
        {
#if UNITY_2020_1_OR_NEWER
            return prefabStage.assetPath;
#else
            return prefabStage.prefabAssetPath;
#endif
        }

        private static bool TryToOpenBookmark(
            Bookmark bookmark, GameObject rootObject, bool select,
            out Object bookmarkObject, SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            if (rootObject == null)
            {
                bookmarkObject = default;
                return false;
            }

            SceneObjectReference sceneObjectReference = bookmark.ObjectReference?.SceneObjectReference;
            if (sceneObjectReference == null)
            {
                bookmarkObject = default;
                return false;
            }

            Scene prefabObjectScene = rootObject.scene;
            SceneByPathAndNameResolver prefabResolver = new SceneByPathAndNameResolver(() => prefabObjectScene);

            TemporaryObjectCandidates.Clear();
            if (prefabResolver.TryResolveNonAlloc(sceneObjectReference, false, TemporaryObjectCandidates))
            {
                bookmarkObject = TemporaryObjectCandidates.Count > 0 ? TemporaryObjectCandidates[0] : default;
                if (select)
                {
                    return SceneBookmarkHelper.SelectObjects(TemporaryObjectCandidates, selectionFlags);
                }

                return bookmarkObject != default;
            }

            bookmarkObject = default;
            return false;
        }
    }
}