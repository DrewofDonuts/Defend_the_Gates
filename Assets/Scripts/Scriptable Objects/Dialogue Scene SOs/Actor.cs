using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Actor", menuName = "Etheral/Characters/Actor")]
    [InlineEditor]
    public class Actor : ScriptableObject
    {
        public ActorData actorData;
    }


    [Serializable]
    public struct ActorData
    {
        public CharacterKey characterKey;
        public Sprite sprite;
    }
}