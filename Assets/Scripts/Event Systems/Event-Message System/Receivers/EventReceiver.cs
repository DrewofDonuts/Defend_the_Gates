using System;
using UnityEngine;

namespace Etheral
{
    /*
     * EventReceiver is an abstract class that listens for events from the MessageSystemEtheral.
     * It is used to perform actions when a specific key is received.
     */
    
    public abstract class EventReceiver : MonoBehaviour
    {
        [SerializeField] string keyToReceive;

        void OnValidate()
        {
            if (keyToReceive != null)
                keyToReceive = keyToReceive.ToUpper();
        }

        void OnEnable() => EtheralMessageSystem.OnCondition += ReceiveKey;
        void OnDisable() => EtheralMessageSystem.OnCondition -= ReceiveKey;

        protected virtual void ReceiveKey(string recievingKey)
        {
            if (recievingKey == keyToReceive)
            {
                PerformActionAfterKeyIsReceived();
            }
        }

        protected abstract  void PerformActionAfterKeyIsReceived();
    }
}