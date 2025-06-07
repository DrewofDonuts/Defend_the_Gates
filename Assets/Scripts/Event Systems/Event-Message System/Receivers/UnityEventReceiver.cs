using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    public class UnityEventReceiver : EventReceiver
    {
        // This class is used to receive events from the MessageSystemEtheral

        [SerializeField] UnityEvent unityEvents;


        protected override void PerformActionAfterKeyIsReceived()
        {
            unityEvents.Invoke();
        }
    }
}