using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral.DefendTheGates
{
    public class PlayerNodeController : MonoBehaviour
    {
        [SerializeField] PlayerStateMachine playerStateMachine;
       [SerializeField] int resources = 100;
        
        // DefenseUpgrade currentUpgrade;
        // TowerNode currentNode;

        public int CurrentResources()
        {
            //future logic to reduce resources when upgrades are applied can go here
            
            return resources;
        }

        public void EnterUIState()
        {
            playerStateMachine.SwitchState(new PlayerUIState(playerStateMachine));
        }
        
        public void ExitUIState()
        {
            playerStateMachine.SwitchState(new PlayerTowerIdleState(playerStateMachine));
        }


    }
}