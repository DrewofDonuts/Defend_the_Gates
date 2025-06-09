using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral.Defenses
{
    public abstract class DefenseUpgrade : MonoBehaviour
    {
        [SerializeField] protected int upgradeLevel = 0;
        [SerializeField] protected List<int> upgradeCosts = new();
        
        [Tooltip("The stuff that will be upgraded, like the tower model, etc.")]
        [SerializeField] protected List<GameObject> upgradeStuff;
        [SerializeField] Canvas upgradeCanvas;
        
       protected GameObject currentUpgradeStuff;


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerTowerController _baseController))
            {
                var baseController = _baseController;
                if (baseController != null)
                {
                    // baseController.SetCurrentUpgrade(this);
                    upgradeCanvas.enabled = true;
                }
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerTowerController _baseController))
            {
                var baseController = _baseController;
                if (baseController != null)
                {
                    // baseController.RemoveCurrentUpgrade();
                    upgradeCanvas.enabled = false;
                }
            }
        }
        
        public abstract void ApplyUpgrade();
    }
}