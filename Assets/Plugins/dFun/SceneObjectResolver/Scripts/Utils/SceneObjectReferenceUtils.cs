using UnityEngine;

namespace DFun.GameObjectResolver
{
    public static class SceneObjectReferenceUtils
    {
        public static SceneObjectReference CreateSceneObjectReference(this GameObject go, bool asDirectLink = false)
        {
            SceneObjectReference sceneObjectReference = new SceneObjectReference();
            if (asDirectLink)
            {
                sceneObjectReference.GameObject = go;
            }

            sceneObjectReference.ParentScenePath = go.scene.path;
            sceneObjectReference.GameObjectScenePath = go.GetScenePathAsArray();
            sceneObjectReference.GameObjectName = go.name;

            sceneObjectReference.SiblingIndex = go.transform.GetSiblingIndex();

            return sceneObjectReference;
        }
    }
}