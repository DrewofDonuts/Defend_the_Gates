using System;
using System.Collections.Generic;
using Etheral.CharacterActions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class SimplePlayerStateMachine : MonoBehaviour
    {
        [field: SerializeField] public PlayerAttributes PlayerCharacterAttributes { get; private set; }
        [field: SerializeField] public List<CharacterActionObject> PlayerCharacterActions { get; private set; }
        [SerializeField] bool injuredStateOnStart;
        [field: SerializeField] public bool isWalking { get; private set; }

        [SerializeField] SimplePlayerComponents playerComponents;

        public SimplePlayerComponents GetPlayerComponents() => playerComponents;
        public bool hasPotion { get; private set; }


        State currentState;

        void Start()
        {
            if (injuredStateOnStart)
                SwitchState(new SimplePlayerInjuredIdleState(this));
            else
                SwitchState(new SimplePlayerIdleState(this));

            playerComponents.GetInput().PauseEvent += OnPause;
        }
        
        public void SetHasPotion(bool value)
        {
            hasPotion = value;
        }

        void OnDestroy()
        {
            playerComponents.GetInput().PauseEvent -= OnPause;
        }

        void OnPause()
        {
            GameMenu.Instance.OnPause();

            if (GameMenu.Instance.isPause)
            {
                if (!injuredStateOnStart)
                    SwitchState(new SimplePlayerUIState(this));
            }

            if (!GameMenu.Instance.isPause)
            {
                if (!injuredStateOnStart)
                    SwitchState(new SimplePlayerIdleState(this));
                else
                    SwitchState(new SimplePlayerInjuredIdleState(this));
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PickUpItem healthPotion) && !hasPotion)
            {
                if (healthPotion.Item.itemType == ItemType.Potion &&
                    Vector3.Distance(transform.position, healthPotion.transform.position) < 2f && !healthPotion.IsUsed)
                {
                    hasPotion = true;
                    healthPotion.PickUp();
                }
            }
        }


        public void SwitchState(State state)
        {
            currentState?.Exit();
            currentState = state;
            state?.Enter();
        }

        void Update()
        {
            currentState.Tick(Time.deltaTime);
        }

#if UNITY_EDITOR
        [Button("Add Atonement")]
        public void AddAtonement()
        {
            GameManager.Instance.GameData.playerAbilityAndResourceData.GatheredResources.AddXP(100);
        }
#endif
    }
}