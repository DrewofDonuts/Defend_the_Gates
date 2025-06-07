using System;
using UnityEngine;

//Receives keys from other objects and checks if the conditions are met
//If the conditions are met, it sends the key to the MessageHandler

namespace Etheral
{
    public class SceneConditionHandler : MonoBehaviour
    {
        [SerializeField] SceneCondition[] sceneConditions;

        public void ReceiveKey(string key)
        {
            foreach (var sceneCondition in sceneConditions)
            {
                if (sceneCondition.receivedKey == key)
                {
                    sceneCondition.numberOfTimesReceived++;

                    sceneCondition.isComplete =
                        sceneCondition.numberOfTimesReceived >= sceneCondition.numberOfTimesToReceive;

                    if (sceneCondition.isComplete)
                        EtheralMessageSystem.SendKey(this, sceneCondition.keyToSend);
                }
            }
        }
    }

    [Serializable]
    public class SceneCondition
    {
        [TextArea(1, 1)]
        public string description;
        public string keyToSend;
        public string receivedKey;
        public int numberOfTimesReceived;
        public int numberOfTimesToReceive = 1;
        public bool isComplete;
    }
}