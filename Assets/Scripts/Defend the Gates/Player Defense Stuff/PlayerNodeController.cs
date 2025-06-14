using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral.DefendTheGates
{
    public class PlayerNodeController : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
       [SerializeField] int resources = 100;
        
        // DefenseUpgrade currentUpgrade;
        // TowerNode currentNode;

        public int CurrentResources()
        {
            //future logic to reduce resources when upgrades are applied can go here
            
            return resources;
        }

        // void Start()
        // {
        //     inputObject.SouthButtonEvent += OnSouthButtonPressed;
        // }
        //
        // void OnSouthButtonPressed()
        // {
        //     if(currentUpgrade !=null )
        //         currentUpgrade.ApplyUpgrade();
        //     
        // }
        //
        // public bool CanAffordUpgrade(int cost)
        // {
        //     return startingResources >= cost;
        // }
        //
        // public void SetCurrentNode(TowerNode node)
        // {
        //     currentNode = node;
        // }
        //
        // public TowerNode GetCurrentNode()
        // {
        //     return currentNode;
        // }
        //
        // public void SetCurrentUpgrade(DefenseUpgrade upgrade)
        // {
        //     currentUpgrade = upgrade;
        // }
        //
        // public void RemoveCurrentUpgrade()
        // {
        //     currentUpgrade = null;
        // }
    }
}