using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Etheral
{
    public class TriggerOnDeath : MonoBehaviour
    {
        [SerializeField] GameObject takeHitObject;
        public UnityEvent onDieEvent;


        ITakeHit takeHit;

        void Awake()
        {
            if (takeHitObject != null)
                takeHit = takeHitObject.GetComponent<ITakeHit>();
            else
                takeHit = GetComponent<ITakeHit>();
        }

        void Start()
        {
            if (takeHit != null)
                takeHit.OnDie += Dead;
        }

        [Button("Trigger OnDie")]
        public void Dead()
        {
            onDieEvent?.Invoke();
        }
    }
}