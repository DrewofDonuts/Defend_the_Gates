using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XInput;

namespace Etheral
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] MouseAimController mouseAimController;
        InputType lastInputType;
        InputDevice inputDevice;

        public event Action<InputDevice> OnInputDeviceChange;


        void Start()
        {
            InputSystem.onEvent += OnInputEvent;
            EventBusPlayerController.OnNearCameraEvent += LockMouse;
            EventBusPlayerController.OnFarCameraEvent += UnlockMouse;
            
            LockMouse();
        }


        void OnDisable()
        {
            InputSystem.onEvent -= OnInputEvent;
            EventBusPlayerController.OnNearCameraEvent -= LockMouse;
            EventBusPlayerController.OnFarCameraEvent -= UnlockMouse;
        }

        public InputType GetInputType() => lastInputType;


        void LockMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void UnlockMouse()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        void OnInputEvent(InputEventPtr arg1, InputDevice device)
        {
            if (inputDevice == device) return;
            if (device is Mouse) return;

            OnInputDeviceChange?.Invoke(device);

            // Check if the device is a Keyboard or Gamepad
            if (device is Keyboard)
            {
                lastInputType = InputType.keyboard;

                // Cursor.lockState = CursorLockMode.None;
                // Cursor.visible = true;
                mouseAimController.SetIsMouseAndKeyboardEnabled(true);
            }
            else if (device is Gamepad)
            {
                // Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                lastInputType = InputType.gamePad;
                mouseAimController.SetIsMouseAndKeyboardEnabled(false);
            }
        }
    }
}