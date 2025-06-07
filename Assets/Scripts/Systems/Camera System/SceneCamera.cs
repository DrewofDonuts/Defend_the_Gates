using Unity.Cinemachine;
using UnityEngine;

namespace Etheral
{
    public class SceneCamera : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera sceneCamera;

        void Start()
        {
            EventBusPlayerController.OnFarCameraEvent += SetPriorityToZero;
            EventBusPlayerController.OnNearCameraEvent += SetPriorityToZero;
        }

        void SetPriorityToZero()
        {
            if (sceneCamera.Priority == 0) return;
            sceneCamera.Priority = 0;
        }
    }
}