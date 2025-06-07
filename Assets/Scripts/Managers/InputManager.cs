using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;


namespace Etheral
{
    public class InputManager : MonoBehaviour, IInitialize
    {
        static InputManager _instance;
        public static InputManager Instance => _instance;

        InputDevice inputDevice;
        InputType inputType;

        public InputDevice GetInputDevice() => inputDevice;


        public void Initialize()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);

            InputSystem.onEvent += OnInputEvent;
        }

        void StartingGameLogic(bool obj)
        {
            if (true)
            {
            }
        }



        void OnDisable()
        {
            InputSystem.onEvent -= OnInputEvent;
        }

        void OnInputEvent(InputEventPtr arg1, InputDevice device)
        {
            if (inputDevice == device) return;
            if (device is Mouse) return;
            if (!Application.isPlaying) return;

            inputDevice = device;

            if (inputDevice is Keyboard)
                inputType = InputType.keyboard;
            else if (inputDevice is Gamepad)
                inputType = InputType.gamePad;

            EventBusGameController.ChangeInputUI(this, device);
        }
    }
}