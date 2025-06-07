using System;
using DFun.UnityDataTypes;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    [Serializable]
    public class ObjectReference
    {
        [SerializeField] private string globalObjectIdString;
        [SerializeField] private SceneObjectReference sceneObjectReference;
        [SerializeField] private AssetReference assetReference;
        [SerializeField] private ComponentReference componentReference;
        [SerializeField] private AssetReference contextAssetReference;
        [SerializeField] private InvokesReference invokesReference;
        [SerializeField] private TypeReference typeReference;

        public string GlobalObjectId
        {
            get => globalObjectIdString;
            set => globalObjectIdString = value;
        }

        public SceneObjectReference SceneObjectReference
        {
            get => sceneObjectReference;
            set => sceneObjectReference = value;
        }

        public AssetReference AssetReference
        {
            get => assetReference;
            set => assetReference = value;
        }

        public ComponentReference ComponentReference
        {
            get => componentReference;
            set => componentReference = value;
        }

        /// Scene or prefab
        public AssetReference ContextAssetReference
        {
            get => contextAssetReference;
            set => contextAssetReference = value;
        }

        public InvokesReference InvokesReference
        {
            get => invokesReference;
            set => invokesReference = value;
        }

        public TypeReference TypeReference
        {
            get => typeReference;
            set => typeReference = value;
        }

        public string Name
        {
            get
            {
                string name = string.Empty;
                if (AssetReference != null && AssetReference.ContainsData)
                {
                    name += AssetReference.ReferenceName;
                }
                else
                {
                    if (SceneObjectReference != null && SceneObjectReference.ContainsData)
                    {
                        name += SceneObjectReference.ReferenceName;
                    }
                }

                if (ComponentReference != null && ComponentReference.ContainsData)
                {
                    name += ComponentReference.ReferenceName;
                }

                if (TypeReference != null && TypeReference.ContainsData)
                {
                    name += TypeReference.TypeName;
                }

                return name;
            }
        }

        public ObjectReference()
        {
            globalObjectIdString = string.Empty;
            sceneObjectReference = new SceneObjectReference();
            assetReference = new AssetReference();
            componentReference = new ComponentReference();
            contextAssetReference = new AssetReference();
            invokesReference = new InvokesReference();
            typeReference = new TypeReference();
        }

        public ObjectReference(ObjectReference copyFrom)
        {
            globalObjectIdString = copyFrom.globalObjectIdString;

            SceneObjectReference = copyFrom.SceneObjectReference != null
                ? new SceneObjectReference(copyFrom.SceneObjectReference)
                : null;

            AssetReference = copyFrom.AssetReference != null
                ? new AssetReference(copyFrom.AssetReference)
                : null;

            ComponentReference = copyFrom.ComponentReference != null
                ? new ComponentReference(copyFrom.ComponentReference)
                : null;

            ContextAssetReference = copyFrom.ContextAssetReference != null
                ? new AssetReference(copyFrom.ContextAssetReference)
                : null;

            InvokesReference = copyFrom.InvokesReference != null
                ? new InvokesReference(copyFrom.InvokesReference)
                : null;

            TypeReference = copyFrom.TypeReference != null
                ? new TypeReference(copyFrom.TypeReference)
                : null;
        }
    }
}