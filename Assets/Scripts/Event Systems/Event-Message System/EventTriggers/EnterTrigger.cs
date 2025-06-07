using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    /*
     * On EnterTrigger is a Trigger that listens for a player's collider entering its trigger area.
     */

    public class EnterTrigger : Trigger
    {
        [SerializeField] protected Color gizmosColor = Color.red;

        bool isPlayerNear;

        void Start()
        {
            //isConditionSatisfied will be true if no key  is needed, and false if a key is needed
            isConditionSatisfied = string.IsNullOrWhiteSpace(keyToReceive);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!isConditionSatisfied) return;
            if (!isRepeatable && isTriggered) return;
            if (isPlayerNear) return;

            if (IsInTagMask(other.tag))
            {
                isPlayerNear = true;
                isTriggered = true;
                SendKeyAndTriggerEvents();
                if (isRepeatable)
                    StartCoroutine(RestTriggerDelay());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (IsInTagMask(other.tag))
            {
                isPlayerNear = false;
                ResetEvent();
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }


#if UNITY_EDITOR
        [Button("Trigger Event")]
        void TriggerEventFromEditor()
        {
            SendKeyAndTriggerEvents();
        }


#endif
    }
}