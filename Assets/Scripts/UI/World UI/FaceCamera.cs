using UnityEngine;

namespace Etheral
{
    public class FaceCamera : MonoBehaviour
    {
        [Header("Disable/Enable")]
        public bool preventFaceCamera;

        [Header("Rotate Settings")]
        public bool rotate180;
        public bool yAxisOnly = true;

        Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (preventFaceCamera) return;
            if (mainCamera == null) return;

            if (rotate180)
            {
                if (yAxisOnly)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                        (mainCamera.transform.rotation.eulerAngles + 180f * Vector3.up).y,
                        transform.rotation.eulerAngles.z);
                }
                else
                {
                    transform.rotation =
                        Quaternion.LookRotation(-mainCamera.transform.forward, mainCamera.transform.up);
                }
            }
            else
            {
                if (yAxisOnly)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                        mainCamera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                else
                {
                    transform.rotation = mainCamera.transform.rotation;
                }
            }
        }

        // void LateUpdate()
        // {
        //     transform.LookAt(Camera.main.transform);
        //     transform.Rotate(0, 180f, 0);
        // }
    }
}