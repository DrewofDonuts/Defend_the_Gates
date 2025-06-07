using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using DialogueSystemTrigger = PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger;

namespace Etheral
{
    public class AIDialogueSystemController : DialogueSystemController
    {
        [SerializeField] PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger systemTrigger;


        [Button("Enable System Trigger")]
        public void EnableSystemTrigger()
        {
            systemTrigger.trigger = DialogueSystemTriggerEvent.OnUse;
        }

        [Button("Disable System Trigger")]
        public void DisableSystemTrigger()
        {
            systemTrigger.trigger = DialogueSystemTriggerEvent.None;
        }
    }
}