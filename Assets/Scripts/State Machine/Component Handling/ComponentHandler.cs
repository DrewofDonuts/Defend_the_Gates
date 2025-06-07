using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public abstract class ComponentHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] AnimationHandler animationHandler;
        [SerializeField] GameObject canvasGroup;

        [Header("Locomotion")]
        [SerializeField] CharacterController characterController;
        [SerializeField] ClimbController climbController;
        [SerializeField] ParkourController parkourController;

        [Header("Combat")]
        [SerializeField] AffiliationController affiliationController;
        [SerializeField] RangedWeaponDamage rangedWeaponDamage;
        [SerializeField] SpellHandler spellHandler;
        [SerializeField] Head head;
        [SerializeField] CharacterCollision characterCollision;


        public Head GetHead() => head;
        public ParkourController GetParkourController() => parkourController;
        public ClimbController GetClimbController() => climbController;
        public GameObject GetCanvasGroup() => canvasGroup;
        public AnimationHandler GetAnimationHandler() => animationHandler;
        public RangedWeaponDamage GetRangedWeaponDamage() => rangedWeaponDamage;
        public CharacterController GetCC() => characterController;
        public AffiliationController GetAffiliationController() => affiliationController;
        public SpellHandler GetSpellHandler() => spellHandler;
        public CharacterCollision GetCharacterCollision() => characterCollision;
        
        public Camera GetCamera() => Camera.main;


#if UNITY_EDITOR

        public virtual void LoadComponents()
        {
            Debug.Log("Loading components");
            head = GetComponentInChildren<Head>();
            animationHandler = GetComponent<AnimationHandler>();
            parkourController = GetComponentInChildren<ParkourController>();
            climbController = GetComponentInChildren<ClimbController>();
            rangedWeaponDamage = GetComponentInChildren<RangedWeaponDamage>();
            characterController = GetComponent<CharacterController>();
            affiliationController = GetComponent<AffiliationController>();
            spellHandler = GetComponentInChildren<SpellHandler>();
            characterCollision = GetComponentInChildren<CharacterCollision>();
        }
#endif
    }
}