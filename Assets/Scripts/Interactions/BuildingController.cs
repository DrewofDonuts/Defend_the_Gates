using System;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [RequireComponent(typeof(BoxCollider))]
    public class BuildingController : MonoBehaviour
    {
        [SerializeField] public CameraTriggerAction onEnterCamera;
        [SerializeField] public WalkTriggerAction onEnterWalk;
        [SerializeField] public CameraTriggerAction onExitCamera = CameraTriggerAction.Nothing;
        [SerializeField] public WalkTriggerAction onExitWalk = WalkTriggerAction.Nothing;
        [SerializeField] bool singleUse;

        bool isUsed;


        void OnTriggerEnter(Collider other)
        {
            if (singleUse && isUsed) return;
            if (other.TryGetComponent(out PlayerStateMachine playerStateMachine))
            {
                if (onEnterCamera == CameraTriggerAction.NearCamera) EventBusPlayerController.NearCamera(this);
                if (onEnterCamera == CameraTriggerAction.FarCamera) EventBusPlayerController.FarCamera(this);
                if (onEnterWalk == WalkTriggerAction.Walk) EventBusPlayerController.Walk(this, true);
                if (onEnterWalk == WalkTriggerAction.Run) EventBusPlayerController.Walk(this, false);

                if (singleUse) isUsed = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (singleUse && isUsed) return;

            if (other.TryGetComponent(out PlayerStateMachine playerStateMachine))
            {
                if (onExitCamera == CameraTriggerAction.NearCamera) EventBusPlayerController.NearCamera(this);
                if (onExitCamera == CameraTriggerAction.FarCamera) EventBusPlayerController.FarCamera(this);
                if (onExitWalk == WalkTriggerAction.Walk) EventBusPlayerController.Walk(this, true);
                if (onExitWalk == WalkTriggerAction.Run) EventBusPlayerController.Walk(this, false);
                
                if (singleUse) isUsed = true;
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }


    public enum CameraTriggerAction
    {
        NearCamera,
        FarCamera,
        Nothing
    }

    public enum WalkTriggerAction
    {
        Walk,
        Run,
        Nothing
    }
}