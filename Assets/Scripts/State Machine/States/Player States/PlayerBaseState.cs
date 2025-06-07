using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Etheral
{
    public abstract class PlayerBaseState : State
    {
        const float AttackRotationSpeed = 8f;

        public PlayerStateMachine stateMachine { get; private set; }
        protected PlayerComponents PlayerComponents { get; private set; }
        protected PlayerStateBlocks playerBlocks;
        protected ActionProcessor actionProcessor;
        protected PlayerStatsController playerStatsController;

        protected Vector3 strafeRelativeToCamera;

        protected float forwardAmount;
        protected float turnAmount;
        protected Vector3 direction;
        protected Vector3 rotation;

        protected float acceleration = 10.75f;
        protected float deceleration = 5f;
        protected float movementSpeed;
        protected float targetSpeed;
        protected bool isIdle;

        #region Animation Floats
        float targetVertical;
        float targetHorizontal;
        #endregion


        //Was using only for free jump and height attacks. Can remove momentum if those actions are removed
        protected Vector3 momentum;
        protected ClimbAndParkourData climbAndParkour;
        protected AvatarTarget matchBodyPart;
        readonly BulletimeHandler bulletimeHandler;

        protected static readonly int ForwardHash = Animator.StringToHash("TargetingForward");
        protected static readonly int RightHash = Animator.StringToHash("TargetingRight");
        protected static readonly int IsBlocking = Animator.StringToHash("IsBlocking");

        // protected AnimationHandler animationHandler;

        const float DISTANCE_BEFORE_APPLYING_FORCE = 1.25f;


        //constructor that passes in the state machine reference
        public PlayerBaseState(PlayerStateMachine _stateMachine)
        {
            stateMachine = _stateMachine;
            PlayerComponents = stateMachine.PlayerComponents;
            PlayerComponents.GetMouseAimController().SetIsAiming(false);
            playerStatsController = PlayerComponents.GetStatsController();
            playerBlocks = new PlayerStateBlocks(stateMachine, playerStatsController);


            //Should handle in PlayerInputController
            actionProcessor = new ActionProcessor();

            actionProcessor.ClearAll();

            // momentum = stateMachine.ForceReceiver.GetMotion();
            bulletimeHandler = new BulletimeHandler(this);

            animationHandler = stateMachine.PlayerComponents.GetAnimationHandler();
        }

        protected void HandleAllLocomotionAndAnimation(float deltaTime, bool walkOverride = false,
            bool lockOverride = false, bool rotateOverride = false, bool isStrafe = false)
        {
            direction = CalculateMovementAgainstCamera();
            HandleMovementSpeed(deltaTime, walkOverride);
            Move(direction * movementSpeed, deltaTime);
            ConvertDirectionalAnimation(deltaTime, isStrafe);
            UpdateBlendTreeParameters(deltaTime);

            HandleRotationBasedOnInputType(deltaTime, lockOverride, rotateOverride);
        }


        #region Movement Methods
        protected void HandleMovementSpeed(float deltaTime, bool walkOverride = false, float _momentum = 0)
        {
            if (stateMachine.isWalking || walkOverride)
            {
                targetSpeed = stateMachine.PlayerCharacterAttributes.WalkSpeed;
            }
            else
            {
                //moving
                targetSpeed = stateMachine.InputReader.IsSprinting
                    ? stateMachine.PlayerCharacterAttributes.SprintSpeed
                    : playerStatsController.GetMovementSpeed();
            }

            if (stateMachine.InputReader.MovementValue.magnitude > .1f)
            {
                stateMachine.OnChangeStateMethod(StateType.Move);

                movementSpeed = Mathf.Lerp(movementSpeed,
                    targetSpeed * stateMachine.InputReader.MovementValue.magnitude, deltaTime * acceleration);

                //If bullet time, compensate movement speed to move normally during bullet time
                movementSpeed = stateMachine.IsBulletTime ? movementSpeed / Time.timeScale : movementSpeed;
            }
            else
            {
                movementSpeed = Mathf.Lerp(movementSpeed, 0, deltaTime * deceleration);

                if (stateMachine.InputReader.MovementValue.magnitude <= .2f)
                {
                    movementSpeed = 0;

                    // stateMachine.SwitchState(new PlayerIdleState(stateMachine));
                }
            }
        }

        protected void HandleAnimationSwitchForAnimationLayersWithOverride(float deltaTime, string idle = "Idle",
            string move = "FreeLookBlendTree")
        {
            isIdle = movementSpeed <= 0;

            if (animationHandler.GetAnimationLayerWeight(1) < 1f)
            {
                animationHandler.SetAnimatorLayer(1,
                    Mathf.MoveTowards(animationHandler.GetAnimationLayerWeight(1), 1, 10 * deltaTime));
            }

            if (isIdle && !animationHandler.GetIfCurrentOrNextState(idle, 0))
                animationHandler.CrossFadeInFixedTime(idle);

            if (!isIdle && !animationHandler.GetIfCurrentOrNextState(move, 0))
                animationHandler.CrossFadeInFixedTime(move);
        }


        //Motion is the direction the player is moving + speed of movement
        protected void Move(Vector3 desiredDirection, float deltaTime)
        {
            var finalDirection = desiredDirection;

            // Check for ledge
            if (desiredDirection.magnitude > 0.1f &&
                stateMachine.ForceReceiver.IsLedgeAhead(desiredDirection, .6f, 2f, 2.1f))
            {
                finalDirection = Vector3.zero;
            }

            stateMachine.PlayerComponents.GetCC().Move(
                (finalDirection + stateMachine.ForceReceiver.ForcesMovement) * deltaTime);
        }

        // protected void Move(Vector3 desiredDirection, float deltaTime)
        // {
        //     stateMachine.GetCharComponents().GetCC().Move(
        //         (desiredDirection + stateMachine.ForceReceiver.ForcesMovement) * deltaTime);
        // }

        protected void MoveWhenFalling(Vector3 movement, float deltaTime)
        {
            stateMachine.GetCharComponents().GetCC()
                .Move((movement + stateMachine.ForceReceiver.ForcesMovement) * deltaTime);
        }


        //used to stop movement and allow ForceReceiver to apply other forces
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected Vector3 CalcControllerRotAgainstCamera(float deltaTime)
        {
            var forward = stateMachine.PlayerComponents.MainCameraTransform.forward;
            var right = stateMachine.PlayerComponents.MainCameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            return forward * stateMachine.InputReader.RotateValue.y +
                   right * stateMachine.InputReader.RotateValue.x;
        }


        protected Vector3 CalculateMovementAgainstCamera()
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
        #endregion

        #region Rotation
        protected void HandleRotationBasedOnInputType(float deltaTime, bool rotateTowardsTarget = false,
            bool rotateWithMouseOrStick = false)
        {
            direction = CalculateMovementAgainstCamera();

            if (GetInputType() is InputType.gamePad) //using gamepad
            {
                //if not using rotate, turn towards movement direction

                if (!HasTarget() || !rotateTowardsTarget)
                    FaceMovementDirection(direction, deltaTime);

                if (rotateTowardsTarget)
                    OnRotateTowardsTarget();


                //If using rotate, rotate player
                // if (stateMachine.InputReader.RotateValue.magnitude >= .1f && rotateWithMouseOrStick)
                // {
                //     rotation = CalcControllerRotAgainstCamera(deltaTime);
                //     RotatePlayer(rotation, deltaTime);
                // }
            }
            else if (GetInputType() is InputType.keyboard) //using keyboard
            {
                // if (HasTarget() && !lockOverride)
                // {
                if (!rotateWithMouseOrStick)
                    FaceMovementDirection(direction, deltaTime);
                if (rotateWithMouseOrStick)
                {
                    if (!rotateTowardsTarget && !stateMachine.isThirdPerson)
                        RotateByMouseTopDown(true);

                    if (rotateTowardsTarget && !HasTarget() && !stateMachine.isThirdPerson)
                        RotateByMouseTopDown(true);
                }

                if (rotateTowardsTarget && HasTarget())
                    OnRotateTowardsTarget();


                // OnRotateTowardsTarget();
                //     CheckAndSetMouseAiming(false);
                // }
                // else
                //     CheckAndSetMouseAiming(true);
            }
        }

        protected void RotateByMouseTopDown(bool isAiming)
        {
            if (GetIsMouseAiming() != isAiming)
                stateMachine.PlayerComponents.GetMouseAimController().SetIsAiming(isAiming);
        }

        protected bool GetIsMouseAiming() =>
            stateMachine.PlayerComponents.GetMouseAimController().GetIsAiming();

        protected InputType GetInputType() => PlayerComponents.GetPlayerInputController().GetInputType();


        protected virtual void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            if (movement.magnitude < .05f) return;

            //DO NOT TOUCH THIS CODE - IT HANDLES THE ROTATION
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(movement), deltaTime * stateMachine.PlayerCharacterAttributes.RotationDamping);
        }


        protected void RotateTowardsTargetSnap()
        {
            Vector3 lookPos = CalculateMovementAgainstCamera();
            lookPos.y = 0f;

            if (stateMachine.InputReader.MovementValue.magnitude > .1f)
                stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }
        #endregion


        #region Animation Handling
        protected void
            ConvertDirectionalAnimation(float deltaTime, bool isStrafe) //TODO should live in AnimatorController
        {
            if (GetInputType() is InputType.gamePad)
            {
                targetVertical = stateMachine.InputReader.MovementValue.y;
                targetHorizontal = stateMachine.InputReader.MovementValue.x;
            }
            else if (GetInputType() is InputType.keyboard)
            {
                targetVertical = Mathf.MoveTowards(targetVertical, stateMachine.InputReader.MovementValue.y,
                    5 * deltaTime);
                targetHorizontal = Mathf.MoveTowards(targetHorizontal, stateMachine.InputReader.MovementValue.x,
                    5 * deltaTime);
            }


            var cameraUp = stateMachine.PlayerComponents.MainCameraTransform.up * targetVertical;
            var cameraRight = stateMachine.PlayerComponents.MainCameraTransform.right * targetHorizontal;

            cameraUp.y = 0;
            cameraRight.y = 0;

            direction = (cameraRight + cameraUp);
            strafeRelativeToCamera = stateMachine.transform.InverseTransformDirection(direction);


            if (stateMachine.InputReader.MovementValue.magnitude < .2f)
            {
                turnAmount = 0;
                forwardAmount = 0;
            }
            else
            {
                //if not strafing, use the velocity of the character controller
                if (!isStrafe)
                {
                    forwardAmount = stateMachine.PlayerComponents.GetCC().velocity.magnitude;
                }
                else
                {
                    //these localValues are what affects the blend tree due relativity to camera
                    float localX = strafeRelativeToCamera.x;
                    float localZ = strafeRelativeToCamera.z;
                    turnAmount = localX * stateMachine.PlayerComponents.GetCC().velocity.magnitude;
                    forwardAmount = localZ * stateMachine.PlayerComponents.GetCC().velocity.magnitude;
                }
            }
        }

        protected void UpdateBlendTreeParameters(float deltaTime)
        {
            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                //idle
                stateMachine.Animator.SetFloat(ForwardHash, 0, AnimatorDampTime, deltaTime);
                stateMachine.Animator.SetFloat(RightHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                stateMachine.Animator.SetFloat(ForwardHash, forwardAmount, .01f, deltaTime);
                stateMachine.Animator.SetFloat(RightHash, turnAmount, .01f, deltaTime);
            }
        }
        #endregion


        //Move to PlayerBlocks
        protected void ReturnToLocomotion(bool zeroMomentum = false, float receivedMomentum = default)
        {
            playerBlocks.EquipMeleeWeapons();

            if (stateMachine.InputReader.IsBlocking)
                stateMachine.SwitchState(new PlayerDefensiveState(stateMachine));
            else
                stateMachine.SwitchState(new PlayerOffensiveState(stateMachine, zeroMomentum, receivedMomentum));

            //TODO else CurrentTarget !=null {PlayerTargeting state}
            //Defensive state (no lock on)
        }


        #region Bullet Time
        protected void StartBulletTime(float bulletTime = .2f, float timeAtBulletTime = .5f)
        {
            bulletimeHandler.StartBulletTime(bulletTime, timeAtBulletTime);
        }

        protected void EndBulletTime()
        {
            bulletimeHandler.EndBulletTime();
        }
        #endregion


        protected CharacterAction GetCharacterAction(CharacterAction _characterAction)
        {
            return _characterAction;
        }

        protected void StartCooldown()
        {
            if (playerBlocks.IsCooldownManagerActive())
                CooldownManager.Instance.StartCooldown(characterAction);
        }

        #region TARGETING METHODS
        protected bool HasTarget()
        {
            //if there are no targets, return
            return stateMachine.PlayerComponents.LockOnController.SelectTarget();

            // RotateTowardsTarget();
        }

        protected bool IsLockEnabled() => stateMachine.PlayerComponents.LockOnController.IsLockEnabled();

        protected void OnRotateTowardsTarget()
        {
            //if there are no targets, return
            if (!stateMachine.PlayerComponents.LockOnController.SelectTarget()) return;
            RotateTowardsTarget();
        }


        protected void RotateTowardsTarget(float rotateSpeed = AttackRotationSpeed)
        {
            if (stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget == null) return;
            if (Vector3.Distance(stateMachine.transform.position,
                    stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget.Transform.position) >
                4) return;

            var lookPos = stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget.Transform.position -
                          stateMachine.transform.position;
            lookPos.y = 0f;

            var targetRotation = Quaternion.LookRotation(lookPos);

            // stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
            stateMachine.transform.rotation =
                Quaternion.Lerp(stateMachine.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        protected void RotateTowardsExecutionPoint(Vector3 position, float rotateSpeed = AttackRotationSpeed)
        {
            if (stateMachine.PlayerComponents.GroundExecutionPointDetector.CurrentHeadExecutionPoint ==
                null) return;

            var lookPos = position - stateMachine.transform.position;
            lookPos.y = 0f;

            var targetRotation = Quaternion.LookRotation(lookPos);

            stateMachine.transform.rotation =
                Quaternion.Lerp(stateMachine.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }

        protected bool CheckIfAdjacentToTarget(float distance)
        {
            if (stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget == null) return false;

            Vector3 directionToTarget =
                PlayerComponents.LockOnController.CurrentEnemyTarget.Transform.position -
                stateMachine.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget >= distance) return false;

            directionToTarget.Normalize();
            float dotProduct = Vector3.Dot(stateMachine.transform.forward, directionToTarget);

            return dotProduct > 0;
        }

        // protected bool CheckIfAdjacentToTarget(float distance)
        // {
        //     if (stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget == null) return false;
        //     
        //     
        //
        //     return Vector3.Distance(stateMachine.transform.position,
        //                stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget.Transform.position) <
        //            distance;
        // }
        #endregion

        #region Weapons Methods
        protected void PassRightBaseDamage(CharacterAction characterAction)
        {
            stateMachine.WeaponHandler._currentRightHandDamage.SettAttackStatDamage(characterAction.Damage,
                characterAction.KnockBackForce,
                characterAction.KnockDownForce, characterAction.IsShieldBreak, characterAction.FeedbackType);
        }

        protected void PassLeftBaseDamage(CharacterAction characterAction)
        {
            stateMachine.WeaponHandler._currentLeftHandDamage.SettAttackStatDamage(characterAction.Damage,
                characterAction.KnockBackForce,
                characterAction.KnockDownForce, characterAction.IsShieldBreak, characterAction.FeedbackType);
        }
        #endregion
    }
}