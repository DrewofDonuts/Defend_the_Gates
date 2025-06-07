using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    public class InputReader : MonoBehaviour, PlayerControls.IPlayerActions
    {
        public PlayerControls _playerControls { get; private set; }
        public event Action CancelEvent;
        public event Action DRightEvent;
        public event Action DDownEvent;
        public event Action EastButtonEvent;
        public event Action EquipHammerEvent;
        public event Action EquipSwordShieldEvent;
        public event Action LeftBumperEvent;
        public event Action OnTargetEvent;
        public event Action RightBumperEvent;
        public event Action SprintActionEvent;
        public event Action SouthButtonEvent;
        public event Action WeaponSwitchEvent;
        public event Action NorthButtonEvent;
        public event Action WestButtonEvent;
        public event Action DialogueEvent;
        public event Action JournalEvent;
        public event Action AttackEvent;
        public event Action TestButtonEvent;
        public event Action WeaponAbilityEvent;
        public event Action PauseEvent;
        public event Action OnTowerModeEvent;


        public bool IsTestButton { get; private set; }
        public bool IsAttack { get; private set; }
        public bool IsBlocking { get; private set; }
        public bool IsNorthButton { get; private set; }
        public bool IsSouthButton { get; private set; }
        public bool IsEastButton { get; private set; }
        public bool IsWestButton { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool IsLeftBumper { get; private set; }
        public bool IsRightBumper { get; set; }
        public bool IsRightTrigger { get; private set; }


        public bool CanDodge { get; private set; }

        // public bool IsDivineCharged { get; private set; }

        bool _isRbsDown;
        float timeBeforeSprint = .75f;
        float timeBeforeSprintAttack = .5f;
        float rbsTimer;

        public Vector2 MovementValue { get; private set; }
        public Vector2 RotateValue { get; private set; }
        public Vector2 MousePosition { get; private set; }

        void Awake()
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.SetCallbacks(this);
            _playerControls.Player.Enable();
        }

        void OnDestroy()
        {
            _playerControls?.Player.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnSwitchStance(InputAction.CallbackContext context) { }

        public void OnRotate(InputAction.CallbackContext context)
        {
            RotateValue = context.ReadValue<Vector2>();
        }

        public void OnAttackLight(InputAction.CallbackContext context)
        {
            if (rbsTimer > timeBeforeSprintAttack)
                SprintActionEvent?.Invoke();

            if (context.performed)
            {
                IsAttack = true;
                AttackEvent?.Invoke();
            }
            else if (context.canceled)
            {
                IsAttack = false;
                CancelEvent?.Invoke();
            }
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            if (context.performed)
                IsBlocking = true;
            else if (context.canceled)
                IsBlocking = false;
        }

        public void OnRightDPad(InputAction.CallbackContext context)
        {
            if (context.performed)
                DRightEvent?.Invoke();
        }

        public void OnDownDPad(InputAction.CallbackContext context)
        {
            if (context.performed)
                DDownEvent?.Invoke();
        }

        public void OnEquipTwoHanded(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            //Disabled until Hammer is configured
            // EquipHammerEvent?.Invoke();
            // WeaponSwitchEvent?.Invoke();
        }

        public void OnEquipSwordandShield(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            //Disabled until Hammer is configured
            // EquipSwordShieldEvent?.Invoke();
            // WeaponSwitchEvent?.Invoke();
        }

        public void OnBumperLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsLeftBumper = true;
                LeftBumperEvent?.Invoke();
            }
            else if (context.canceled)
            {
                IsLeftBumper = false;
                CancelEvent?.Invoke();
            }
        }

        public void OnBumperRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                RightBumperEvent?.Invoke();
                IsRightBumper = true;
            }
            else if (context.canceled)
                IsRightBumper = false;
        }


        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
                PauseEvent?.Invoke();
        }

        public void OnJournal(InputAction.CallbackContext context)
        {
            if (context.started)
                JournalEvent?.Invoke();
        }

        public void OnTestButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsTestButton = true;
                TestButtonEvent?.Invoke();
            }
            else if (context.canceled)
            {
                IsTestButton = false;
                CancelEvent?.Invoke();
            }
        }

        public void OnToggleLock(InputAction.CallbackContext context) { }

        public void OnRThumbDown(InputAction.CallbackContext context) { }

        public void OnWeaponAbility(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                WeaponAbilityEvent?.Invoke();
                IsRightTrigger = true;
            }
            else if (context.canceled)
            {
                CancelEvent?.Invoke();
                IsRightTrigger = false;
            }
        }

        public void OnTowerModeTabSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnTowerModeEvent?.Invoke();
            }
        }


        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnInteract(InputAction.CallbackContext context) { }

        public void OnWestButton(InputAction.CallbackContext context)
        {
            // if (!context.performed) return;
            // OnTargetEvent?.Invoke();

            if (context.performed)
            {
                IsWestButton = true;
                WestButtonEvent?.Invoke();
            }
            else if (context.canceled)
            {
                IsWestButton = false;
                CancelEvent?.Invoke();
            }
        }

        public void OnNorthButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsNorthButton = true;
                NorthButtonEvent?.Invoke();
            }
            else if (context.canceled)
            {
                IsNorthButton = false;
                CancelEvent?.Invoke();
            }
        }

        public void OnEastButton(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsEastButton = true;
                EastButtonEvent?.Invoke();
            }
            else if (context.canceled)
            {
                IsEastButton = false;
                CancelEvent?.Invoke();
            }
        }

        public void OnSouthButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isRbsDown = true;
                IsSprinting = true;
                IsSouthButton = true;

                DialogueEvent?.Invoke();

                SouthButtonEvent?.Invoke();
            }
            else if (context.canceled)
            {
                _isRbsDown = false;
                IsSprinting = false;
                if (rbsTimer < timeBeforeSprint && MovementValue.magnitude >= .85f)
                {
                    CanDodge = true;
                    SouthButtonEvent?.Invoke();
                }

                CanDodge = false;
                IsSouthButton = false;
            }
        }


        public void OnDPadRight(InputAction.CallbackContext context)
        {
            if (context.performed)
                DRightEvent?.Invoke();
        }

        public void OnDPadDown(InputAction.CallbackContext context)
        {
            if (context.performed)
                DDownEvent?.Invoke();
        }

        void Update()
        {
            CalculateRBS();
        }

        void CalculateRBS()
        {
            if (_isRbsDown)
            {
                rbsTimer += Time.deltaTime;
            }
            else
            {
                rbsTimer = 0;
            }
        }

        // public void ResetDivineCharge()
        // {
        //     // IsDivineCharged = false;
        // }
    }
}