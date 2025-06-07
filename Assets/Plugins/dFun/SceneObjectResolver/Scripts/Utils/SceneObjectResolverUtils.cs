using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DFun.GameObjectResolver
{
    public static class SceneObjectResolverUtils
    {
        public static bool TryResolve(
            this SceneObjectReference sceneObjectReference, bool firstOnly, List<GameObject> candidates)
        {
            return DefaultSceneObjectResolver.Instance.TryResolveNonAlloc(sceneObjectReference, firstOnly, candidates);
        }

        public static bool IsOnActiveScene(this SceneObjectReference sceneObjectReference)
        {
            return IsOnScene(sceneObjectReference, SceneManager.GetActiveScene());
        }

        public static bool IsOnScene(this SceneObjectReference sceneObjectReference, Scene scene)
        {
            if (!sceneObjectReference.HasParentScenePath)
            {
                return false;
            }

            if (!scene.isLoaded)
            {
                return false;
            }

            if (scene.path.Equals(sceneObjectReference.ParentScenePath))
            {
                return true;
            }

            return false;
        }

        public static void GetMatchedParents(
            List<GameObject> rootGameObjects, string[] gameObjectPath, List<Transform> matchedParents)
        {
            matchedParents.Clear();
            for (int i = 0, iSize = rootGameObjects.Count; i < iSize; i++)
            {
                GetMatchedParents(0, rootGameObjects[i].transform, gameObjectPath, matchedParents);
            }
        }

        private static void GetMatchedParents(
            int depth, Transform depthTransform, string[] path, List<Transform> matchedParents)
        {
            if (depth >= path.Length)
            {
                return;
            }

            string depthTargetName = path[depth];
            if (depthTransform.name == depthTargetName)
            {
                if (depth == path.Length - 1)
                {
                    matchedParents.Add(depthTransform);
                }
                else
                {
                    int childCount = depthTransform.childCount;
                    for (int i = 0; i < childCount; i++)
                    {
                        GetMatchedParents(depth + 1, depthTransform.GetChild(i), path, matchedParents);
                    }
                }
            }
        }
    }
}