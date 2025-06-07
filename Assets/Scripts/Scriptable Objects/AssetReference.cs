using System;
using UnityEngine;


namespace Etheral
{
    [Obsolete]
    public class AssetReference : ScriptableObject
    {
        [SerializeField] Animations[] animationSections;
        [SerializeField] Textures[] texturesSection;
        [SerializeField] Prefabs[] prefabSections;
    }

    public class AssetTypes
    {
        [SerializeField] string assetType;
        [SerializeField] string description;
    }


    [Serializable]
    public class Animations : AssetTypes
    {
        [SerializeField] AnimationClip[] animationClips;
    }

    [Serializable]
    public class Textures : AssetTypes
    {
        [SerializeField] Texture[] textures;
    }

    [Serializable]
    public class Prefabs : AssetTypes
    {
        [SerializeField] GameObject[] prefabs;
    }
}