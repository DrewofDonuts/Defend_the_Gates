using UnityEngine;

namespace Etheral
{
    public class MouseAimController : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] LayerMask aimLayerMask;
        public float rayLength = 10f;
        Vector3 lookDirection;
        public bool isAiming;
        public bool isMouseAndKeyboardEnabled;

        void LateUpdate()
        {
            if (!isMouseAndKeyboardEnabled) return;
            
            if (isAiming)
                MouseInput();
        }

        public void SetIsMouseAndKeyboardEnabled(bool value) => isMouseAndKeyboardEnabled = value;
        public void SetIsAiming(bool value) => isAiming = value;
        public bool GetIsAiming() => isAiming;

        Vector3 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(inputObject.MousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
            {
                Debug.DrawRay(Camera.main.transform.position, hitInfo.point * rayLength, Color.red);
                return hitInfo.point;
            }

            return Vector3.zero;
        }

        public void MouseInput()
        {
            Vector3 lookDirection = GetMousePosition() - transform.position;
            lookDirection.y = 0f;
            lookDirection.Normalize();

            // transform.forward = lookDirection;

            var targetRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

            //
            // Quaternion rotation = Quaternion.LookRotation(lookDirection);
            // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.fixedDeltaTime);
        }
    }
}