using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    [CreateAssetMenu(fileName = "InputObject", menuName = "Etheral/Input/UI Controller", order = 0)]
    public class InputUIObject : ScriptableObject, UIControler.IUIActionsActions
    {
        UIControler uiControls;
        public event Action<string> UpButtonEvent;
        public event Action<string> DownButtonEvent;
        public event Action<string> LeftButtonEvent;
        public event Action<string> RightButtonEvent;
        public event Action<string> NorthButtonEvent;
        public event Action<string> SouthButtonEvent;
        public event Action<string> EastButtonEvent;
        public event Action<string> WestButtonEvent;
        public event Action LBButtonEvent;
        public event Action RBButtonEvent;

        public event Action UpButtonCanceled;
        public event Action DownButtonCanceled;
        public event Action LeftButtonCanceled;
        public event Action RightButtonCanceled;
        public event Action NorthButtonCanceled;
        public event Action SouthButtonCanceled;
        public event Action EastButtonCanceled;
        public event Action WestButtonCanceled;
        public event Action LBButtonCanceled;
        public event Action RBButtonCanceled;

        void OnEnable()
        {
            if (uiControls == null)
            {
                uiControls = new UIControler();
                uiControls.UIActions.SetCallbacks(this);
            }

            uiControls.UIActions.Enable();
        }

        void OnDisable()
        {
            uiControls.UIActions.Disable();
        }


        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.performed)
                UpButtonEvent?.Invoke("up");
            else if (context.canceled)
                UpButtonCanceled?.Invoke();
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            if (context.performed)
                RightButtonEvent?.Invoke("right");
            else if (context.canceled)
                RightButtonCanceled?.Invoke();
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.performed)
                DownButtonEvent?.Invoke("down");
            else if (context.canceled)
                DownButtonCanceled?.Invoke();
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
                LeftButtonEvent?.Invoke("left");
            else if (context.canceled)
                LeftButtonCanceled?.Invoke();
        }

        public void OnNorth(InputAction.CallbackContext context)
        {
            if (context.performed)
                NorthButtonEvent?.Invoke("north");
            else if (context.canceled)
                NorthButtonCanceled?.Invoke();
        }

        public void OnWest(InputAction.CallbackContext context)
        {
            if (context.performed)
                WestButtonEvent?.Invoke("west");
            else if (context.canceled)
                WestButtonCanceled?.Invoke();
        }

        public void OnSouth(InputAction.CallbackContext context)
        {
            if (context.performed)
                SouthButtonEvent?.Invoke("south");
            else if (context.canceled)
                SouthButtonCanceled?.Invoke();
        }

        public void OnEast(InputAction.CallbackContext context)
        {
            if (context.performed)
                EastButtonEvent?.Invoke("east");
            else if (context.canceled)
                EastButtonCanceled?.Invoke();
        }

        public void OnLB(InputAction.CallbackContext context)
        {
            if (context.performed)
                LBButtonEvent?.Invoke();
            else if (context.canceled)
                LBButtonCanceled?.Invoke();
        }

        public void OnRB(InputAction.CallbackContext context)
        {
            if (context.performed)
                RBButtonEvent?.Invoke();
            else if (context.canceled)
                RBButtonCanceled?.Invoke();
        }
    }
}