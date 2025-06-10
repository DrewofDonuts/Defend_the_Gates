using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class Gate : MonoBehaviour
    {
        [Header("Gate References")]
        [SerializeField] List<DefensesHealth> defensesHealth;
        [SerializeField] GameObject gateVisuals;

        public event Action<Gate> OnGateDestroyed;
        public bool IsDestroyed { get; private set; }

        void Start()
        {
            foreach (var health in defensesHealth)
            {
                health.OnDie += HandleGateDestroyed;
            }

            GateManager.Instance.RegisterGate(this);
        }


        void HandleGateDestroyed()
        {
            IsDestroyed = true;
            gateVisuals.SetActive(false);
            OnGateDestroyed?.Invoke(this);
        }


        void OnDisable()
        {
            foreach (var health in defensesHealth)
            {
                if (health != null)
                {
                    health.OnDie -= HandleGateDestroyed;
                }
            }
            GateManager.Instance.UnregisterGate(this);
        }
    }
}