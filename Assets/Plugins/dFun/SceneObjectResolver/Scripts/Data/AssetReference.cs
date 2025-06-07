using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.GameObjectResolver
{
    [Serializable]
    public class AssetReference
    {
        [SerializeField] private Object assetObject;
        [SerializeField] private string assetGuid;

        public Object AssetObject
        {
            get => assetObject;
            set => assetObject = value;
        }

        public string AssetGuid
        {
            get => assetGuid;
            set => assetGuid = value;
        }

        public string ReferenceName => assetObject != null ? AssetObject.name : string.Empty;
        public bool ContainsData => assetObject != null || AssetObject != null;

        public AssetReference()
        {
        }

        public AssetReference(AssetReference copyFrom)
        {
            AssetObject = copyFrom.AssetObject;
            AssetGuid = copyFrom.AssetGuid;
        }
    }
}