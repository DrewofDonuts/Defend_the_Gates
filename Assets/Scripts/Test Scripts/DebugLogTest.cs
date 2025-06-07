using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class DebugLogTest : MonoBehaviour
    {
        
        [Button("Send Event to test Collisions")]
        public void SendEvent()
        {
            EventBusEnemyController.IgnorePlayerCollision(this, true);
        }
        
        public void DebugTest()
        {
            Debug.Log("Debug Log Test");
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter" + other.name);
        }
    }
}