using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace Etheral
{
    public class LeverTrigger : MonoBehaviour, IGetTriggered
    {
        [SerializeField] AnimationHandler animationHandler;


        public ITrigger Trigger { get; set; }
        public event Action<LeverTrigger> OnChanged;
        public bool IsActivated { get; private set; }

        void Start()
        {
            Trigger = GetComponent<ITrigger>();
            Trigger.OnTrigger += Activated;
            Trigger.OnReceive += Deactivated;
        }

        void OnDestroy()
        {
            if (Trigger != null)
            {
                Trigger.OnTrigger -= Activated;
                Trigger.OnReceive -= Deactivated;
            }
        }

        public void Deactivated()
        {
            if (!IsActivated) return; // Prevents re-triggering if already in down position
            IsActivated = false;
            Debug.Log($"Lever Triggered: {gameObject.name} is{IsActivated}");
            animationHandler.CrossFadeInFixedTime("Deactivating", 0.1f);

            Trigger.ResetTrigger();

            OnChanged?.Invoke(this);
        }

        void Activated()
        {
            if (IsActivated) return; // Prevents re-triggering if already in up position
            IsActivated = true;
            OnChanged?.Invoke(this);

            animationHandler.CrossFadeInFixedTime("Activating", 0.1f);
            Debug.Log($"Lever Triggered: {gameObject.name} is{IsActivated}");

            //Move in Upright Position
        }

        public void SetPosition(bool value) => IsActivated = value;
    }
}