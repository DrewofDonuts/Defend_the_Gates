using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace DFun.GameObjectResolver
{
    public static class SceneUtils
    {
        public static void GetRootObjectsNonAlloc(this Scene scene, List<GameObject> rootGameObjects, int depth = 0)
        {
            if (depth == 0)
            {
                scene.GetRootGameObjects(rootGameObjects);
            }
            else
            {
                using (ListPool<GameObject>.Get(out List<GameObject> tmpDepthObjects))
                {
                    scene.GetRootGameObjects(tmpDepthObjects);
                    AddDepthObjects(0, depth, tmpDepthObjects, rootGameObjects);
                }
            }
        }

        private static void AddDepthObjects(
            int currentDepth, int targetDepth, List<GameObject> depthObjects, List<GameObject> result)
        {
            for (int i = 0, iSize = depthObjects.Count; i < iSize; i++)
            {
                AddDepthObjects(currentDepth, targetDepth, depthObjects[i], result);
            }
        }

        private static void AddDepthObjects(
            int currentDepth, int targetDepth, GameObject depthObject, List<GameObject> result)
        {
            if (currentDepth == targetDepth)
            {
                result.Add(depthObject);
                return;
            }

            for (int i = 0; i < depthObject.transform.childCount; i++)
            {
                AddDepthObjects(
                    currentDepth + 1, targetDepth, depthObject.transform.GetChild(i).gameObject, result
                );
            }
        }
    }
}