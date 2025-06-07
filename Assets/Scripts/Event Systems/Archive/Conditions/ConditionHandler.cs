using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class ConditionHandler : MonoBehaviour
    {
        [field: SerializeField] public Condition[] Conditions { get; private set; }


        IEnumerator Start()
        {
            yield return new WaitUntil(() => ConditionManager.Instance);

            ConditionManager.Instance.Register(this);
        }

        void OnDestroy()
        {
            ConditionManager.Instance.Unregister(this);
        }

        public void TriggerCondition(EventKey eventKey)
        {
            var condition = Conditions.FirstOrDefault(x => x.EventKey == eventKey);
            condition?.SetConditionToTrue();
        }

        public bool AreAllConditionsMet(EventKey eventKey)
        {
            foreach (var condition in Conditions)
            {
                if (condition.EventKey == eventKey)
                {
                    if (condition.ConditionMetElements.Any(x => !x.value))
                    {
                        return false;
                    }

                    return true;
                }

                Debug.LogError("No Condition found with that EventKey");
            }

            return default;
        }
    }
}