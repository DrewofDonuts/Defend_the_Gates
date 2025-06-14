using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class Node : MonoBehaviour, INode, IGameStateListener
    {
        [Header("Settings")]
        [Tooltip(" This determines what upgrades are available for this node.")]
        [SerializeField] UpgradeBranch<StructureObject> upgradeBranch;
        [Tooltip("Costs for each upgrade level, starting from level 0.")]
        [SerializeField] List<int> upgradeCosts = new();


        [Header("Upgrade UI References")]
        [SerializeField] Canvas upgradeCanvas;
        [SerializeField] GameObject buttonHolder;

        [Header("World Space UI References")]
        [SerializeField] Canvas worldSpaceCanvas;
        [SerializeField] TextMeshProUGUI nextUpgradeCostText;


        [Header("References")]
        [SerializeField] InputObject inputObject;
        [SerializeField] UpgradeButton upgradeButtonPrefab;


        //Update for Multiplayer
        bool isPlayerOccupyingNode;
        GameObject currentPrefab;
        public GameState CurrentGameState { get; private set; }
        PlayerNodeController playerNodeController;
        List<UpgradeButton> upgradeButtons = new();
        
        int currentUpgradeIndex ;
        int currentUpgradeLevel => currentUpgradeIndex + 1; // Level starts from 0, so index 0 is level 1

        void Start()
        {
            inputObject.SouthButtonEvent += OnSouthButtonPressed;

            worldSpaceCanvas.enabled = false;
            upgradeCanvas.enabled = false;
            RegisterListener();
        }

        void OnDisable() =>
            inputObject.SouthButtonEvent -= OnSouthButtonPressed;


        void OnSouthButtonPressed()
        {
            if (isPlayerOccupyingNode && playerNodeController != null)
            {
                if (upgradeBranch != null)
                {
                    DisplayCurrentUpgradeBranchOptions();
                }
            }
        }


        void DisplayCurrentUpgradeBranchOptions()
        {
            if (upgradeCosts.Count <= currentUpgradeIndex) return;


            foreach (var upgradeButton in upgradeButtons)
            {
                Destroy(upgradeButton.gameObject);
            }

            upgradeButtons.Clear();
            worldSpaceCanvas.enabled = false;
            upgradeCanvas.enabled = true;

            foreach (var towerObject in upgradeBranch.upgradeObjectList)
            {
                if (currentUpgradeLevel == towerObject.UpgradeData.Level)
                {
                    var button = Instantiate(upgradeButtonPrefab, buttonHolder.transform);
                    button.Initialize(towerObject.UpgradeData, this);
                    upgradeButtons.Add(button);
                }
            }
        }

        public void Upgrade(IUpgradable data)
        {
            if (currentPrefab != null)
            {
                // Destroy the current tower prefab if it exists
                Destroy(currentPrefab);
            }

            //Instantiate on network
            var newTower = Instantiate(data.Prefab, transform.position, Quaternion.identity);
            currentPrefab = newTower;
            upgradeCanvas.enabled = false;
            currentUpgradeIndex++;
        }


        public void OnGameStateChanged(GameState newGameState)
        {
            CurrentGameState = newGameState;
        }

        public void RegisterListener()
        {
            GameStateManager.Instance?.RegisterListener(this);
        }

        public void UnregisterListener()
        {
            GameStateManager.Instance?.UnregisterListener(this);
        }


        void OnTriggerEnter(Collider other)
        {
            if (CurrentGameState is GameState.BasePhase)
            {
                if (playerNodeController != null)
                {
                    // If a player is already occupying the node, do not allow another player to occupy it.
                    return;
                }

                if (other.TryGetComponent(out PlayerNodeController _baseController))
                {
                    var baseController = _baseController;
                    if (baseController != null)
                    {
                        isPlayerOccupyingNode = true;
                        playerNodeController = baseController;

                        //Display if there are upgrades available (the Count of upgradeCosts should be greater than the current index)
                        if (upgradeCosts.Count > currentUpgradeIndex)
                        {
                            nextUpgradeCostText.text = upgradeCosts[currentUpgradeIndex].ToString();
                            worldSpaceCanvas.enabled = true;
                        }
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerNodeController _playerTowerController))
            {
                if (playerNodeController != _playerTowerController)
                {
                    // If the exiting player is not the one occupying the node, do nothing.
                    return;
                }

                var baseController = _playerTowerController;
                if (baseController != null)
                {
                    upgradeCanvas.enabled = false;
                    isPlayerOccupyingNode = false;
                    playerNodeController = null;
                    worldSpaceCanvas.enabled = false;
                }
            }
        }
    }
}