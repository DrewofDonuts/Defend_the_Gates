using UnityEngine;

namespace Etheral
{
    public abstract class SimplePlayerBaseState : State
    {
        protected SimplePlayerStateMachine stateMachine;
        protected SimplePlayerComponents playerComponents;
        protected AnimationHandler animationHandler;
        protected CharacterAudio characterAudio;

        protected CharacterAction characterAction;
        protected bool injuredWalk;

        protected Vector3 strafeRelativeToCamera;


        protected float acceleration = 10.75f;
        protected float deceleration = 5f;
        protected float movementSpeed;
        protected float targetSpeed;
        protected float customSpeed;


        protected float forwardAmount;
        protected float turnAmount;
        protected Vector3 direction;
        protected Vector3 rotation;

        #region Animation Floats
        float targetVertical;
        float targetHorizontal;
        #endregion

        protected static readonly int ForwardHash = Animator.StringToHash("TargetingForward");
        protected static readonly int RightHash = Animator.StringToHash("TargetingRight");

        protected SimplePlayerBaseState(SimplePlayerStateMachine _stateMachine)
        {
            stateMachine = _stateMachine;
            playerComponents = stateMachine.GetPlayerComponents();
            animationHandler = playerComponents.GetAnimationHandler();
            characterAudio = playerComponents.GetCharacterAudio();
        }
        
        protected virtual void RegisterEvents()
        {
            stateMachine.GetPlayerComponents().GetInput().DRightEvent += OnDPadRightDown;
        }
        
        protected virtual void DeRegisterEvents()
        {
            stateMachine.GetPlayerComponents().GetInput().DRightEvent -= OnDPadRightDown;
        }

        void OnDPadRightDown()
        {
            Debug.Log("DPad Right Down");
            if(stateMachine.hasPotion)
            {
                Debug.Log("Should Switch to Drink Potion");
                stateMachine.SwitchState(new SimplePlayerDrinkPotionState(stateMachine));
            }
        }


        protected void HandleAllLocomotionAndAnimation(float deltaTime, bool walkOverride = false,
            bool lockOverride = false, bool rotateOverride = false)
        {
            direction = CalculateMovementAgainstCamera();
            HandleMovementSpeed(deltaTime, walkOverride);
            Move(direction * movementSpeed, deltaTime);
            ConvertDirectionalAnimation(deltaTime);
            UpdateBlendTreeParameters(deltaTime);
            HandleRotationBasedOnInputType(deltaTime, lockOverride, rotateOverride);
        }


        protected void HandleMovementSpeed(float deltaTime, bool walkOverride = false, float _momentum = 0)
        {
            if (stateMachine.isWalking || walkOverride)
                targetSpeed = injuredWalk ? customSpeed : stateMachine.PlayerCharacterAttributes.WalkSpeed;
            else
                targetSpeed = stateMachine.PlayerCharacterAttributes.RunSpeed;

            if (playerComponents.GetInput().MovementValue.magnitude > .1f)
            {
                movementSpeed = Mathf.Lerp(movementSpeed,
                    targetSpeed * playerComponents.GetInput().MovementValue.magnitude, deltaTime * acceleration);
            }
            else
            {
                movementSpeed = Mathf.Lerp(movementSpeed, 0, deltaTime * deceleration);

                if (playerComponents.GetInput().MovementValue.magnitude <= .2f)
                {
                    movementSpeed = 0;

                    // stateMachine.SwitchState(new PlayerIdleState(stateMachine));
                }
            }
        }

        #region Input Handling
        protected void HandleRotationBasedOnInputType(float deltaTime, bool rotateTowardsTarget = false,
            bool rotateWithMouseOrStick = false)
        {
            direction = CalculateMovementAgainstCamera();

            if (GetInputType() is InputType.gamePad) //using gamepad
            {
                //if not using rotate, turn towards movement direction

                FaceMovementDirection(direction, deltaTime);
            }
            else if (GetInputType() is InputType.keyboard) //using keyboard
            {
                // if (HasTarget() && !lockOverride)
                // {
                if (!rotateWithMouseOrStick)
                    FaceMovementDirection(direction, deltaTime);
                if (rotateWithMouseOrStick)
                {
                    CheckAndSetMouseAiming(true);
                }
            }
        }

        protected void CheckAndSetMouseAiming(bool isAiming)
        {
            if (GetIsMouseAiming() != isAiming)
                playerComponents.GetMouseAimController().SetIsAiming(isAiming);
        }

        protected bool GetIsMouseAiming() =>
            playerComponents.GetMouseAimController().GetIsAiming();

        protected InputType GetInputType() => playerComponents.GetPlayerInputController().GetInputType();
        #endregion


        #region Movement Methods
        //Motion is the direction the player is moving + speed of movement
        protected void Move(Vector3 desiredDirection, float deltaTime)
        {
            playerComponents.GetCC().Move(
                (desiredDirection + playerComponents.GetForceReceiver().ForcesMovement) * deltaTime);
        }


        //used to stop movement and allow ForceReceiver to apply other forces
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected Vector3 CalculateMovementAgainstCamera()
        {
            var forward = playerComponents.MainCameraTransform.forward;
            var right = playerComponents.MainCameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return forward * playerComponents.GetInput().MovementValue.y +
                   right * playerComponents.GetInput().MovementValue.x;
        }

        protected virtual void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            if (movement.magnitude < .05f) return;

            //DO NOT TOUCH THIS CODE - IT HANDLES THE ROTATION
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(movement), deltaTime * stateMachine.PlayerCharacterAttributes.RotationDamping);
        }
        #endregion

        #region Animation Handling
        protected void ConvertDirectionalAnimation(float deltaTime) //TODO should live in AnimatorController
        {
            if (GetInputType() is InputType.gamePad)
            {
                targetVertical = playerComponents.GetInput().MovementValue.y;
                targetHorizontal = playerComponents.GetInput().MovementValue.x;
            }
            else if (GetInputType() is InputType.keyboard)
            {
                targetVertical = Mathf.MoveTowards(targetVertical, playerComponents.GetInput().MovementValue.y,
                    5 * deltaTime);
                targetHorizontal = Mathf.MoveTowards(targetHorizontal, playerComponents.GetInput().MovementValue.x,
                    5 * deltaTime);
            }


            var cameraUp = playerComponents.MainCameraTransform.up * targetVertical;
            var cameraRight = playerComponents.MainCameraTransform.right * targetHorizontal;

            cameraUp.y = 0;
            cameraRight.y = 0;

            direction = (cameraRight + cameraUp);
            strafeRelativeToCamera = stateMachine.transform.InverseTransformDirection(direction);

            float localX = strafeRelativeToCamera.x;
            float localZ = strafeRelativeToCamera.z;

            if (playerComponents.GetInput().MovementValue.magnitude < .2f)
            {
                turnAmount = 0;
                forwardAmount = 0;
            }
            else
            {
                turnAmount = localX * playerComponents.GetCC().velocity.magnitude;
                forwardAmount = localZ * playerComponents.GetCC().velocity.magnitude;
            }
        }

        protected void UpdateBlendTreeParameters(float deltaTime)
        {
            if (playerComponents.GetInput().MovementValue == Vector2.zero)
            {
                //idle
                playerComponents.GetAnimationHandler().SetFloatWithDampTime(ForwardHash, 0, .2f, deltaTime);
                playerComponents.GetAnimationHandler().SetFloatWithDampTime(RightHash, 0, .2f, deltaTime);
            }
            else
            {
                playerComponents.GetAnimationHandler().SetFloatWithDampTime(ForwardHash, forwardAmount, .02f, deltaTime);
                playerComponents.GetAnimationHandler().SetFloatWithDampTime(RightHash, turnAmount, .02f, deltaTime);
            }
        }
        #endregion
    }
}