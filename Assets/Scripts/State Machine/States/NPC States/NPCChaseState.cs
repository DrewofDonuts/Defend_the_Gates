using UnityEngine;

namespace Etheral
{
    public class NPCChaseState : NPCBaseState
    {
        public NPCChaseState(NPCStateMachine _stateMachine) : base(_stateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(State.Locomotion);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            Move(GetCurrentTargetPosition(), stateMachine.AIAttributes.RunSpeed, deltaTime);
            RotateTowardsTargetSmooth(stateMachine.AIAttributes.RotateSpeed);

            animationHandler.SetFloatWithDampTime(ForwardSpeed, movementSpeed);
            
            

            if (IsInMeleeRange())
            {
                Decelerate(deltaTime);

                if (movementSpeed < 2f)
                {
                    Debug.Log("Switching to attack");
                    stateMachine.SwitchState(new NPCAttackState(stateMachine));
                    return;
                }
            }
        }

        public override void Exit()
        {
        }
    }
}