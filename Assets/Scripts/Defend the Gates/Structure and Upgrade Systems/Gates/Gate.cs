using System;
using System.Collections.Generic;
using Etheral.DefendTheGates;
using UnityEngine;

namespace Etheral
{
    public class Gate : MonoBehaviour, IStructure
    {
        [Header("Gate References")]
        [SerializeField] List<DefensesHealth> defensesHealth;
        [SerializeField] List<DefenseTarget> defensesTargets;
        [SerializeField] GameObject gateVisuals;

        
        //Used to update Enemy Moving Towards Gate
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
            
            foreach (var target in defensesTargets)
            {
                target.HandleDeadTarget();
            }
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