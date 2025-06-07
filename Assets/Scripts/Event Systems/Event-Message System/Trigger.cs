using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Etheral
{
    public abstract class Trigger : MessengerClass
    {
        [FoldoutGroup("Trigger")]
        [TextArea(3, 10)]
        public string description;
        
        [FoldoutGroup("Trigger")]
        [SerializeField] protected List<string> triggerTags = new() { "Player" };

        [FoldoutGroup("Trigger")]
        [Header("Trigger Settings")]
        [SerializeField] protected bool isRepeatable;
        [FoldoutGroup("Trigger")]
        [FoldoutGroup("Trigger")]
        [ShowIf("isRepeatable")]
        [SerializeField] protected float repeatDelay = 3f;

        [FoldoutGroup("Trigger")]
        [Header("Events")]
        [SerializeField] protected UnityEvent onTriggerUnityEvent;
        



        

        public void SendKeyAndTriggerEvents()
        {
            SendKeyAndMessage();
            onTriggerUnityEvent?.Invoke();
        }
        

        public bool IsInTagMask(string tag)
        {
            if (triggerTags == null || triggerTags.Count == 0) return true;
            for (int i = 0; i < triggerTags.Count; i++)
            {
                if (string.Equals(tag, triggerTags[i])) return true;
            }

            return false;
        }

        protected IEnumerator RestTriggerDelay()
        {
            yield return new WaitForSeconds(repeatDelay);
            isTriggered = false;
        }
    }
}