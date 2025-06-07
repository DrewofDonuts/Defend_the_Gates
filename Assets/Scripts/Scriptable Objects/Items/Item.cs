using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
//parent class to specific items
    [InlineEditor]
    [CreateAssetMenu(fileName = "New Item", menuName = "Etheral/Items/New Item")]
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        [HideLabel, PreviewField(55, ObjectFieldAlignment.Left)]
        public Sprite Icon;
        public AudioClip itemPickupSound;

        public string itemName;
        public bool isUnique;
        public GameObject itemPrefab;

        [TextArea(5, 5)]
        [SerializeField] string description;

        public ItemType itemType;
    }

    public enum ItemType
    {
        Food = 0,
        Default = 1,
        Equipment = 2,
        Gold = 3,
        Potion = 4,
        QuestItem = 5,
        Weapon = 6,
        Ammo = 7,
    }
}