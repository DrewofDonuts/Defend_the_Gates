using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    [CreateAssetMenu(fileName = "InputUtilityObject", menuName = "Etheral/Input/ Utility Object", order = 1)]
    public class InputUtilityObject : ScriptableObject, UtilityInput.IUtilityActions
    {
        UtilityInput utilityInput;

        public event Action screenCaptureEvent;
        
        void OnEnable()
        {
            if (utilityInput == null)
            {
                utilityInput = new UtilityInput();
                utilityInput.Utility.SetCallbacks(this);
            }

            utilityInput.Enable();
        
        }

        public void OnScreenCapture(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                screenCaptureEvent?.Invoke();
            }
        }
    }
}