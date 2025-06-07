using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UltEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    //Serializable class that can be added to any class

    [Serializable]
    public class EtheralEvents
    {
        [SerializeField] string eventDescription;
        [field: InlineButton("CreateEventKey", "New Key")]
        [field: SerializeField] public EventKey EventKey { get; private set; }

        [field: BoxGroup("Unity Event Group")]
        [field: SerializeField] public UnityEvent OnUnityEvent { get; private set; }

        [field: BoxGroup("Unity Event Group")]
        [SerializeField] UltEvent OnUltEvent;

        public void UpdateEventName()
        {
            if (EventKey != null)
                eventDescription = EventKey.EventDescription;
        }

        public void TriggerEnterEvents()
        {
            OnUnityEvent?.Invoke();
            OnUltEvent?.Invoke();
        }


#if UNITY_EDITOR
        
        public void CreateEventKey()
        {
            EventKey = AssetCreator.NewEventKey();
        }

#endif
    }
}