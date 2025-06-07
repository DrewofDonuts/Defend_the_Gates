using System;
using UnityEngine;

namespace Etheral
{
    public class TurretLockOnController : MonoBehaviour
    {
        EnemyTarget currentTarget;
        public Transform targetTransform;
        public event Action<Transform> OnTargetChanged;


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyTarget target))
            {
                if (currentTarget == null)
                {
                    currentTarget = target;
                    targetTransform = target.transform;
                    target.OnDestroyed += RemoveTarget;
                    OnTargetChanged?.Invoke(targetTransform);
                }
            }
        }

        void RemoveTarget(ITargetable _target)
        {
            currentTarget.OnDestroyed -= RemoveTarget;
            OnTargetChanged?.Invoke(null);
            currentTarget = null;
        }


        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyTarget target))
            {
                if (currentTarget == target)
                {
                    RemoveTarget(currentTarget);
                }
            }
        }
    }
}