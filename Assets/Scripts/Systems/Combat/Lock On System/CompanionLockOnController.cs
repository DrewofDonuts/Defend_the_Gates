using System;
using System.Collections.Generic;
using Etheral.Combat;
using UnityEngine;

namespace Etheral
{
    public class CompanionLockOnController : MonoBehaviour
    {
        public ITargetable CurrentEnemyTarget { get; protected set; }
        public List<ITargetable> _targets = new();
        public List<Transform> _targetTransforms { get; private set; } = new();
        public Transform currentTargetTransform;
        public event Action<ITargetable> OnCurrentTarget;

        ITargetable _previousEnemyTarget;


        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITargetable target) && !_targets.Contains(target) && !target.IsDead)
            {
                if (target.Affiliation != Affiliation.Enemy) return;

                _targets.Add(target);
                _targetTransforms.Add(target.Transform);
                target.OnDestroyed += RemoveTarget;
            }
        }

        protected virtual void Update()
        {
            if (CurrentEnemyTarget != null)
            {
                float distance = Vector3.Distance(CurrentEnemyTarget.Transform.position, transform.position);

                if (distance > 5)
                {
                    CurrentEnemyTarget.ShowThatIsTargeted(false);
                    RemoveTarget(CurrentEnemyTarget);
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out ITargetable target)) return;
            OnCurrentTarget?.Invoke(null);
            RemoveTarget(target);
        }

        public bool GetTarget()
        {
            if (_targets.Count == 0) return false;

            // if (!lockOnToggled) return false;


            ITargetable closeEnemyTarget = null;
            float closestTargetDistance = Mathf.Infinity; //need a big number so the first target is always smaller

            foreach (var target in _targets)
            {
                if (target.IsDead) continue;
                var distanceToPlayer = Vector3.Distance(transform.position, target.Transform.position);
                if (distanceToPlayer < closestTargetDistance)
                {
                    closeEnemyTarget = target;
                    closestTargetDistance = distanceToPlayer;
                }
            }

            if (closeEnemyTarget == null)
            {
                return false;
            }


            //pass Current to Previous in order to remove the lock on indicator
            _previousEnemyTarget = CurrentEnemyTarget;
            CurrentEnemyTarget = closeEnemyTarget;
            currentTargetTransform = CurrentEnemyTarget.Transform;
            CurrentEnemyTarget.ShowThatIsTargeted(true);

            if (_previousEnemyTarget != CurrentEnemyTarget && _previousEnemyTarget != null)
            {
                _previousEnemyTarget.ShowThatIsTargeted(false);
            }

            OnCurrentTarget?.Invoke(CurrentEnemyTarget);

            return true;
        }


        public void Cancel()
        {
            CurrentEnemyTarget = null;
        }

        protected void RemoveTarget(ITargetable enemyTarget)
        {
            //first, clear the Current Enemy and Target if it's the same
            if (CurrentEnemyTarget == enemyTarget)
            {
                CurrentEnemyTarget.ShowThatIsTargeted(false);
                CurrentEnemyTarget = null;
                currentTargetTransform = null;
            }

            //Unsubscribe from the event
            enemyTarget.OnDestroyed -= RemoveTarget;

            //Remove from list
            _targets.Remove(enemyTarget);
            _targetTransforms.Remove(enemyTarget.Transform);
        }

        public virtual Transform GetCurrentTargetTransform()
        {
            return currentTargetTransform;
        }
    }
}