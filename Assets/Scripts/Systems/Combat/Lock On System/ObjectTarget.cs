using System;
using UnityEngine;

namespace Etheral
{
    public class ObjectTarget : MonoBehaviour, ITargetable
    {
        [field: SerializeField] public bool isObject { get; private set; } = true;
        [SerializeField]  int priority = 5;

        public int Priority => priority;

        [field: SerializeField] public bool IsTargetable { get; private set; } = false;
        public TargetType TargetType { get; }
        public Transform Transform { get; private set; }
        public event Action<ITargetable> OnDestroyed;
        [field: SerializeField] public Affiliation Affiliation { get; set; }

        public bool IsDead { get; private set; }

        void OnEnable()
        {
            Transform = transform;
        }

        public void HandleDeadTarget()
        {
            OnDestroyed?.Invoke(this);
            IsDead = true;
        }

        public T GetStateMachine<T>() where T : StateMachine
        {
            return null;
        }

        public void ShowThatIsTargeted(bool enable) { }
    }
}