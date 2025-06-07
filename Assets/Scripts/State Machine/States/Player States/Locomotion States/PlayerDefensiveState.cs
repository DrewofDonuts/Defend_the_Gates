using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace Etheral
{
    public class PlayerDefensiveState : PlayerBaseActionState
    {
        Transform strafeCamera;


        float movementSpeed;
        float dodgeSpeed = 10f;
        float remainingDodgeTime;


        // static readonly int ForwardHash = Animator.StringToHash("TargetingForward");
        // static readonly int RightHash = Animator.StringToHash("TargetingRight");
        // static readonly int IsBlocking = Animator.StringToHash("IsBlocking");
        bool ifDodge;
        Vector3 rotation;


        public PlayerDefensiveState(PlayerStateMachine stateMachine) : base(stateMachine) { }


        public override void Enter()
        {
            if (stateMachine.isThirdPerson)
                PlayerComponents.GetCameraHandler().SetDefenseCamera();
            stateMachine.stateIndicator.color = Color.blue;

            // stateMachine.Animator.SetBool(IsBlocking, stateMachine.InputReader.IsBlocking);
            stateMachine.Animator.CrossFadeInFixedTime("Defense", .05f);
            animationHandler.SetBool(IsBlocking, true);

            stateMachine.Health.SetBlocking(true);

            strafeCamera = stateMachine.PlayerComponents.MainCameraTransform;

            remainingDodgeTime = stateMachine.PlayerCharacterAttributes.DodgeDuration;

            if (CombatManager.Instance != null)
                CombatManager.Instance.SetPlayerBlocking(true);
        }

        public override void Tick(float deltaTime)
        {
            direction = DefensiveMovement();

            //Disable movement while blocking
            Move(direction * stateMachine.PlayerCharacterAttributes.StrafeSpeed,
                deltaTime);

            ConvertDirectionalAnimation(deltaTime, isStrafe: true);
            UpdateBlendTreeParameters(deltaTime);

            // stateMachine.Animator.SetFloat(ForwardHash, stateMachine.InputReader.MovementValue.y, .01f, deltaTime);
            // stateMachine.Animator.SetFloat(RightHash, stateMachine.InputReader.MovementValue.x, .01f, deltaTime);

            RotateByControllerType(deltaTime);

            //3rd Person Testing
            FaceCameraForward();

            HasTarget();


            if (stateMachine.InputReader.IsAttack ||
                stateMachine.InputReader.IsWestButton) //Update how we are attacking from Defense state
            {
                playerBlocks.EnterAttackingState();
            }


            if (!stateMachine.InputReader.IsBlocking)
            {
                animationHandler.SetBool(IsBlocking, false);
                ReturnToLocomotion();
            }
        }

        protected Vector3 DefensiveMovement()
        {
            Vector3 movement = new Vector3();

            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

            return movement;
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


        void RotateByControllerType(float deltaTime)
        {
            if (GetInputType() is InputType.gamePad) //using gamepad
            {
                //if not using rotate, turn towards movement direction
                if (stateMachine.InputReader.RotateValue.magnitude <= .1f)
                {
                    // if (!HasTarget())
                    FaceMovementDirection(direction, deltaTime);

                    // else
                    //     OnRotateTowardsTarget();
                }

                // if (stateMachine.InputReader.RotateValue.magnitude >= .1f) //If using rotate, rotate player
                // {
                //     rotation = CalcControllerRotAgainstCamera(deltaTime);
                //     RotatePlayer(rotation, deltaTime);
                // }
            }
            else if (GetInputType() is InputType.keyboard) //using keyboard
            {
                // if (HasTarget())
                // {
                //     Debug.Log("Should be Keyboard now");
                //     OnRotateTowardsTarget();
                //     CheckAndSetMouseAiming(false);
                // }
                // else
                if (!stateMachine.isThirdPerson)
                    RotateByMouseTopDown(true);
            }
        }

        public override void Exit()
        {
            if (CombatManager.Instance != null)
                CombatManager.Instance.SetPlayerBlocking(false);

            stateMachine.Animator.SetBool(IsBlocking, false);
            stateMachine.Health.SetBlocking(false);
        }
    }
}

// Vector3 CalculateFacingDirection(float deltaTime)
// {
//     var forward = stateMachine.MainCameraTransform.forward;
//     var right = stateMachine.MainCameraTransform.right;
//     forward.y = 0;
//     right.y = 0;
//     forward.Normalize();
//     right.Normalize();
//
//     return forward * stateMachine.InputReader.RotateValue.y +
//            right * stateMachine.InputReader.RotateValue.x;
// }