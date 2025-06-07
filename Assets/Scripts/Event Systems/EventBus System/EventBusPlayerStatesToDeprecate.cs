using System;
using UnityEngine;

namespace Etheral
{
    public class EventBusPlayerStatesToDeprecate
    {
        public static event Action<StateType> OnPlayerChangeState;
        public static event Action OnPlayerGetUp;
        public static event Action OnPlayerAttack;
        public static event Action OnPlayerBlock;
        public static event Action OnPlayerUnblock;

        public static StateType PlayerCurrentState { get; set; }

        //Should be the new way of tracking player states. Right now it's mixed within States and StateMachine
        public static void PlayerSwitchedState(object sender, StateType stateType)
        {
            OnPlayerChangeState?.Invoke(stateType);
            PlayerCurrentState = stateType;
        }
    }
}