using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class GroundExecutionPointDetector : MonoBehaviour
    {
        //Detects if head is near
        //Determine closest head to player
        //If action taken within proximity, transmit to headRe

        [field: SerializeField] public float DetectionRange { get; private set; } = 1f;
        [field: SerializeField] public LayerMask LayerMask { get; private set; }


        [field: SerializeField] public HeadExecutionPoint CurrentHeadExecutionPoint { get; private set; }
        public List<HeadExecutionPoint> _executionPoints = new();

        public bool canDetect;

        void Update()
        {
            // if (canDetect)

            DetectHeadReceiver();

            // if (_executionPoints.Count > 0)
            //     RemoveHeadReceiver();
        }

        public void DetectHeadReceiver()
        {
            // Physics.SphereCast(transform.position, DetectionRange, transform.forward, out RaycastHit hit,
            //     DetectionRange,
            //     LayerMask);

            var results = Physics.OverlapSphere(transform.position, DetectionRange, LayerMask,
                QueryTriggerInteraction.Collide);

            // if (hit.collider == null) return;

            foreach (var result in results)
            {
                if (result.TryGetComponent(out HeadExecutionPoint executionPoint))
                {
                    if (!_executionPoints.Contains(executionPoint))
                    {
                        if (executionPoint.CanBeHit)
                        {
                            _executionPoints.Add(executionPoint);
                            executionPoint.OnDestroyed += RemoveExecutionPoint;
                        }
                    }
                }
            }
        }

        public bool SelecClosestExecutionPoint()
        {
            HeadExecutionPoint closestHeadExecutionPoint = null;
            float closestDistance = Mathf.Infinity;

            foreach (var executionPoint in _executionPoints)
            {
                var distanceToPlayer = Vector3.Distance(transform.position, executionPoint.transform.position);

                if (distanceToPlayer < closestDistance)
                {
                    closestHeadExecutionPoint = executionPoint;
                    closestDistance = distanceToPlayer;
                }
            }

            if (closestHeadExecutionPoint == null || closestDistance > DetectionRange)
                return false;

            CurrentHeadExecutionPoint = closestHeadExecutionPoint;
            return true;
        }

        void RemoveExecutionPoint()
        {
            foreach (var headReceiver in _executionPoints.ToList())
            {
                if (headReceiver.isDead)
                    _executionPoints.Remove(headReceiver);

                if (Vector3.Distance(headReceiver.transform.position, transform.position) > 5)
                    _executionPoints.Remove(headReceiver);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DetectionRange);
        }
    }
}