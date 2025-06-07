using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Etheral
{
    public class PlayerHolyShieldState : PlayerBaseState
    {
        bool isShieldPlaced;

        public PlayerHolyShieldState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.InputReader.CancelEvent += TriggerShield;
            stateMachine.PlayerComponents.HolyShieldController.EnableTargetter();
            StartBulletTime(.1f);

        }

        public override void Tick(float deltaTime)
        {
            if (isShieldPlaced)
                ReturnToLocomotion();
        }


        void TriggerShield()
        {
            StartCooldown();
            stateMachine.PlayerComponents.HolyShieldController.InstantiateShield();
            stateMachine.PlayerComponents.HolyShieldController.DisableTargetter();
            isShieldPlaced = true;
        }

        public override void Exit()
        {
            EndBulletTime();
            stateMachine.InputReader.CancelEvent -= TriggerShield;
        }
    }
}