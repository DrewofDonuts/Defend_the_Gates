using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class EnemyGateHandler : MonoBehaviour
    {
        public List<Gate> gates = new();


        public Transform GetClosestGate()
        {
            gates = GateManager.Instance.GetGates();
            
            var closestDistance = Mathf.Infinity;
            Transform closestGate = null;

            foreach (var gate in gates)
            {
                float distance = Vector3.Distance(transform.position, gate.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGate = gate.transform;
                }
            }

            return closestGate;
        }
    }
}