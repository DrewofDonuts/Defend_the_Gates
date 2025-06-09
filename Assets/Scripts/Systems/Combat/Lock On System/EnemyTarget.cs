using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class EnemyTarget : MonoBehaviour, ITargetable
    {
        [Title("Load Components from Enemy Component Handler")]
        [field: SerializeField] public EnemyStateMachine StateMachine { get; private set; }
        public TargetType TargetType { get; } = TargetType.Enemy;
        public Transform Transform { get; private set; }
        [field: SerializeField] public bool isObject { get; private set; }
        public event Action<ITargetable> OnDestroyed;
        public bool IsDead { get; private set; }
        public int Priority { get; }

        void Awake()
        {
            if (StateMachine == null)
            {
                StateMachine = GetComponentInParent<EnemyStateMachine>();
                Debug.LogError($"Enemy StateMachine was null for {name}. Attempting to find it in parent.");
            }

            gameObject.layer = LayerMask.NameToLayer("Enemy");


            ShowThatIsTargeted(false);
            Transform = transform;
        }

        void OnEnable()
        {
            StateMachine.Health.OnDie += HandleDeadTarget;

            if (StateMachine.GetAIComponents().GetHeadExecutionPoint() != null)
                StateMachine.GetAIComponents().GetHeadExecutionPoint().OnDestroyed += HandleDeadTarget;
            else
                Debug.LogError("Head Execution Point is null");
        }

        void HandleDeadTarget(Health obj)
        {
            OnDestroyed?.Invoke(this);
            IsDead = true;
        }

        public void HandleDeadTarget()
        {
            OnDestroyed?.Invoke(this);
            IsDead = true;
        }


  

        public void OnDestroy()
        {
            StateMachine.Health.OnDie -= HandleDeadTarget;

            if (StateMachine.GetAIComponents().GetHeadExecutionPoint() != null)
                StateMachine.GetAIComponents().GetHeadExecutionPoint().OnDestroyed -= HandleDeadTarget;
        }

        
        //CREATE METHODS TO CALL WITH PROFILES DIRECTLY IN HIGHLIGHT EFFECT CONTROLLER
        public void ShowThatIsTargeted(bool enable)
        {
            if (enable)
            {
                // StateMachine.AIComponents().GetHighlightEffectController().SetHighlightEffect(StateMachine.HighlightEffectController.LockOnHighlightEffect,
                //     StateMachine.CharacterAttributes.HighlightProfile.LockedOnProfile);
                //
                // StateMachine.AIComponents().GetHighlightEffectController().ToggleHighlightEffect(StateMachine.HighlightEffectController.LockOnHighlightEffect, true);
                
                StateMachine.GetAIComponents().GetHighlightEffectController().ToggleLockedOnEffect(true);
            }

            else
            {
                // StateMachine.HighlightEffectController.ToggleHighlightEffect(
                //     StateMachine.HighlightEffectController.LockOnHighlightEffect, false);
                
                StateMachine.GetAIComponents().GetHighlightEffectController().ToggleLockedOnEffect(false);

            }
        }

        public T GetStateMachine<T>() where T : StateMachine
        {
            return StateMachine as T;
        }

#if UNITY_EDITOR

        public void LoadComponents()
        {
            StateMachine = GetComponentInParent<EnemyStateMachine>();
        }
#endif
        public Affiliation Affiliation { get; set; }
    }
}


// void Update()
// {
//     if (StateMachine.CombatManager == null) return;
//     
//     if (this == StateMachine.CombatManager.CurrentTarget && !StateMachine.targetingInfo.activeSelf)
//     {
//         ShowThatIsTargeted(true);
//     }
//     else if (this != StateMachine.CombatManager.CurrentTarget && StateMachine.targetingInfo.activeSelf)
//     {
//         ShowThatIsTargeted(false);
//     }
// }