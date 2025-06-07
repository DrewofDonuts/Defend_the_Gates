using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class ConditionManager : MonoBehaviour
    {
        static ConditionManager _instance;
        public static ConditionManager Instance => _instance;

        ConditionHandler conditionHandler;

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void Register<T>(T register)
        {
            if (typeof(T) == typeof(ConditionHandler))
            {
                conditionHandler = register as ConditionHandler;
            }
        }

        public void Unregister<T>(T unregister)
        {
            if (typeof(T) == typeof(ConditionHandler))
            {
                conditionHandler = unregister as ConditionHandler;
            }
        }

        public void TriggerCondition(EventKey eventKey)
        {
            if (conditionHandler.Conditions.Any(x => x.EventKey == eventKey))
            {
                conditionHandler.TriggerCondition(eventKey);
            }
        }

        public bool CheckCondition(EventKey eventKey)
        {
            if (conditionHandler.Conditions.Any(x => x.EventKey == eventKey))
            {
                return conditionHandler.AreAllConditionsMet(eventKey);
            }

            Debug.Log("No Condition found with that EventKey, returning true");

            return true;
        }
    }
}