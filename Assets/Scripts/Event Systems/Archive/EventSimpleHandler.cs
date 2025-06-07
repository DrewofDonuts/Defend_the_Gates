using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    [RequireComponent(typeof(BoxCollider))]
    public class EventSimpleHandler : MonoBehaviour
    {
        [field: BoxGroup("Events")]
        [field: SerializeField] public UnityEvent OnEventEnter { get; private set; }
        [field: BoxGroup("Events")]
        [field: SerializeField] public UnityEvent OnEventStay { get; private set; }

        [field: BoxGroup("Events")]
        [field: SerializeField] public UnityEvent OnEventExit { get; private set; }


        [field: BoxGroup("Trigger Condition")]
        [field: SerializeField] public CharacterKey CharacterKey { get; private set; }

        [field: BoxGroup("Trigger Condition")]
        [field: SerializeField] public float TimeToTrigger { get; private set; }


        [field: BoxGroup("On Trigger Settings")]
        [field: SerializeField] public bool TriggersOnEnter { get; private set; }
        [field: BoxGroup("On Trigger Settings")]
        [field: SerializeField] public bool TriggersOnStay { get; private set; }
        [field: BoxGroup("On Trigger Settings")]
        [field: SerializeField] public bool TriggersOnExit { get; private set; }
        [field: BoxGroup("On Trigger Settings")]
        [field: SerializeField] public bool IsRepeatable { get; private set; }


        [field: SerializeField] public Timers Timers { get; private set; } = new();

        bool OnEnterIsTriggered;
        bool OnStayIsTriggered;
        bool OnExitIsTriggered;

        void Awake()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }


        void OnTriggerEnter(Collider other)
        {
            if (!TriggersOnEnter || OnEnterIsTriggered) return;
            
            if (other.TryGetComponent(out Speaker _eventHandler))
            {
                if (CharacterKey != null && _eventHandler.CharacterKey != CharacterKey) return;
            
                if (!IsRepeatable)
                    OnEnterIsTriggered = true;
            
                OnEventEnter?.Invoke();
            }
        }

        void OnTriggerStay(Collider other)
        {
            // if (!TriggersOnStay) return;
            //
            // if (other.TryGetComponent(out EventHandler _eventHandler))
            // {
            //     if (OnStayIsTriggered || _eventHandler.EntityKey != CharacterKey) return;
            //
            //     if (Timers.CountTowardsTime() >= TimeToTrigger)
            //     {
            //         OnStayIsTriggered = true;
            //         OnEventEnter?.Invoke();
            //     }
            // }
        }

        void OnTriggerExit(Collider other)
        {
            // if (!TriggersOnExit) return;
            // if (other.TryGetComponent(out EventHandler _eventHandler))
            // {
            //     if (OnExitIsTriggered || _eventHandler.EntityKey != CharacterKey) return;
            //
            //     OnEnterIsTriggered = true;
            //     OnEventExit?.Invoke();
            // }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }


    [Serializable]
    public class Timers
    {
        public float timer;

        public float CountTowardsTime()
        {
            timer += Time.deltaTime;
            return timer;
        }
    }
}