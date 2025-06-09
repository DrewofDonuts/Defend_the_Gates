using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [Serializable]
    public class AITestingControl
    {
        public bool displayStateIndicator;
        public bool idleAndImpactOnly;
        public bool blockSwitchState;
        public bool blockAttack;
        public bool blockCounterAttack;
        public bool blockChase;
        public bool blockRotate;
        public bool blockMovement;
        [field: SerializeField] public int strafeDirection { get; set; }
        
    }
}