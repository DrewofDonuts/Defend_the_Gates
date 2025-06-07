using System;
using UnityEngine;

namespace Etheral
{
    public class SimplePlayerStateSelector : MonoBehaviour
    {
        [SerializeField] SimplePlayerStateMachine stateMachine;


        void Start()
        {
            EventBusPlayerController.OnPlayerStateChangeEvent += EventStateHandler;
        }

        void EventStateHandler(PlayerControlTypes state, float arg2)
        {
            switch (state)
            {
                case PlayerControlTypes.MovementState:
                    stateMachine.SwitchState(new SimplePlayerIdleState(stateMachine));
                    break;
                case PlayerControlTypes.UIState:
                    stateMachine.SwitchState(new SimplePlayerUIState(stateMachine));
                    break;
            }
        }

        void OnDisable()
        {
            EventBusPlayerController.OnPlayerStateChangeEvent -= EventStateHandler;
        }
    }
}