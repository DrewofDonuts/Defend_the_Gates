using UnityEngine;

namespace Etheral
{
    public class PlayerFallingState : PlayerBaseState
    {
        readonly int FallHash = Animator.StringToHash("Fall");

        bool hasLanded;
        Vector3 forwardMomentum;
        float airControl = 1.5f;

        float fallingDeathTimer = 1.15f;
        bool isLongFallAnimation;


        float fallingDistance;
        float fallingDeathThreshold = 7f;
        bool shouldDieOnLanding;


        const float CrossFadeDuration = 0.3f;


        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.SetIsFallingForJump(true);
            momentum =stateMachine.GetCharComponents().GetCC().velocity;
            momentum.y = 0f;

            stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
            stateMachine.InputReader.EastButtonEvent += AttemptClimb;
            stateMachine.InputReader.AttackEvent += HeightAttack;

            // stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        }

        public override void Tick(float deltaTime)
        {
            var normalizedTime = GetNormalizedTime(stateMachine.Animator, "JumpLanding");
            if (!stateMachine.ForceReceiver.IsGrounded())
            {
                var direction = CalculateMovementAgainstCamera();
                Move(momentum, deltaTime);

                // MoveWhenFalling(momentum + direction * airControl, deltaTime);
                FaceMovementDirection(direction / 2, deltaTime);
            }

            var maxNormalizedTime = ModifyNormalizedTimeBasedOnMovementVelocity();

            HandleFallingDeath(deltaTime);


            if (shouldDieOnLanding) return;
            IfGroundedPlayLandingAnimation();
            ReturnToOffensiveSteBasedOnvelocity(normalizedTime, maxNormalizedTime);

            // FaceTarget();
        }
        

        void HandleFallingDeath(float deltaTime)
        {
            if (!stateMachine.ForceReceiver.IsGrounded())
            {
                fallingDistance +=
                    Mathf.Abs(stateMachine.ForceReceiver.verticalVelocity) *
                    deltaTime; // Increment falling distance based on how fast the player is falling

                if (fallingDistance >= fallingDeathThreshold)
                {
                    Debug.Log("Long Fall");
                    stateMachine.Animator.CrossFadeInFixedTime("LongFall", 0.2f);
                    isLongFallAnimation = true;
                    shouldDieOnLanding = true;
                }
            }
            else
            {
                fallingDistance = 0f; // Reset falling distance when the player lands
            }


            if (stateMachine.ForceReceiver.IsGrounded() && shouldDieOnLanding)
            {
                Debug.Log("Dead");
                stateMachine.SwitchState(new PlayerDeadState(stateMachine));
                return;
            }
        }

        void ReturnToOffensiveSteBasedOnvelocity(float normalizedTime, float maxNormalizedTime)
        {
            if (normalizedTime >= maxNormalizedTime && hasLanded)
            {
                if (stateMachine.InputReader.MovementValue.magnitude < 0.1f)
                {
                    ReturnToLocomotion();
                }
                else
                    stateMachine.SwitchState(new PlayerOffensiveState(stateMachine, false,
                        stateMachine.GetCharComponents().GetCC().velocity.magnitude * .50f));
            }
        }

        void IfGroundedPlayLandingAnimation()
        {
            if (stateMachine.ForceReceiver.IsGrounded())
            {
                if (!hasLanded)
                    stateMachine.Animator.CrossFadeInFixedTime("JumpLanding", 0.2f);

                hasLanded = true;
            }
        }

        float ModifyNormalizedTimeBasedOnMovementVelocity()
        {
            //Will determine how long the player will take to recover after landing
            float maxNormalizedTime =
                1f - (Mathf.Abs(stateMachine.GetCharComponents().GetCC().velocity.z) +
                      Mathf.Abs(stateMachine.GetCharComponents().GetCC().velocity.x)) * .20f;
            return maxNormalizedTime;
        }

        void HeightAttack()
        {
            // if (!hasLanded && (stateMachine.GetCharComponents().GetCC().velocity.y < 0.1f &&
            //     stateMachine.ForceReceiver.GetDistanceToGround() > 2.5f))
            //     stateMachine.SwitchState(new PlayerHeightAttackState(stateMachine));
        }

        void AttemptClimb()
        {
            //TODO: APPLY TO CLIMBING LEDGES ONLY

            // var parkourAction = stateMachine.PlayerComponentHandler.ParkourController.CheckIfPossible();
            // Debug.Log(parkourAction);
            //
            // if (parkourAction.animName != null)
            // {
            //     stateMachine.SwitchState(new PlayerParkourState(stateMachine, parkourAction));
            //     return;
            // }
        }

        bool HandleFallingDeathByTime()
        {
            if (fallingDeathTimer <= 0f)
            {
                if (!isLongFallAnimation)
                {
                    Debug.Log("Long Fall");
                    stateMachine.Animator.CrossFadeInFixedTime("LongFall", 0.2f);
                    isLongFallAnimation = true;
                }
            }

            if (fallingDeathTimer <= 0 && stateMachine.ForceReceiver.IsGrounded())
            {
                stateMachine.SwitchState(new PlayerDeadState(stateMachine));
                return true;
            }

            return false;
        }

        public override void Exit()
        {
            stateMachine.ForceReceiver.SetIsFallingForJump(false);
            stateMachine.InputReader.EastButtonEvent -= AttemptClimb;

            // stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
        }

        private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
        {
            // stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
        }
    }
}