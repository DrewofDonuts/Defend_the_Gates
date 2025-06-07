using UnityEngine;

namespace Etheral
{
    public class PlayerCrusadeState : PlayerBaseState
    {
        const float TURN_SPEED = .75f;
        float sprintSpeed;

        float timerMax = 5f;
        float timer;

        float attackReadyTime = .50f;
        bool hasAttacked;
        float attackForceTime;

        bool hasEnteredLoop;
        bool isStartRunning;

        public PlayerCrusadeState(PlayerStateMachine _stateMachine) : base(_stateMachine)
        {
            sprintSpeed = stateMachine.PlayerCharacterAttributes.SprintSpeed;
        }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.GetAbility("Crusade");
            animationHandler.CrossFadeInFixedTime(characterAction.PreAnimation, CrossFadeDuration);
            stateMachine.Health.SetBlocking(true);
            stateMachine.Health.SetSturdy(true);
            PlayerComponents.GetCrusadeController().SetIfCanDamage(true);
            PlayerComponents.GetCameraHandler().SetDefenseCamera();

            StartCooldown();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.PreAnimation);

            if (!hasEnteredLoop && normalizedTime >= 1f && stateMachine.InputReader.IsRightTrigger)
            {
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName, CrossFadeDuration);
                hasEnteredLoop = true;
            }

            if (normalizedTime >= .77f && !isStartRunning && stateMachine.InputReader.IsRightTrigger)
            {
                isStartRunning = true;
            }

            if (isStartRunning && stateMachine.InputReader.IsRightTrigger)
            {
                timer += deltaTime;
                attackReadyTime -= Mathf.Max(deltaTime, 0);
                Vector3 movement = CalculateMovement();
                Move(movement * 8.5f, deltaTime);
                rotation = CalculateFacingDirection(deltaTime);
                // RotatePlayer(rotation, deltaTime);
            }
            else if (!stateMachine.InputReader.IsRightTrigger && attackReadyTime <= 0)
            {
                stateMachine.SwitchState(new PlayerCrusadeStrikeState(stateMachine));
                return;
            }
            else if (!stateMachine.InputReader.IsRightTrigger)
            {
                ReturnToLocomotion();
                return;
            }
            
            FaceCameraForward();
        }
        
        void FaceCameraForward()
        {
            Vector3 camForward = PlayerComponents.MainCameraTransform.forward;
            Vector3 camRight = PlayerComponents.MainCameraTransform.right;

            camForward.y = 0;
            camForward.Normalize();

            camRight.y = 0;
            camRight.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(camForward);

            stateMachine.transform.rotation =
                Quaternion.Slerp(stateMachine.transform.rotation, targetRot, 50f * Time.deltaTime);
        }

        public override void Exit()
        {
            PlayerComponents.GetCrusadeController().SetIfCanDamage(false);
            
        }

        Vector3 CalculateMovement()
        {
            var movement = new Vector3();

            movement += stateMachine.transform.forward * 1f;
            return movement;
        }

        Vector3 CalculateFacingDirection(float deltaTime)
        {
            var forward = stateMachine.PlayerComponents.MainCameraTransform.forward;
            var right = stateMachine.PlayerComponents.MainCameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return forward * stateMachine.InputReader.MovementValue.y +
                   right * stateMachine.InputReader.MovementValue.x;
        }

        void RotatePlayer(Vector3 movement, float deltaTime)
        {
            if (movement.magnitude < .05f) return;

            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(movement), deltaTime * TURN_SPEED);
        }
    }
}