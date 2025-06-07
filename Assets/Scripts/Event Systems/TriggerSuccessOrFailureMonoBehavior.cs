using System;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    public abstract class TriggerSuccessOrFailureMonoBehavior : MonoBehaviour, ITriggerSuccessOrFailure
    {
        // public delegate void OnTriggerInteraction();
        public event Action OnTriggerSuccessEvent;
        public event Action OnTriggerFailedEvent;

        protected void OnTriggerEvent()
        {
            OnTriggerSuccessEvent?.Invoke();
        }

    }
}