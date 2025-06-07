using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral
{
    //How do we trigger the Condition?

    [Serializable]
    public class Condition
    {
        // [field: ShowIf("TriggerEventOnComplete")]
        // [field: SerializeField] public CharacterKey CharacterKey { get; private set; }

        [field: SerializeField] public EventKey EventKey { get; private set; }

        [field: Obsolete("Use ConditionMetElements instead")]

        // [field: SerializeField] public bool[] ConditionMet { get; private set; }
        [field: InlineButton("GiveIDToConditionElements", "Give IDs")]
        [field: SerializeField] public List<BoolElement> ConditionMetElements { get; private set; } =
            new();

        [field: LabelWidth(175)]
        [field: SerializeField] public bool IsConditionComplete { get; private set; }
        [field: LabelWidth(175)]
        [field: SerializeField] public bool TriggerEventOnComplete { get; private set; }


        public void SetConditionToTrue()
        {
            if (IsConditionComplete) return;

            if (ConditionMetElements.Any(x => !x.value))
            {
                for (int i = 0; i < ConditionMetElements.Count; i++)
                {
                    if (ConditionMetElements[i].value != true)
                    {
                        ConditionMetElements[i].value = true;
                        return;
                    }
                }
            }

            // if (ConditionMetElements.All(x => x.value) && TriggerEventOnComplete)
            //     TriggerEvent();

            if (ConditionMetElements.All(x => x.value))
                IsConditionComplete = true;
        }


        void TriggerEvent()
        {
            EventManager.Instance.TriggerEvent(EventKey);
        }


        public void SetKeys(EventKey eventKey)
        {
            // CharacterKey = characterKey;
            EventKey = eventKey;
        }

#if UNITY_EDITOR
        [ShowIf("@EventKey == null")]
        [Button("Create Event Key", ButtonSizes.Medium), GUIColor(.75f, .25f, .50f)]
        public void CreateEventKey()
        {
            EventKey = AssetCreator.NewEventKey();
        }

        public void GiveIDToConditionElements()
        {
            for (int i = 0; i < ConditionMetElements.Count; i++)
            {
                ConditionMetElements[i].conditionID = i + 1;
            }
        }

#endif
    }

    [Serializable]
    public class BoolElement
    {
        public string description;
        public int conditionID;
        public bool value;
    }
}