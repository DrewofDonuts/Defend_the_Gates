using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Characters/Character Editor", fileName = "New Character Editor")]
    public class Characters : ScriptableObject
    {
        public PlayerAttributes Player;
        
        [Searchable]
        public EnemyCategory[] categories;
    }

    [Serializable]
    public class EnemyCategory
    {
        public string category;
        public CharacterInfo[] characters;
    }

    [Serializable]
    public class CharacterInfo
    {
        public AIAttributes Enemy;
    }
}