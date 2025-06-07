using UnityEngine;

namespace Etheral
{
    public class PlayerJumpState : PlayerBaseState
    {
        readonly float jumpForce = 3f;
        readonly float airControl = .75f;
        readonly float damping = .99f;

        public PlayerJumpState(PlayerStateMachine stateMachine, float velocityFromMovement = default) : base(
            stateMachine)
        {
            if (velocityFromMovement != default)
                momentum = stateMachine.transform.forward * velocityFromMovement;
            else
                momentum = stateMachine.GetCharComponents().GetCC().velocity;
        }

        public override void Enter()
        {
            
            //Removed from Force Receiver. See code below that would be in Force Receiver
            // stateMachine.ForceReceiver.Jump(jumpForce);
            momentum.y = 0f;


            stateMachine.Animator.CrossFadeInFixedTime("Jump", .2f);
        }


        public override void Tick(float deltaTime)
        {
            var direction = CalculateMovementAgainstCamera();

            // momentum.z *= damping;
            // momentum.x *= damping;


            // Apply gravity to the velocity
            Move(momentum + direction * airControl, deltaTime);
            FaceMovementDirection(direction / 3, deltaTime);

            if (stateMachine.GetCharComponents().GetCC().velocity.y <= 0f)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
        }
    }
}

//ForceReceiver code that was removed
// public void Jump(float jumpForce)
// {
//     isFalling = true;
//     verticalVelocity += jumpForce;
// }
