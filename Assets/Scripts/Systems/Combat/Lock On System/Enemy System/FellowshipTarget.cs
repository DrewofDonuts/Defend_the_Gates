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

        public void HandleDeadTarget()
        {
            throw new NotImplementedException();
        }

        public bool IsDead { get; }
        public int Priority { get; }
        public bool IsTargetable { get; }


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