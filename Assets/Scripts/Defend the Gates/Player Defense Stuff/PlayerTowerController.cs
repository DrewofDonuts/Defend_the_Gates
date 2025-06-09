using System;
using UnityEngine;

namespace Etheral.Defenses
{
    public class PlayerTowerController : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] int startingResources = 100;
        
        DefenseUpgrade currentUpgrade;
        TowerNode currentNode;

        void Start()
        {
            inputObject.SouthButtonEvent += OnSouthButtonPressed;
        }

        void OnSouthButtonPressed()
        {
            if(currentUpgrade !=null )
                currentUpgrade.ApplyUpgrade();
            
        }

        public bool CanAffordUpgrade(int cost)
        {
            return startingResources >= cost;
        }
        
        public void SetCurrentNode(TowerNode node)
        {
            currentNode = node;
        }
        
        public TowerNode GetCurrentNode()
        {
            return currentNode;
        }

        public void SetCurrentUpgrade(DefenseUpgrade upgrade)
        {
            currentUpgrade = upgrade;
        }

        public void RemoveCurrentUpgrade()
        {
            currentUpgrade = null;
        }
    }
}