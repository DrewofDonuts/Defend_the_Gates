using System.Collections.Generic;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    public static class SceneObjectUtils
    {
        private static readonly LinkedList<string> PathBuilder = new LinkedList<string>();

        public static string[] GetScenePathAsArray(this GameObject obj)
        {
            PathBuilder.Clear();
            Transform parent = obj.transform.parent;

            while (parent != null)
            {
                PathBuilder.AddFirst(parent.name);
                parent = parent.parent;
            }

            string[] path = new string[PathBuilder.Count];
            PathBuilder.CopyTo(path, 0);

            return path;
        }

        public static void GetChildrenNonAlloc(this Transform t, List<GameObject> children)
        {
            children.Clear();
            for (int i = 0; i < t.childCount; i++)
            {
                children.Add(t.GetChild(i).gameObject);
            }
        }
    }
}