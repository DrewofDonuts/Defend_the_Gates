using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Etheral
{
    public class HubAbilityUnlockButton : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] AbilityUnlockData abilityUnlockData;

        [Header("Ability Stuff")]
        [SerializeField] Sprite abilitySprite;
        [SerializeField] string abilityTitle;
        [SerializeField] string abilityDescription;
        [SerializeField] PlayerAbilityTypes abilityType;
        [SerializeField] Sprite unlockedAbilitySprite;


        [Header("Item References")]
        [SerializeField] Button abilityUnlockButton;
        [SerializeField] TextMeshProUGUI abilityTitleTMP;
        [SerializeField] TextMeshProUGUI abilityDescriptionTMP;
        [SerializeField] TextMeshProUGUI abilityCostTMP;
        [SerializeField] Image abilityIcon;
        [SerializeField] Image buttonImage;


        bool isUnlocked;

        void Start()
        {
            EventBusGameController.OnAbilityUnlock += AbilityWasUnlocked;
            PassAbilityDataToFields();
            abilityUnlockButton.onClick.AddListener(UnlockAbility);
            PassDataToUI();

            if (!EventSystem.current.alreadySelecting && InputManager.Instance.GetInputDevice() is Gamepad)
            {
                abilityUnlockButton.Select();

                // EventSystem.current.SetSelectedGameObject(abilityUnlockButton.gameObject);
                // Debug.Log($"Selected {abilityUnlockButton.gameObject.name}");
            }

            UpdateAndDisplayCost();

            if (GameManager.Instance.GameData.playerAbilityAndResourceData.playerStatsData.unlockedAbilities.Contains(abilityUnlockData.AbilityType))
            {
                UnlockAbility();
            }
        }

        void OnDisable()
        {
            EventBusGameController.OnAbilityUnlock -= AbilityWasUnlocked;
        }

        void AbilityWasUnlocked(AbilityUnlockData obj)
        {
            UpdateAndDisplayCost();
        }


        void UpdateAndDisplayCost()
        {
            int totalCost = abilityUnlockData.StartingAbilityCost +
                            GameManager.Instance.GameData.playerAbilityAndResourceData.GatheredResources.AddedAtonementCost;

            Debug.Log($"Total Cost: {totalCost}");

            abilityCostTMP.text = totalCost.ToString();
        }


        void UnlockAbility()
        {
            if (isUnlocked) return;

            if (GameManager.Instance.GameData.playerAbilityAndResourceData.GatheredResources.CurrentAtonement <
                abilityUnlockData.StartingAbilityCost) return;

            EventBusGameController.PlayerUnlocksAbility(this, abilityUnlockData);

            buttonImage.sprite = unlockedAbilitySprite;
            isUnlocked = true;
        }

        void PassAbilityDataToFields()
        {
            abilitySprite = abilityUnlockData.AbilitySprite;
            abilityTitle = abilityUnlockData.AbilityTitle;
            abilityDescription = abilityUnlockData.AbilityDescription;
            abilityType = abilityUnlockData.AbilityType;
        }

        void PassDataToUI()
        {
            abilityIcon.sprite = abilitySprite;
            abilityTitleTMP.text = abilityTitle;
            abilityDescriptionTMP.text = abilityDescription;

            if (isUnlocked)
            {
                buttonImage.sprite = unlockedAbilitySprite;
            }
        }


#if UNITY_EDITOR

        [Button("Set Ability Data")]
        void SetAbilityDataFromEditor()
        {
            PassAbilityDataToFields();
        }
#endif
    }
}