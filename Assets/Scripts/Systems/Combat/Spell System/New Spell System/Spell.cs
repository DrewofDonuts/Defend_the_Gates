using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Ability System/Spell")]
    [InlineEditor]
    public class Spell : ScriptableObject
    {
        public Sprite icon;
        [Header("Cast  Settings")]
        public CastDirection castDirection = CastDirection.None;
        
        [FormerlySerializedAs("InstantiateAtCastPoint")]
        public bool instantiateAtCastPoint;

        [FormerlySerializedAs("Offset")] [Header("Offset")]
        public Vector3 offset;

        [FormerlySerializedAs("IsChildOfCaster")] [Header("Parenting Settings")]
        public bool isChildOfCaster;

        [FormerlySerializedAs("ActiveOnHold")] [Header("Control Settings")]
        public bool activeOnHold;

        [Header("Spell Object")]
        public SpellObject spellObject;


        public bool InstantiateAtCastPoint => instantiateAtCastPoint;
        public Vector3 Offset => offset;
        public bool IsChildOfCaster => isChildOfCaster;
        public bool IsActiveOnHold => activeOnHold;
        public SpellObject SpellObject => spellObject;
        public CastDirection CastDirection => castDirection;
    }
}