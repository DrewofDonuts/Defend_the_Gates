using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace Etheral
{
    public class InputUIChange : MonoBehaviour
    {
        [Header("Icon Settings")]
        public Sprite actionIcon;
        public Image actionImageComponent;
        public float alpha = .7f;

        [Header("Input Settings")]
        public Image inputImage;
        public InputIcons inputIcons;

        // InputType inputType;
        // InputDevice inputDevice;

        void Start()
        {
            EventBusGameController.OnInputDeviceChange += OnInputEvent;
            GetInputOnStart();
        }

        void OnDisable()
        {
            EventBusGameController.OnInputDeviceChange -= OnInputEvent;
        }

        void GetInputOnStart()
        {
            OnInputEvent(InputManager.Instance.GetInputDevice());
        }


        void OnInputEvent(InputDevice device)
        {
            // Check if the device is a Keyboard or Gamepad
            if (device is Keyboard)
            {
                inputImage.sprite = inputIcons.KeyboardIcon;
            }
            else if (device is Gamepad)
            {
                if (device is XInputController)
                    inputImage.sprite = inputIcons.XboxIcon;
                else if (device is DualShockGamepad)
                    inputImage.sprite = inputIcons.PlayStationIcon;
                else
                    inputImage.sprite = inputIcons.XboxIcon;
            }
        }

#if UNITY_EDITOR
        [Button("Load Icon")]
        void LoadIcon()
        {
            if (actionIcon != null)
                actionImageComponent.sprite = actionIcon;

            if (inputIcons != null)
                inputImage.sprite = inputIcons.XboxIcon;

            if (inputImage.sprite != null)
                inputImage.color = new Color(inputImage.color.r, inputImage.color.g, inputImage.color.b, alpha);
        }
#endif
    }
}