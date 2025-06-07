using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Unsure if needed 11/11/2023
namespace Etheral
{
    public class EventSystemBridge : MonoBehaviour
    {
        public UnityEvent Event;
        
        [ContextMenu("Trigger Event")]
        public void TriggerEvent()
        {
            Event.Invoke();
        }
    }
}
