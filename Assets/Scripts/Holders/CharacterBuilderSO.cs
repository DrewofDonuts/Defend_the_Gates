using System;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "CharacterBuilderSO", menuName = "Etheral/CharacterBuilderSO")]
    public class CharacterBuilderSO : ScriptableObject
    {
        public Sprite icon;
        public CharacterType[] characterTypes;
        public AnimationType[] animationTypes;
        public AnimationClips[] animationClips;
        public WeaponType[] weaponTypes;
        public VFXType[] vfxTypes;
        public SFXType[] sfxTypes;
    }


    [Serializable]
    public class CharacterType
    {
        public string type;
        public GameObject[] characterPrefabs;
    }

    [Serializable]
    public class AnimationType
    {
        public string type;
        public GameObject[] animationPrefabs;
    }

    [Serializable]
    public class WeaponType
    {
        public string type;
        public GameObject[] weaponPrefabs;
    }

    [Serializable]
    public class VFXType
    {
        public string type;
        public GameObject[] vfxPrefabs;
    }

    [Serializable]
    public class SFXType
    {
        public string type;
        public AudioClip[] sfxPrefabs;
    }

    [Serializable]
    public class AnimationClips
    {
        public string type;
        public AnimationClip[] animationClips;
    }
    
}