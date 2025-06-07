using UnityEngine;

namespace Etheral
{
    public class UIInputReader : MonoBehaviour
    {
        PlayerControls playerControls;
        protected static bool isRegistered;
        bool didIRegister;

        void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.Enable();
        }

        void OnEnable()
        {
            if (!isRegistered)
            {
                isRegistered = true;
                didIRegister = true;
                // InputDeviceManager.RegisterInputAction("Submit", playerControls.UI.Submit);
                // InputDeviceManager.RegisterInputAction("Cancel", playerControls.UI.Cancel);
            }
        }

        void OnDisable()
        {
            if (didIRegister)
            {
                playerControls.UI.Disable();
                // InputDeviceManager.UnregisterInputAction("Submit");
                // InputDeviceManager.UnregisterInputAction("Cancel");
            }
        }
    }
}