using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    /*
     * OnUseTrigger is a Trigger that listens for a player's input (like pressing a button)
     * when they are within a certain range.
     */
    public class OnUseTrigger : Trigger
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] protected Color gizmosColor = Color.cyan;

        [Header("For Testing")]
        public bool playerInRange;

        public void OnUse()
        {
            if (!isConditionSatisfied) return;
            if (!isRepeatable && isTriggered) return;


            if (playerInRange)
            {
                isTriggered = true;
                SendKeyAndTriggerEvents();
            }

            if (isRepeatable)
                StartCoroutine(RestTriggerDelay());
        }

        void OnTriggerEnter(Collider other)
        {
            if (IsInTagMask(other.tag))
            {
                playerInRange = true;

                // Future logic to modify trigger behavior based on game state
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (IsInTagMask(other.tag))
            {
                playerInRange = false;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

#if UNITY_EDITOR
        [Button("Trigger Event")]
        void TriggerEvent()
        {
            OnUse();
        }


#endif
    }
}