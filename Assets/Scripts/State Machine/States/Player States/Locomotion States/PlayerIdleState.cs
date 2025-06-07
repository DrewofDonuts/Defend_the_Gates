using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    public class PlayerIdleState : PlayerBaseActionState
    {
        Vector3 rotation;

        public PlayerIdleState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            stateMachine.OnChangeStateMethod(StateType.Idle);
            animationHandler.CrossFadeInFixedTime(Idle, .2f);
            RegisterEvents();

            // PlayerComponents.GetMouseAimController().SetIsAiming(GetInputType() is InputType.keyboard);
        }

        public override void Tick(float deltaTime)
        {
            playerBlocks.EnterDefenseState();

            if (GetInputType() is InputType.gamePad && GetIsMouseAiming())
                RotateByMouseTopDown(false);


            if (stateMachine.InputReader.IsEastButton)
                OnEastButtonDown();


            Move(Vector3.zero, deltaTime);
            SwitchToPlayerOffensiveStateIfMoving();
        }

        void SwitchToPlayerOffensiveStateIfMoving()
        {
            if (stateMachine.InputReader.MovementValue.magnitude > 0.1f)
            {
                stateMachine.SwitchState(new PlayerOffensiveState(stateMachine));
            }
        }

        public override void Exit()
        {
            DeRegisterEvents();
        }
    }
}