using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] InputObject inputObject;

        [Header("Cinemachine Cameras")]
        [SerializeField] CinemachineTargetGroup targetGroup;
        [FormerlySerializedAs("playerCamera")] [SerializeField]
        CinemachineCamera offensiveCamera;
        [SerializeField] CinemachineCamera targetCamera;
        [SerializeField] CinemachineCamera defensiveCamera;
        [SerializeField] CinemachineCamera topDownCamera;

        [Header("Orbit Cameras")]
        [SerializeField] CinemachineOrbitalFollow playerOrbitalCamera;
        [SerializeField] CinemachineOrbitalFollow defenseOrbitalCamera;

        [Header("Camera Settings")]
        [SerializeField] float horizontalSpeed = 50;
        [SerializeField] float verticalSpeed = 50;
        [SerializeField] Vector2 verticalAxisRange = new(0, 60);


        public List<Transform> targets = new();

        CinemachineCamera currentCamera;

        WaitForSeconds waitBeforeRemovingMembers = new(2f);


        bool isRecenter;


        void Start()
        {
            EventBusPlayerController.OnAddTarget += AddMember;
            EventBusPlayerController.OnRemoveTarget += RemoveMember;
            EventBusPlayerController.OnTargetCameraEvent += SetTargetingCameraToPrimary;
            EventBusPlayerController.OnNearCameraEvent += SetThirdPersonCamera;
            EventBusPlayerController.OnFarCameraEvent += SetTopDownCamera;


            inputObject.RightStickDown += Recenter;
            inputObject.EventCanceled += CancelRecenter;

            playerOrbitalCamera.VerticalAxis.Range = verticalAxisRange;
            playerOrbitalCamera.VerticalAxis.Value = verticalAxisRange.y;
            playerOrbitalCamera.HorizontalAxis.Value = 0;
        }


        void Recenter()
        {
            isRecenter = true;
        }

        void CancelRecenter() => isRecenter = false;

        public void SetDefenseCamera()
        {
            defensiveCamera.Priority = 1;
            offensiveCamera.Priority = 0;
        }

        public void SetOffensiveCamera()
        {
            offensiveCamera.Priority = 1;
            defensiveCamera.Priority = 0;
        }

        void Update()
        {
            SyncOrbitalCameras();
        }

        void SyncOrbitalCameras()
        {
            if (playerOrbitalCamera == null || defenseOrbitalCamera == null) return;

            // Sync the radius of the defense camera with the player camera
            defenseOrbitalCamera.Radius = playerOrbitalCamera.Radius;

            // Sync the position and rotation of the defense camera with the player camera
            defenseOrbitalCamera.transform.position = playerOrbitalCamera.transform.position;
            defenseOrbitalCamera.transform.rotation = playerOrbitalCamera.transform.rotation;
        }
        


        void LateUpdate()
        {
            // // return;
            // playerOrbitalCamera.HorizontalAxis.Value += inputObject.RotationInput.x * horizontalSpeed * Time.deltaTime;
            // playerOrbitalCamera.VerticalAxis.Value += inputObject.RotationInput.y * verticalSpeed * Time.deltaTime;
            //
            //
            // //clamp vertical axis
            // playerOrbitalCamera.VerticalAxis.Value = Mathf.Clamp(playerOrbitalCamera.VerticalAxis.Value,
            //     verticalAxisRange.x, verticalAxisRange.y);
            //
            //
            // playerOrbitalCamera.HorizontalAxis.Recentering.Enabled = isRecenter;
        }


        void SetTargetingCameraToPrimary()
        {
            targetCamera.Priority = 1;
            offensiveCamera.Priority = 0;

            // nearCamera.Priority = 0;
        }

        [ContextMenu("Set Near Camera Priority")]
        void SetThirdPersonCamera()
        {
            offensiveCamera.Priority = 1;
            defensiveCamera.Priority = 0;
            StartCoroutine(LerpRadiusToNear());
            targetCamera.Priority = 0;
            topDownCamera.Priority = 0;
        }

        [ContextMenu("Set Far Camera Priority")]
        void SetTopDownCamera()
        {
            topDownCamera.Priority = 1;
            offensiveCamera.Priority = 0;
            defensiveCamera.Priority = 0;
            StartCoroutine(LerpRadiusToFar());
            targetCamera.Priority = 0;
        }

        IEnumerator LerpRadiusToFar()
        {
            float duration = 1.5f;
            float elapsedTime = 0f;
            float startValue = 4f;
            float endValue = 6f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                t = t * t * (3f - 2f * t); // Smoothstep function
                playerOrbitalCamera.Radius = Mathf.Lerp(startValue, endValue, t);
                yield return null;
            }

            playerOrbitalCamera.Radius = endValue;
        }

        IEnumerator LerpRadiusToNear()
        {
            float duration = 1.5f;
            float elapsedTime = 0f;
            float startValue = 6f;
            float endValue = 4f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                t = t * t * (3f - 2f * t); // Smoothstep function
                playerOrbitalCamera.Radius = Mathf.Lerp(startValue, endValue, t);
                yield return null;
            }

            playerOrbitalCamera.Radius = endValue;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TargetGroupObject targetGroupObject))
            {
                // if (targets.Count > 0) return;
                AddMember(targetGroupObject.Target, targetGroupObject.Weight, targetGroupObject.Radius);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out TargetGroupObject targetGroupObject))
            {
                SetTopDownCamera();
                StartCoroutine(RemoveAfterBlendEnds(targetGroupObject.Target));
            }
        }

        IEnumerator RemoveAfterBlendEnds(Transform target)
        {
            yield return waitBeforeRemovingMembers;

            Debug.Log("Removing Target");
            RemoveMember(target);
        }

        public void AddMember(Transform target, float weight, float radius)
        {
            SetTargetingCameraToPrimary();

            targetGroup.AddMember(target, weight, radius);

            if (!targets.Contains(target))
                targets.Add(target);
        }

        public void RemoveMember(Transform target)
        {
            targetGroup.RemoveMember(target);
            targets.Remove(target);
        }

        void OnDisable()
        {
            EventBusPlayerController.OnAddTarget -= AddMember;
            EventBusPlayerController.OnRemoveTarget -= RemoveMember;
            EventBusPlayerController.OnTargetCameraEvent -= SetTargetingCameraToPrimary;
            EventBusPlayerController.OnNearCameraEvent -= SetThirdPersonCamera;
            EventBusPlayerController.OnFarCameraEvent -= SetTopDownCamera;
            inputObject.RightStickDown -= Recenter;
            inputObject.EventCanceled -= CancelRecenter;
        }
    }
}