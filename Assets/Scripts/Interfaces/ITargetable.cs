using System;
using UnityEngine;

namespace Etheral
{
    public interface ITargetable : IAffiliate
    {
        public event Action<ITargetable> OnDestroyed;

        public TargetType TargetType { get; }
        // EnemyStateMachine StateMachine { get; }
        Transform Transform { get; }
        // public bool isObject { get; }
        public void HandleDeadTarget();
        public bool IsDead { get; }

        // [Range(1, 10)]
        // public int Priority { get; }


        // public void OnDestroy();

        public void ShowThatIsTargeted(bool enable);
        public T GetStateMachine<T>() where T : StateMachine;
    }
}