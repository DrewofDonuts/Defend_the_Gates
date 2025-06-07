using System.Collections;
using Etheral;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    TMP_Text tmpText;
    public Image image;
    public TutorialObject tutorialObject;
    [FormerlySerializedAs("trigger")] public TriggerSuccessOrFailureMonoBehavior triggerSuccessOrFailure;
    public InputType inputType;

    void Start()
    {
        if (triggerSuccessOrFailure != null)
            triggerSuccessOrFailure.OnTriggerSuccessEvent += HideTutorialUI;


        if (!tutorialObject.text.IsNullOrWhitespace())
            tmpText = GetComponentInChildren<TMP_Text>();

        SetText();
        // inputType = InputManager.Instance.inputModel;
        
        EventBusGameController.OnInputDeviceChange+= SetTextAndController;
    }

    void SetText()
    {
        if (tutorialObject.text.IsNullOrWhitespace()) return;

        tmpText.text = tutorialObject.text;
    }

    void SetTextAndController(InputDevice device)
    {
        if (device is Keyboard)
        {
            image.sprite = tutorialObject.inputIcons.KeyboardIcon;
        }
        else if (device is Gamepad)
        {
            if (device is XInputController)
                image.sprite = tutorialObject.inputIcons.XboxIcon;
            else if (device is DualShockGamepad)
                image.sprite = tutorialObject.inputIcons.PlayStationIcon;
            else
                image.sprite = tutorialObject.inputIcons.XboxIcon;
        }
    }

    void HideTutorialUI()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (triggerSuccessOrFailure != null)
            triggerSuccessOrFailure.OnTriggerSuccessEvent -= HideTutorialUI;
    }
}