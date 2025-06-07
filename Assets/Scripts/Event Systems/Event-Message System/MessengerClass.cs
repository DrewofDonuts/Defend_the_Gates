using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    //KEYS SHOULD BE IN ALL CAPS AND UNIQUE FOR EACH SCENE
    //QUESTMACHINE MESSAGE FORMAT: Message, parameter

    //SENDING

    public abstract class MessengerClass : MonoBehaviour, ITrigger
    {
        [FoldoutGroup("Messenger")]
        [Header("Message Settings")]
        [Tooltip("Should represent the sender's name")]
        public string keyToSend;
        [FoldoutGroup("Messenger")]
        [Tooltip("Should represent the name receiving the message from")]
        public string keyToReceive;

        [FoldoutGroup("Messenger")]
        [Header("Quest Machine Info")]
        protected string message;
        [FoldoutGroup("Messenger")]
        protected string parameter;

        [Header("Condition Settings")]
        [FoldoutGroup("Messenger")]
        [SerializeField] protected bool ignoreCondition = true;


        public event Action OnTrigger;
        public event Action OnReceive;
        public event Action OnReset;
        public string KeyToSend => keyToSend;
        public string KeyToReceive => keyToReceive;
        [ReadOnly]
        public bool isTriggered;


        public void ResetTrigger()
        {
            isTriggered = false;
        }


        [Tooltip(
            "Becomes satisfied when receiving a key, which enables the Trigger to function as well as other logic")]
        [ReadOnly]
        [FoldoutGroup("Messenger")]
        public bool isConditionSatisfied;


        void Awake()
        {
            if (keyToSend != null)
                EtheralMessageSystem.CheckForDuplicatesAndRegister(this, keyToSend);

            isConditionSatisfied = ignoreCondition;
        }

        void OnValidate()
        {
            UpdateMethodsToFormats();
        }

        void UpdateMethodsToFormats()
        {
            if (keyToReceive != null)
                keyToReceive = keyToReceive.ToUpper();

            if (keyToSend != null)
                keyToSend = keyToSend.ToUpper();

            if (message != null)
                message = NameConverter.CapitalizeFirstLetter(message);

            if (parameter != null)
                parameter = parameter.ToLower();
        }


        protected virtual void OnEnable() => EtheralMessageSystem.OnCondition += ReceiveKey;
        protected virtual void OnDisable() => EtheralMessageSystem.OnCondition -= ReceiveKey;


        public void SendKeyAndMessage()
        {
            if (!keyToSend.IsNullOrWhitespace())
                SendKey();

            if (!message.IsNullOrWhitespace())
                SendMessage();

            TriggerEvent();
        }


        protected void SendKey() => EtheralMessageSystem.SendKey(this, keyToSend);
        protected virtual void SendMessage() => EtheralMessageSystem.SendQuestMachineMessage(this, message, parameter);


        protected void ReceiveKey(string _receivingKey)
        {
            Debug.Log($"On {gameObject.name} Key To Receive: {keyToReceive} and _receiving Key: {_receivingKey}");

            if (keyToReceive == _receivingKey)
            {
                Debug.Log($"Received Key: {gameObject.name} with key {_receivingKey}");
                HandleReceivingKey();
            }
        }

        protected virtual void HandleReceivingKey()
        {
            isConditionSatisfied = true;
            ReceiveEvent();
        }

        protected void TriggerEvent() =>
            OnTrigger?.Invoke();

        protected void ReceiveEvent() =>
            OnReceive?.Invoke();

        protected void ResetEvent() =>
            OnReset?.Invoke();
    }
}