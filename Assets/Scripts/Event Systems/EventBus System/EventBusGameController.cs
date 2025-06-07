using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    public static class EventBusGameController
    {
        public static event Action<InputDevice> OnInputDeviceChange;
        public static event Action OnAbilitySelected;
        public static event Action<AbilityUnlockData> OnAbilityUnlock;
        public static event Action<QuestObject> OnQuestAdded;
        public static event Action<QuestObject> OnQuestCompleted;

        public static event Action OnUnloadAdditiveScene;


        public static void ChangeInputUI(object sender, InputDevice inputDevice)
        {
            OnInputDeviceChange?.Invoke(inputDevice);
        }

        public static void AbilitySelected(object sender, PlayerAbilityTypes abilityType)
        {
            OnAbilitySelected?.Invoke();
        }

        public static void PlayerUnlocksAbility(object sender, AbilityUnlockData abilityUnlockData)
        {
            OnAbilityUnlock?.Invoke(abilityUnlockData);
        }

        public static void AddQuest(object sender, QuestObject questObject)
        {
            // This is a placeholder for the event that will be triggered when a quest is added.
            // You can implement the logic to handle the quest addition here.
            OnQuestAdded?.Invoke(questObject);
        }

        public static void CompleteQuest(object sender, QuestObject questObject)
        {
            // This is a placeholder for the event that will be triggered when a quest is completed.
            // You can implement the logic to handle the quest completion here.
            OnQuestCompleted?.Invoke(questObject);
        }

        public static void UnloadAdditiveScene(object sender)
        {
            // This is a placeholder for the event that will be triggered when an additive scene is unloaded.
            // You can implement the logic to handle the scene unloading here.
            Debug.Log($"Called by: {sender}");
            OnUnloadAdditiveScene?.Invoke();
        }
    }
}