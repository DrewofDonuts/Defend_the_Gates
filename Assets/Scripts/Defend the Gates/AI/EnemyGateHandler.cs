using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class EnemyGateHandler : MonoBehaviour
    {
        public List<Gate> gates = new();
        
        public bool IsAllGatesDestroyed => gates.All(gate => gate.IsDestroyed);


        public Gate GetClosestGate()
        {
            if (gates.Count <= 0)
            {
                gates = GateManager.Instance.GetGates().FindAll(a => !a.IsDestroyed);
            }

            var closestDistance = Mathf.Infinity;
            Gate closestGate = null;

            foreach (var gate in gates)
            {
                if (gate == null || gate.IsDestroyed)
                    continue;
                float distance = Vector3.Distance(transform.position, gate.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGate = gate;
                }
            }

            return closestGate;
        }


        // public Transform GetClosestGate()
        // {
        //     gates = GateManager.Instance.GetGates();
        //     
        //     var closestDistance = Mathf.Infinity;
        //     Transform closestGate = null;
        //
        //     foreach (var gate in gates)
        //     {
        //         float distance = Vector3.Distance(transform.position, gate.transform.position);
        //         if (distance < closestDistance)
        //         {
        //             closestDistance = distance;
        //             closestGate = gate.transform;
        //         }
        //     }
        //
        //     return closestGate;
        // }
    }
}