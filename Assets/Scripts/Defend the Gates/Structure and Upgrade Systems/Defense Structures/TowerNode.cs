using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral.DefendTheGates
{
    //Tower node is the base class for all tower nodes in the game. 
    //This represents where the tower will be placed
    //And provides option for what kind of tower will be placed on it.
    public class TowerNode : MonoBehaviour, IGameStateListener, INode
    {
        [Header("Tower Node Settings")]
        [SerializeField] UpgradeBranch<TowerObject> upgradeBranch;
        [SerializeField] List<int> upgradeCosts = new();

        [Header("Upgrade UI References")]
        [SerializeField] Canvas upgradeCanvas;
        [SerializeField] GameObject buttonHolder;
        [SerializeField] UpgradeButton upgradeButtonPrefab;

        [Header("World Space UI References")]
        [SerializeField] Canvas worldSpaceCanvas;
        [SerializeField] TextMeshProUGUI nextUpgradeCostText;


        [Header("References")]
        [SerializeField] InputObject inputObject;

        [Header("Debug")]
        public GameObject currentTowerPrefab;

        int currentUpgradeLevel;

        List<UpgradeButton> upgradeButtons = new();

        PlayerTowerController playerTowerController;
        [field: SerializeField] public GameState CurrentGameState { get; private set; }


        //Update for Multiplayer
        bool isPlayerOccupyingNode;

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
            if (isPlayerOccupyingNode && playerTowerController != null)
            {
                if (upgradeBranch != null)
                {
                    DisplayCurrentUpgradeBranchOptions();
                }
            }
        }


        void DisplayCurrentUpgradeBranchOptions()
        {
            if (upgradeCosts.Count <= currentUpgradeLevel) return;


            foreach (var upgradeButton in upgradeButtons)
            {
                Destroy(upgradeButton.gameObject);
            }

            upgradeButtons.Clear();
            worldSpaceCanvas.enabled = false;
            upgradeCanvas.enabled = true;

            foreach (var towerObject in upgradeBranch.upgradeDataList)
            {
                if (currentUpgradeLevel == towerObject.TowerData.Level)
                {
                    var button = Instantiate(upgradeButtonPrefab, buttonHolder.transform);
                    button.Initialize(towerObject.TowerData, this);
                    upgradeButtons.Add(button);
                }
            }
        }

        public void Upgrade(IUpgradable data)
        {
            if (currentTowerPrefab != null)
            {
                // Destroy the current tower prefab if it exists
                Destroy(currentTowerPrefab);
            }

            //Instantiate on network
            var newTower = Instantiate(data.Prefab, transform.position, Quaternion.identity);
            currentTowerPrefab = newTower;
            upgradeCanvas.enabled = false;
            currentUpgradeLevel++;
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
                if (playerTowerController != null)
                {
                    // If a player is already occupying the node, do not allow another player to occupy it.
                    return;
                }

                if (other.TryGetComponent(out PlayerTowerController _baseController))
                {
                    var baseController = _baseController;
                    if (baseController != null)
                    {
                        isPlayerOccupyingNode = true;
                        playerTowerController = baseController;

                        if (upgradeCosts.Count > currentUpgradeLevel)
                        {
                            nextUpgradeCostText.text = upgradeCosts[currentUpgradeLevel].ToString();
                            worldSpaceCanvas.enabled = true;
                        }
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerTowerController _playerTowerController))
            {
                if (playerTowerController != _playerTowerController)
                {
                    // If the exiting player is not the one occupying the node, do nothing.
                    return;
                }

                var baseController = _playerTowerController;
                if (baseController != null)
                {
                    upgradeCanvas.enabled = false;
                    isPlayerOccupyingNode = false;
                    playerTowerController = null;
                    worldSpaceCanvas.enabled = false;
                }
            }
        }
    }
}