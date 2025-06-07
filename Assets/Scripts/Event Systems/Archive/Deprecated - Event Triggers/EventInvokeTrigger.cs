using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class EventInvokeTrigger : MonoBehaviour
    {
        [SerializeField] List<EventKey> eventsToInvoke = new();
        [SerializeField] bool triggersCondition;

        [ShowIf("triggersCondition")]
        [SerializeField] EventKey conditionEventKey;

        //Called in Events
        public void CheckIfCanInvoke(EventKey eventKey)
        {
            foreach (var eventToInvoke in eventsToInvoke)
            {
                if (eventToInvoke == eventKey)
                {
                    InvokeEvent(eventKey);
                }
            }
        }

        void InvokeEvent(EventKey eventKey)
        {
            if (triggersCondition)
                ConditionManager.Instance.TriggerCondition(eventKey);

            if (EventManager.Instance != null)
                EventManager.Instance.TriggerEvent(eventKey);
        }
    }
}