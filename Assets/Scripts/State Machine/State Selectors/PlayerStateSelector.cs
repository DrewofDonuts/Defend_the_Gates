using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Etheral
{
    public class PlayerStateSelector : StateSelector
    {
        PlayerStateMachine playerStateMachine;

        void Start()
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();

            // EventBus.OnDialogueEvent += EnterDialogueState;

            EventBusPlayerController.OnPlayerStateChangeEvent += EventStateHandler;
        }


        void OnDisable()
        {
            // EventBus.OnDialogueEvent -= EnterDialogueState;

            EventBusPlayerController.OnPlayerStateChangeEvent -= EventStateHandler;
        }


        void EventStateHandler(PlayerControlTypes states, float angle)
        {
            switch (states)
            {
                case PlayerControlTypes.DialogueState:
                    EnterDialogueState();
                    break;
                case PlayerControlTypes.MovementState:
                    EnterPlayerOffensiveState();
                    break;
                case PlayerControlTypes.UIState:
                    EnterUIState();
                    break;
                case PlayerControlTypes.KnockedDownState:
                    EnterKnockedDownState(angle);
                    break;
            }
        }

        void EnterKnockedDownState(float angle)
        {
            // Calculate the opposite direction based on the angle
            float oppositeAngle = angle + 180f;
            Quaternion targetRotation = Quaternion.Euler(0, oppositeAngle, 0);

            // Apply the rotation to the player
            playerStateMachine.transform.Rotate(0, oppositeAngle, 0);

            // Switch to the knocked down state
            playerStateMachine.SwitchState(new PlayerKnockedDownState(playerStateMachine));
        }

        void EnterUIState() =>
            playerStateMachine.SwitchState(new PlayerUIState(playerStateMachine));


        public override void EnterDialogueStateWithAnimation(string animationName) { }

        public override void EnterDialogueState() =>
            playerStateMachine.SwitchState(new PlayerDialogueState(playerStateMachine, true));

        public void EnterPlayerOffensiveState() =>
            playerStateMachine.SwitchState(new PlayerOffensiveState(playerStateMachine));


        public override void EnterIdleState() { }

        public override void EnterDeadState() { }

        public override void EnterEventState(string s) { }
    }
}