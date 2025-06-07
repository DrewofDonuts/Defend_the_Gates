using System;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    [Serializable]
    public class SceneObjectReference
    {
        [SerializeField] private GameObject gameObject;
        [SerializeField] private string parentScenePath;
        [SerializeField] private string[] gameObjectScenePath;
        [SerializeField] private string gameObjectName;
        [SerializeField] private int siblingIndex = DefaultSiblingIndex;

        public GameObject GameObject
        {
            get => gameObject;
            set => gameObject = value;
        }

        public string ParentScenePath
        {
            get => parentScenePath;
            set => parentScenePath = value;
        }

        public bool HasParentScenePath => ParentScenePath != null;

        public string[] GameObjectScenePath
        {
            get => gameObjectScenePath;
            set => gameObjectScenePath = value;
        }

        public bool HasGameObjectScenePath => GameObjectScenePath != null
                                              && GameObjectScenePath.Length > 0;

        public string GameObjectName
        {
            get => gameObjectName;
            set => gameObjectName = value;
        }

        private const int DefaultSiblingIndex = -1;

        public int SiblingIndex
        {
            get => siblingIndex;
            set => siblingIndex = value;
        }

        public bool HasSiblingIndex => SiblingIndex != DefaultSiblingIndex && SiblingIndex > 0;

        public string ReferenceName => GameObjectName;
        public bool ContainsData => GameObject != null || ParentScenePath != null;

        public SceneObjectReference()
        {
        }

        public SceneObjectReference(SceneObjectReference copyFrom)
        {
            GameObject = copyFrom.GameObject;
            ParentScenePath = copyFrom.ParentScenePath;

            string[] copyFromPath = copyFrom.GameObjectScenePath;
            if (copyFromPath != null)
            {
                GameObjectScenePath = new string[copyFromPath.Length];
                for (int i = 0, iSize = GameObjectScenePath.Length; i < iSize; i++)
                {
                    string copyPathElement = copyFromPath[i];
                    GameObjectScenePath[i] = copyPathElement == null ? null : string.Copy(copyPathElement);
                }
            }

            GameObjectName = copyFrom.GameObjectName;
            SiblingIndex = copyFrom.SiblingIndex;
        }
    }
}