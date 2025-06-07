using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    //Parent to Event Trigger components
    public abstract class BaseEventTrigger : MonoBehaviour
    {
        [field: SerializeField] public string TriggerName { get; private set; }

        // [field: BoxGroup("Who Can Trigger Event")]
        // [field: SerializeField] public CharacterKey CharacterToTriggerEvent { get; private set; }

        // [field: BoxGroup("Keys")]
        // [field: InlineButton("CreateCharacterKey", "New")]
        // [field: SerializeField] public CharacterKey CharacterKey { get; private set; }

        [field: BoxGroup("Keys")]
        [field: InlineButton("CreateEventKey", "New")]
        [field: SerializeField] public EventKey EventKey { get; private set; }

        [field: BoxGroup("Condition Configuration")]
        [field: SerializeField] public bool HasParentTriggerCondition { get; private set; }

        [field: BoxGroup("Condition Configuration")]
        [field: ShowIf("HasParentTriggerCondition")]
        [field: SerializeField] public BaseEventTrigger ParentTriggerCondition { get; private set; }
        [field: BoxGroup("Condition Configuration")]
        [field: Tooltip("If true, will trigger the condition if the condition is not met")]
        [field: SerializeField] public bool TriggersCondition { get; private set; } = true;
        [field: BoxGroup("Condition Configuration")]
        [field: SerializeField] public bool IsRepeatable { get; private set; }

        [field: BoxGroup("Condition Configuration")]
        [field: ShowIf("TriggersCondition")]
        [field: SerializeField] public EventKey ConditionEventKey { get; private set; }

        // [field: SerializeField] public int ConditionID { get; private set; } //not used yet 10/20/2023


        public bool IsTriggered { get; protected set; }
        protected float timer;

        public void CountTowardsTime()
        {
            timer += Time.deltaTime;
        }


        protected void StartTriggerCondition()
        {
            if (EventKey == null) return;

            if (!CheckCondition() && TriggersCondition)
                ConditionManager.Instance.TriggerCondition(ConditionEventKey);

            if (CheckCondition())
                TriggerEvent();
            else
                Debug.Log("Condition not met");
        }

        bool CheckCondition()
        {
            if (!HasParentTriggerCondition) return true;

            //if no previous event key condition, just check the condition
            if (EventKey.PreviousEventKeyCondition == null)
                return ConditionManager.Instance.CheckCondition(EventKey);

            //if there is a previous event key condition, check both conditions
            return ConditionManager.Instance.CheckCondition(EventKey) &&
                   ConditionManager.Instance.CheckCondition(EventKey.PreviousEventKeyCondition);
        }

        void TriggerEvent()
        {
            if (!IsRepeatable)
                IsTriggered = true;

            // if (CharacterKey != null && EventKey != null)
            //     EventManager.Instance.TriggerEventObsolete(CharacterKey, EventKey);
            // else if (CharacterKey == null && EventKey != null)
            if (EventKey != null)
                EventManager.Instance.TriggerEvent(EventKey);
        }


#if UNITY_EDITOR

        public void CreateCharacterKey()
        {
            // CharacterKey = AssetCreator.NewCharacterKey();
        }

        public void CreateEventKey()
        {
            if (EventKey == null)
                EventKey = AssetCreator.NewEventKey();
        }

#endif
    }
}