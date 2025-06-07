using System;
using UnityEngine;

namespace Etheral
{
    public class Interactor : MonoBehaviour
    {
        OnUseTrigger currentOnUseTrigger;


        public bool TryUse()
        {
            if (currentOnUseTrigger == null) return false;


            currentOnUseTrigger.OnUse();
            return true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<OnUseTrigger>(out var onUseTrigger))
            {
                if (currentOnUseTrigger != null) return;
                currentOnUseTrigger = onUseTrigger;
                currentOnUseTrigger.playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<OnUseTrigger>(out var onUseTrigger))
            {
                if (currentOnUseTrigger == onUseTrigger)
                {
                    currentOnUseTrigger.playerInRange = false;
                    currentOnUseTrigger = null;
                }
            }
        }
    }
}