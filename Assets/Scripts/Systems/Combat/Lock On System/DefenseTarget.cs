using System;
using UnityEngine;

namespace Etheral
{
    public class DefenseTarget : MonoBehaviour, ITargetable
    {
        [field: SerializeField] public TargetType TargetType { get; set; }
        [field: SerializeField] public Affiliation Affiliation { get; set; }
        [SerializeField] Gate gate;


        public Transform Transform { get; private set; }
        public event Action<ITargetable> OnDestroyed;

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