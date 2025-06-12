using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class GateManager : MonoBehaviour
    {
        static GateManager instance;
        public static GateManager Instance => instance;

        public List<Gate> gates = new();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterGate(Gate gate)
        {
            if (!gates.Contains(gate))
            {
                gates.Add(gate);
            }
        }

        public void UnregisterGate(Gate gate)
        {
            if (gates.Contains(gate))
            {
                gates.Remove(gate);
            }
        }

        public List<Gate> GetGates()
        {
            return gates;
        }
    }
}