using UnityEngine;

namespace Etheral
{
    /*
     * This class is used to trigger events when a specific condition is met.
     */
    public class EventOnlyTrigger : Trigger
    {
        [SerializeField] bool sendKeyAfterReceivingKey;

        protected override void HandleReceivingKey()
        {
            base.HandleReceivingKey();


            if (sendKeyAfterReceivingKey)
                SendKeyAndTriggerEvents();
        }
    }
}