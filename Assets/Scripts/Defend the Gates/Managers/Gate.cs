using System;
using UnityEngine;

namespace Etheral
{
    public class Gate : MonoBehaviour
    {
        void Start()
        {
            GateManager.Instance.RegisterGate(this);
        }


         void OnDestroy()
        {
            GateManager.Instance.UnregisterGate(this);
        }
    }
}