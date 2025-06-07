using System;
using UnityEngine;


namespace Etheral
{
    public class TurretRotator : MonoBehaviour
    {
        [SerializeField] TurretLockOnController turretLockOnController;
        [SerializeField] float rotationSpeed = 10f;
        public Transform targetTransform;

        Quaternion defaultRotation;

        void Start()
        {
            if (turretLockOnController == null)
            {
                turretLockOnController = GetComponent<TurretLockOnController>();
            }

            turretLockOnController.OnTargetChanged += HandleTargetChanged;

            defaultRotation = transform.rotation;
        }

        void OnDestroy()
        {
            turretLockOnController.OnTargetChanged -= HandleTargetChanged;
        }

        void HandleTargetChanged(Transform _target)
        {
            if (_target != null)
            {
                targetTransform = _target;
            }
            else
            {
                targetTransform = null;
            }
        }


        void Update()
        {
            if (targetTransform != null)
            {
                Vector3 direction = targetTransform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // If no target, reset rotation to default
            else if (transform.rotation != defaultRotation)
            {
                transform.rotation = Quaternion.RotateTowards(defaultRotation, Quaternion.identity,
                    rotationSpeed * Time.deltaTime);
            }
        }
    }
}