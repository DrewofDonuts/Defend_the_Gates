using TMPro;
using UnityEngine;

using UnityEngine.UI;

namespace Etheral
{
    public class HubAbilityUnlockMenuUI : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] Canvas weaponSelectionCanvas;
        [SerializeField] Button weaponSelectionExitButton;
        [SerializeField] TextMeshProUGUI currentAtonementTMP;
        [SerializeField] Button firstAbilityUnlockButton;


        GameData gameData;
        bool playerInRange;

        void Start()
        {
            inputObject.SouthButtonEvent += OnSouthButton;
            EventBusGameController.OnAbilityUnlock += UnlockAbility;
            weaponSelectionExitButton.onClick.AddListener(ExitSelectionScreen);
            weaponSelectionCanvas.gameObject.SetActive(false);
            gameData = GameManager.Instance.GameData;
        }


        void OnDisable()
        {
            inputObject.SouthButtonEvent -= OnSouthButton;
            EventBusGameController.OnAbilityUnlock -= UnlockAbility;
        }

        void OnSouthButton()
        {
            if (playerInRange && !weaponSelectionCanvas.gameObject.activeSelf)
            {
                weaponSelectionCanvas.gameObject.SetActive(true);
                EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.UIState);
                DisplayAtonement();
                firstAbilityUnlockButton.Select();
            }
        }

        void ExitSelectionScreen()
        {
            weaponSelectionCanvas.gameObject.SetActive(false);
            EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.MovementState);
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;

                //Future logic to modify weapon selection and upgrades based on game state
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
            }
        }

        #region Handling Unclocking and Upgrading Abilities
        public void UnlockAbility(AbilityUnlockData abilityUnlockButton)
        {
            PlayerAbilityTypes abilityType = abilityUnlockButton.AbilityType;

            if (!gameData.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Contains(abilityType))
            {
                gameData.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Add(abilityType);
                SpendAtonementAndAddToCost(abilityUnlockButton.StartingAbilityCost);
                DisplayAtonement();
            }
            else
                Debug.Log("Ability already unlocked");
            

        }

        void DisplayAtonement()
        {
            currentAtonementTMP.text = gameData.playerAbilityAndResourceData.GatheredResources.CurrentAtonement.ToString();
        }


        void SpendAtonementAndAddToCost(int atonementCost)
        {
            gameData.playerAbilityAndResourceData.GatheredResources.IncreaseCostAndMax(50);
            gameData.playerAbilityAndResourceData.GatheredResources.AddXP(-atonementCost);
        }
        #endregion
    }
}