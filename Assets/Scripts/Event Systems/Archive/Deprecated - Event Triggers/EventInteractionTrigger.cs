using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    public class EventInteractionTrigger : BaseEventTrigger
    {
        [SerializeField] public UnityEvent OnEventTriggered;

        void OnEnable()
        {
            var iTrigger = GetComponent<ITriggerSuccessOrFailure>();

            if (iTrigger != null)
            {
                // iTrigger.OnTriggerInteractionEvent += StartTriggerCondition;
                iTrigger.OnTriggerSuccessEvent += StartTriggerCondition;
            }
        }
        
    }
}