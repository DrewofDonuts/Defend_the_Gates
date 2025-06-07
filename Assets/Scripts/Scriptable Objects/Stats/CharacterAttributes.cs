using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

//Will accept Action types as scriptable objects, including:
//Regular attacks
//Heavy Attacks
//Special Attacks
//maybe special moves

namespace Etheral
{
    public class CharacterAttributes : ScriptableObject
    {
        [HorizontalGroup("Basic Info", 100, LabelWidth = 70)]
        [HideLabel, PreviewField(100, ObjectFieldAlignment.Left)]
        public Sprite Icon;

        [VerticalGroup("Basic Info/Center")]
        public string Name;

        [VerticalGroup("Basic Info/Center")]
        [TextArea(3, 8)]
        public string Description;


        [VerticalGroup("Basic Info/Right")]
        [field: FoldoutGroup("Health and Defense", Expanded = true)]
        [field: HorizontalGroup("Health and Defense/1")]
        [field: VerticalGroup("Health and Defense/1/Left")]
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: VerticalGroup("Health and Defense/1/Left")]
        [field: SerializeField] public float MaxDefense { get; private set; } 
        [field: VerticalGroup("Health and Defense/1/Left")]
        [field: SerializeField] public float MaxHolyWill { get; private set; }
        [field: VerticalGroup("Health and Defense/1/Right")]
        [field: SerializeField] public float HealthRegenRate { get; private set; } 
        [field: VerticalGroup("Health and Defense/1/Right")]
        [field: SerializeField] public float StaminaRegenRate { get; private set; } 
        [field: VerticalGroup("Health and Defense/1/Right")]
        [field: SerializeField] public float DefenseRegenRate { get; private set; } 
        [field: VerticalGroup("Health and Defense/1/Left")]
        [field: SerializeField] public float KnockBackDefense { get; private set; }
        [field: VerticalGroup("Health and Defense/1/Left")]
        [field: SerializeField] public float KnockDownDefense { get; private set; } 
        [field: VerticalGroup("Health and Defense/1/Right")]
        [field: SerializeField] public float BlockingAngle { get; private set; } = 90f;


        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float WalkSpeed { get; private set; } = 3.5f;
        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float RunSpeed { get; private set; } = 5f;
        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float SprintSpeed { get; private set; } = 6f;
        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float StrafeSpeed { get; private set; } = 4f;
        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float RotateSpeed { get; private set; } = 120f;

        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float Acceleration { get; private set; } = 2.75f;
        [field: FoldoutGroup("Movement")]
        [field: SerializeField] public float Deceleration { get; private set; } = 3.75f;
        
    }
}