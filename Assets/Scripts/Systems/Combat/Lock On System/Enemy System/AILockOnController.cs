using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
     public class AILockOnController : MonoBehaviour
    {
        [SerializeField] List<TargetType> aiPriorities;
        [SerializeField] float timeBetweenChecks = 0.5f;
        

        public List<TargetType> AIPriorities => aiPriorities;


        [Header("References")]
        [SerializeField] Transform raycastOrigin;


        [Header("Raycast Settings")]
        [SerializeField] float raycastDistance = 4f;
        [SerializeField] float numberOfRays = 3f;
        [SerializeField] List<LayerMask> raycastLayers = new();

        [Header("Debug")]
        [ReadOnly]
        public Transform currentTarget;


        [ReadOnly]
        public List<Transform> targetTransforms = new();

        [ReadOnly]
        public float timer;
        ITargetable overrideTarget;
        public event Action<Transform> OnTargetChanged;

        List<ITargetable> targets = new();




        // public CompanionStateMachine companion;

        
        void Update()
        {
            // if (targets.Count == 0)
            // {
            //     if (timer > 0)
            //         timer = 0;
            //     return;
            // }
            //
            // timer += Time.deltaTime;
            //
            // if (timer < timeBetweenChecks)
            //     return;
            //
            //
            // timer = 0f;
        }

        ITargetable FindClosestTarget()
        {
            ITargetable closestTarget = null;
            float closestDistance = float.MaxValue;


            foreach (var target in targets)
            {
                if (target == null || target.IsDead) continue;

                float distance = Vector3.Distance(transform.position, target.Transform.position);

                if (distance < closestDistance)
                {
                    closestTarget = target;
                    closestDistance = distance;
                }
            }

            if (closestTarget != null)
            {
                currentTarget = closestTarget.Transform;
                OnTargetChanged?.Invoke(currentTarget);
            }
            else
            {
                currentTarget = null;
                OnTargetChanged?.Invoke(null);
            }

            return closestTarget;
        }


        public ITargetable GetTarget()
        {
            return FindClosestTarget();
        }

        void OnTriggerStay(Collider other)
        {
            timer += Time.deltaTime;
            
            if (timer < timeBetweenChecks) return;
            
            if (other.transform.root == transform.root) return;
            if (targetTransforms.Contains(other.transform)) return;
            if (!other.TryGetComponent(out ITargetable target)) return;
            // if (target.Affiliation == Affiliation.Enemy) return;
            if (!aiPriorities.Contains(target.TargetType)) return;
            
            Debug.Log($"Adding target: {target.TargetType}");
            
            if (target.IsDead) return;
            
            if (!targets.Contains(target))
            {
                targets.Add(target);
                targetTransforms.Add(target.Transform);
                target.OnDestroyed += RemoveTarget;
            }
            
            timer = 0f;
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out ITargetable target)) return;
            if (!targetTransforms.Contains(other.transform)) return;
            // if (target.Affiliation == Affiliation.Enemy) return;
            if (!aiPriorities.Contains(target.TargetType)) return;


            RemoveTarget(target);

            // targetTransforms.Remove(target.Transform);
            // OnTargetChanged?.Invoke(null);
        }

        void RemoveTarget(ITargetable targetToRemove)
        {
            targetToRemove.OnDestroyed -= RemoveTarget;
            targets.Remove(targetToRemove);
            targetTransforms.Remove(targetToRemove.Transform);
            if (currentTarget == targetToRemove.Transform)
            {
                currentTarget = null;
                OnTargetChanged?.Invoke(null);
            }

        }
    }
}