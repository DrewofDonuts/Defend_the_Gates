using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    [CreateAssetMenu(fileName = "InputObject", menuName = "Etheral/Input/Input Object", order = 0)]
    public class InputObject : ScriptableObject, PlayerControls.IPlayerActions
    {
        PlayerControls playerControls;

        public event Action PauseEvent;
        public event Action NorthButtonEvent;
        public event Action SouthButtonEvent;
        public event Action<Vector2> RotationEvent;
        public event Action RightStickDown;
        public event Action LeftStickDownEvent;
        public event Action DialogueEvent;
        public event Action JournalEvent;
        public event Action DRightEvent;
        public event Action LeftBumperEvent;
        public event Action ModeSelectEvent;
        public event Action ModeSelectCancelEvent;


        public bool IsSouthButton { get; set; }
        public bool IsNorthButton { get; set; }
        public bool IsWaveToggle { get; set; }

        public event Action EventCanceled;

        public Vector2 RotationInput { get; private set; }
        public Vector2 MovementValue { get; private set; }
        public Vector2 MousePosition { get; private set; }


        public void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.Player.SetCallbacks(this);
            }

            playerControls.Player.Enable();
        }

        public void OnDisable()
        {
            playerControls.Player.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnMovementKeyboard(InputAction.CallbackContext context) { }

        public void OnSwitchStance(InputAction.CallbackContext context) { }

        public void OnRotate(InputAction.CallbackContext context)
        {
            RotationInput = context.ReadValue<Vector2>();

            if (RotationInput.magnitude > .5)
                RotationEvent?.Invoke(RotationInput);
        }

        public void OnAttackLight(InputAction.CallbackContext context) { }

        public void OnBlock(InputAction.CallbackContext context) { }

        public void OnEquipTwoHanded(InputAction.CallbackContext context) { }

        public void OnEquipSwordandShield(InputAction.CallbackContext context) { }

        public void OnNorthButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                NorthButtonEvent?.Invoke();
                IsNorthButton = true;
            }
        }

        public void OnEastButton(InputAction.CallbackContext context) { }

        public void OnSouthButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsSouthButton = true;
                SouthButtonEvent?.Invoke();
                DialogueEvent?.Invoke();
            }
            else if (context.canceled)
            {
                EventCanceled?.Invoke();
                IsSouthButton = false;
            }
        }

        public void OnWestButton(InputAction.CallbackContext context) { }

        public void OnDPadRight(InputAction.CallbackContext context)
        {
            if (context.performed)
                DRightEvent?.Invoke();
        }

        public void OnDPadDown(InputAction.CallbackContext context) { }

        public void OnBumperLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                LeftBumperEvent?.Invoke();
            }
        }

        public void OnBumperRight(InputAction.CallbackContext context) { }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
                PauseEvent?.Invoke();
        }

        public void OnJournal(InputAction.CallbackContext context)
        {
            if (context.performed)
                JournalEvent?.Invoke();
        }

        public void OnTestButton(InputAction.CallbackContext context) { }

        public void OnToggleLock(InputAction.CallbackContext context)
        {
            if (context.performed)
                LeftStickDownEvent?.Invoke();
            if (context.canceled)
                EventCanceled?.Invoke();
        }


        public void OnRThumbDown(InputAction.CallbackContext context)
        {
            if (context.performed)
                RightStickDown?.Invoke();
            if (context.canceled)
                EventCanceled?.Invoke();
        }

        public void OnWeaponAbility(InputAction.CallbackContext context) { }

        public void OnTowerModeTabSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsWaveToggle = true;
                ModeSelectEvent?.Invoke();
            }

            if (context.canceled)
            {
                IsWaveToggle = false;
                ModeSelectCancelEvent?.Invoke();
            }
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }
    }
}