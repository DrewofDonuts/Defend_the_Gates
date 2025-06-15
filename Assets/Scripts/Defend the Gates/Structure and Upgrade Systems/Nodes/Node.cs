using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] Image radialFillImage;


        [Header("References")]
        [SerializeField] InputObject inputObject;
        [SerializeField] UpgradeButton upgradeButtonPrefab;


        //Update for Multiplayer
        bool isPlayerOccupyingNode;
        GameObject currentPrefab;
        public GameState CurrentGameState { get; private set; }
        PlayerNodeController playerNodeController;
        List<UpgradeButton> upgradeButtons = new();

        int currentUpgradeIndex;
        int currentUpgradeLevel => currentUpgradeIndex + 1; // Level starts from 0, so index 0 is level 1

        //Network variable to prevent multiple players from accessing  the same node
        bool isMenuOpen => upgradeCanvas.enabled;

        float holdProgress;
        bool isHolding;


        void Start()
        {
            inputObject.SouthButtonEvent += OnSouthButtonPressed;
            inputObject.SouthButtonCancelEvent += OnSouthButtonPressed;

            worldSpaceCanvas.enabled = false;
            upgradeCanvas.enabled = false;
            RegisterListener();
            radialFillImage.fillAmount = 0f; // Initialize fill amount
        }

        void OnDisable()
        {
            inputObject.SouthButtonEvent -= OnSouthButtonPressed;
            inputObject.SouthButtonCancelEvent -= OnSouthButtonPressed;
        }


        void OnSouthButtonPressed()
        {
            isHolding = inputObject.IsSouthButton;
        }

        void Update()
        {
            holdProgress = UIFillUtility.UpdateRadialFill(
                radialFillImage,
                isHolding,
                holdProgress,
                2f, // Assuming 1 second to hold for the upgrade
                2f, // Reset speed
                OnComplete);
        }

        void OnComplete()
        {
            if (isPlayerOccupyingNode && playerNodeController != null)
            {
                if (upgradeBranch != null)
                {
                    playerNodeController.EnterUIState();
                    DisplayCurrentUpgradeBranchOptions();
                }
            }
        }


        void DisplayCurrentUpgradeBranchOptions()
        {
            if (upgradeCosts.Count <= currentUpgradeIndex) return;
            if (isMenuOpen) return;
            Button firstButton = null;


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

                    if (firstButton == null)
                    {
                        firstButton = button.UpgradeButtonComponent; // Set the first button to be selected
                        firstButton.Select();
                    }
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

            playerNodeController.ExitUIState();
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