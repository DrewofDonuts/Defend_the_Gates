using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Asset Holder", menuName = "Etheral/Holders/Asset Library")]
public class AssetLibrary : ScriptableObject
{
    [Searchable]
    public AssetSection[] assetSections;
}

[Serializable]
public class AssetSection
{
    public string name;
    public Object[] objects;
}