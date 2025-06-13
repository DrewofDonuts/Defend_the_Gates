using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class TurretLockOnController : MonoBehaviour
    {
        public EnemyTarget currentTarget;

        [Header("Debug")]
        public Transform targetTransform;
        public List<EnemyTarget> targets = new();


        public event Action<Transform> OnTargetChanged;

        float triggerStayTimer;
        float triggerStayInterval = .25f;

        void UpdateCurrentTargetByDistance()
        {
            if (targets.Count == 0)
                return;


            float closestDistance = float.MaxValue;
            EnemyTarget closestTarget = null;

            foreach (var target in targets)
            {
                float distance = Vector3.Distance(target.transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            if (closestTarget != null)
            {
                currentTarget = closestTarget;
                targetTransform = closestTarget.transform;
                OnTargetChanged?.Invoke(targetTransform);
            }

            if (closestTarget == null)
            {
                targetTransform = null;
                OnTargetChanged?.Invoke(null);
            }
        }

        void OnTriggerStay(Collider other)
        {
            triggerStayTimer += Time.deltaTime;

            if (triggerStayTimer < triggerStayInterval)
                return;
            if (other.TryGetComponent(out EnemyTarget target))
            {
                if (targets.Contains(target)) return;
                if (currentTarget == null)
                {
                    targets.Add(target);
                    target.OnDestroyed += RemoveTarget;

                    // OnTargetChanged?.Invoke(targetTransform);
                    UpdateCurrentTargetByDistance();
                }
            }
            triggerStayTimer = 0f;
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyTarget target))
            {
                if (!targets.Contains(target)) return;
                if (currentTarget == target)
                {
                    RemoveTarget(currentTarget);
                    UpdateCurrentTargetByDistance();
                }
            }
        }

        void RemoveTarget(ITargetable _target)
        {
            currentTarget.OnDestroyed -= RemoveTarget;
            targets.Remove(currentTarget);
            currentTarget = null;
            if (targets.Count <= 0)
                OnTargetChanged?.Invoke(null);
        }
    }
}