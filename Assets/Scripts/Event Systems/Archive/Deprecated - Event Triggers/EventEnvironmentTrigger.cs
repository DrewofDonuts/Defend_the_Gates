using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class EventEnvironmentTrigger : BaseEventTrigger
    {
        [FormerlySerializedAs("characterKey")]
        [BoxGroup("On Trigger Settings")]
        [SerializeField] CharacterKey characterToTriggerEvent;

        [BoxGroup("On Trigger Settings")]
        [SerializeField] bool isTriggerOnEnter = true;

        [FormerlySerializedAs("IsTriggerOnExit")]
        [BoxGroup("On Trigger Settings")]
        [SerializeField] bool isTriggerOnExit;

        [BoxGroup("On Trigger Settings")]
        [SerializeField] bool isTriggerOnStay;

        [BoxGroup("Trigger Configuration")]
        [ShowIf("isTriggerOnStay")]
        [SerializeField] float timeToTrigger;

        void OnTriggerEnter(Collider other)
        {
            // if (HasParentTriggerCondition && !ParentTriggerCondition.IsTriggered) return;
            if (!isTriggerOnEnter) return;
            if (IsTriggered) return;

            if (other.TryGetComponent(out CharacterID character))
            {
                if (character.CharacterKey == characterToTriggerEvent)
                    StartTriggerCondition();
            }
        }

        void OnTriggerExit(Collider other)
        {
            // if (HasParentTriggerCondition && !ParentTriggerCondition.IsTriggered) return;
            if (!isTriggerOnExit) return;

            if (other.TryGetComponent(out CharacterID character))
                if (character.CharacterKey == characterToTriggerEvent)
                    StartTriggerCondition();
        }

        void OnTriggerStay(Collider other)
        {
            // if (HasParentTriggerCondition && !ParentTriggerCondition.IsTriggered) return;
            if (!isTriggerOnStay) return;

            if (other.TryGetComponent(out CharacterID character))
                if (character.CharacterKey == characterToTriggerEvent)
                    StartTriggerCondition();
        }
    }
}