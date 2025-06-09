using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [DisallowMultipleComponent]
    public class FellowshipTarget : MonoBehaviour, ITargetable
    {
        [SerializeField] TargetType targetType;
        [SerializeField] StateMachine stateMachine;
        public Transform Transform => transform;
        public TargetType TargetType => targetType;


        public bool IsDead { get; private set; }
        public int Priority { get; }
        public bool IsTargetable { get; }

        void Start()
        {
            if (stateMachine != null)
                stateMachine.Health.OnDie += HandleDeadTarget;
        }

        void OnDestroy()
        {
            if (stateMachine != null)
                stateMachine.Health.OnDie -= HandleDeadTarget;
        }


        public void HandleDeadTarget(Health health)
        {
            OnDestroyed?.Invoke(this);
            IsDead = true;
        }

        //required for now - I know it's redundant but may be needed for other systems
        public void HandleDeadTarget() { }


        public void ShowThatIsTargeted(bool enable) { }

        public T GetStateMachine<T>() where T : StateMachine
        {
            return stateMachine as T;
        }

        public event Action<ITargetable> OnDestroyed;


        void Awake()
        {
            if (stateMachine == null)
            {
                stateMachine = GetComponentInParent<StateMachine>();
            }
        }

        public Affiliation Affiliation { get; set; } = Affiliation.Fellowship;
    }
}