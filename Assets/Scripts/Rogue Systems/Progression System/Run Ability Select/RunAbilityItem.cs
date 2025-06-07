using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [RequireComponent(typeof(SphereCollider))]
    public class RunAbilityItem : MonoBehaviour
    {
        //UI 
        [SerializeField] InputObject inputObject;
        [SerializeField] List<AbilityUnlockData> abilityUnlockData;
        [SerializeField] WorldAbilityUIText worldAbilityUIText;

        [ReadOnly]
        [SerializeField] AbilityUnlockData displayedAbility;
        PlayerStatsController playerStatsController;


        [ReadOnly]
        [SerializeField] bool playerIsNear;

        void Start()
        {
            inputObject.NorthButtonEvent += NorthButtonEvent;
            EventBusGameController.OnAbilitySelected += DisableObject;
        }


        void DisableObject()
        {
            gameObject.SetActive(false);
        }

        void OnDisable()
        {
            inputObject.NorthButtonEvent -= NorthButtonEvent;
            EventBusGameController.OnAbilitySelected -= DisableObject;
        }


        void NorthButtonEvent()
        {
            if (playerIsNear)
            {
                Debug.Log("Ability Unlocked");
                playerStatsController.UpdateAbility(displayedAbility.AbilityType);
                EventBusGameController.AbilitySelected(this, displayedAbility.AbilityType);
            }
        }

        public void SetAbilityData(PlayerAbilityTypes unlockData)
        {
            displayedAbility = abilityUnlockData.Find(data => data.AbilityType == unlockData);
            PassAbilityInfoToUI();
        }

        void PassAbilityInfoToUI()
        {
            worldAbilityUIText.SetAbilityData(displayedAbility);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerStateMachine playerStateMachine))
            {
                playerStatsController = playerStateMachine.PlayerComponents.GetStatsController();
                playerIsNear = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerStateMachine playerStateMachine))
            {
                playerIsNear = false;
            }
        }
    }
}