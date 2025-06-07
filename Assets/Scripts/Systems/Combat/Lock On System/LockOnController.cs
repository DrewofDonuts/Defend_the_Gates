using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral
{
    [RequireComponent(typeof(SphereCollider))]
    public class LockOnController : MonoBehaviour
    {
        [SerializeField] InputObject input;
        [SerializeField] Image lockOnImage;
        [SerializeField] Sprite lockOnIcon;
        [SerializeField] Sprite lockOffIcon;


        public ITargetable CurrentEnemyTarget { get; protected set; }
        public List<ITargetable> _targets = new();
        public List<Transform> targetTransforms = new();
        public Transform currentTargetTransform;
        public event Action<ITargetable> OnCurrentTarget;


        ITargetable _previousEnemyTarget;

        bool lockOnToggled = true;

        void OnEnable()
        {
            lockOnToggled = false;
            lockOnImage.sprite = lockOffIcon;
            input.LeftStickDownEvent += ToggleIsLockOn;
        }

        void OnDisable()
        {
            input.LeftStickDownEvent -= ToggleIsLockOn;
        }

        void ToggleIsLockOn()
        {
            if (lockOnToggled)
            {
                lockOnToggled = false;
                lockOnImage.sprite = lockOffIcon;
            }
            else
            {
                lockOnToggled = true;
                lockOnImage.sprite = lockOnIcon;
            }
        }


        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITargetable target) && !_targets.Contains(target) && !target.IsDead)
            {
                if (target.Affiliation == Affiliation.Fellowship) return;
                _targets.Add(target);
                targetTransforms.Add(target.Transform);
                target.OnDestroyed += RemoveTarget;
            }
        }

        // protected virtual void OnTriggerStay(Collider other)
        // {
        //     if (other.TryGetComponent(out ITargetable target) && !_targets.Contains(target) && !target.isDead)
        //     {
        //         _targets.Add(target);
        //         _targetTransforms.Add(target.Transform);
        //         target.OnDestroyed += RemoveTarget;
        //     }
        // }


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
            if (target.Affiliation == Affiliation.Fellowship) return;

            OnCurrentTarget?.Invoke(null);
            RemoveTarget(target);
        }

        public bool IsLockEnabled() => lockOnToggled;

        public bool SelectTarget()
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
            targetTransforms.Remove(enemyTarget.Transform);
        }

        public Transform GetCurrentTargetTransform()
        {
            return currentTargetTransform;
        }

        public ITargetable GetCurrentTarget()
        {
            if (CurrentEnemyTarget == null)
                return null;

            return CurrentEnemyTarget;
        }
    }
}