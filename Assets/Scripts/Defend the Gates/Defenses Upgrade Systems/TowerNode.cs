using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral.Defenses
{
    //Tower node is the base class for all tower nodes in the game. 
    //This represents where the tower will be placed
    //And provides option for what kind of tower will be placed on it.
    public class TowerNode : MonoBehaviour
    {
        [Header("Tower Node Settings")]
        [SerializeField] UpgradeBranch upgradeBranch;
        [SerializeField] List<int> upgradeCosts = new List<int>();

        [Header("UI References")]
        [SerializeField] Canvas towerCanvas;
        [SerializeField] GameObject uiPanel;
        [SerializeField] GameObject buttonHolder;
        [SerializeField] UpgradeButton upgradeButtonPrefab;

        [Header("Debug")]
        public TowerObject currentTowerObject;

        int currentUpgradeLevel;
        
        List<UpgradeButton> upgradeButtons = new();


        void DisplayCurrentUpgradeBranchOptions()
        {
            upgradeButtons.Clear();
            
            foreach (var towerObject in upgradeBranch.towerDataList)
            {
                if (currentUpgradeLevel == towerObject.TowerData.Level)
                {
                    var button = Instantiate(upgradeButtonPrefab, buttonHolder.transform);
                    button.Initialize(towerObject.TowerData, this);
                    upgradeButtons.Add(button);
                }
            }
        }

        public void Upgrade(TowerData data)
        {
            //Instantiate on network
            var newTower = Instantiate(data.TowerPrefab, transform.position, Quaternion.identity);
            towerCanvas.enabled = false;
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerTowerController _baseController))
            {
                var baseController = _baseController;
                if (baseController != null)
                {
                    baseController.SetCurrentNode(this);
                    towerCanvas.enabled = true;
                    DisplayCurrentUpgradeBranchOptions();
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
                    baseController.SetCurrentNode(null);
                    towerCanvas.enabled = false;
                }
            }
        }
    }
}