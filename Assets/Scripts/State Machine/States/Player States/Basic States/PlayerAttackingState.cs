using UnityEngine;
using UnityEngine.InputSystem;

namespace Etheral
{
    //Combo State Index in CharacterAction is the index of the NEXT ATTACK in the combo!!!!
    //So, if the combo state index is 1, then the next attack in the combo is the second attack in the array
    public class PlayerAttackingState : PlayerBaseAttackState
    {
        float previousFrameTime; //prevents getting data from final frame of previous animation

        bool canChangeDirection = true;
        const float RotationDampening = 1000;
        Transform currentTarget;
        EnemyStateMachine _enemyStateMachine;
        bool shouldCombo;

        float normalizedTimeBeforeCombo;
        float currentAttackSpeed;
        float newAttackSpeed;

        protected static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");

        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex = 0) : base(stateMachine)
        {
            if (stateMachine.WeaponInventory.RightEquippedWeapon.TypeOfWeapon is TypeOfWeapon.OneHandWeapon)
                characterAction = stateMachine.PlayerCharacterAttributes.Attacks[attackIndex];
            else if (stateMachine.WeaponInventory.RightEquippedWeapon.TypeOfWeapon is TypeOfWeapon.TwoHand)
                characterAction = stateMachine.PlayerCharacterAttributes.HammerAttacks[attackIndex];
        }

        public override void Enter()
        {
            currentAttackSpeed = PlayerComponents.GetStatsController().GetAttackSpeed();
            newAttackSpeed = Mathf.Max(currentAttackSpeed, 1f);

            stateMachine.Health.SetSturdy(true);

            if (CombatManager.Instance != null)
                CombatManager.Instance.SetPlayerAttacking(true);

            //check if there is a target
            if (HasTarget())
            {
                // //Temporarily disabling execution 11/09/24
                // if (IfCanExecute())
                // {
                //     HandleExecution();
                //     return;
                // }

                currentTarget = stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget.Transform;
            }
            else
                currentTarget = null;

            float bonusDamage = PlayerComponents.GetStatsController().GetBonusAttackDamage();

            animationHandler.CrossFadeInFixedTime(characterAction.AnimationName, 0);
            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction, bonusDamage);

            // stateMachine.InputReader.SouthButtonEvent += OnSouthButtonDown;
            // stateMachine.InputReader.AttackEvent += OnAttackButton;

            RegisterEvents();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            animationHandler.SetFloat(AttackSpeed, newAttackSpeed);

            if(stateMachine.isThirdPerson)
                ChangeDirectionThirdPerson(deltaTime);

            
            if (!stateMachine.isThirdPerson)
                ChangeDirectionOrAttackTargetTopDown(deltaTime);

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);

            if (GetInputType() == InputType.keyboard)
                if (normalizedTime >= .1f)
                    canChangeDirection = false;

            if (normalizedTime >= 1f)
            {
                ReturnToLocomotion();
            }

            actionProcessor.ApplyForceTimes(normalizedTime, CheckIfAdjacentToTarget(characterAction.AdjacentDistance));
            actionProcessor.RightWeaponTimes(normalizedTime);

            //if normalized time is greater than 1, then the animation already finished


            HandleEarlyExit(normalizedTime);

            //HANDLING COMBO ATTACKS
            if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
            {
                if (characterAction.NextComboStateIndex != -1)
                    normalizedTimeBeforeCombo = normalizedTime;


                if (stateMachine.InputReader.IsAttack)
                {
                    //DISABLING ATUTO ATTACKING!!  01/03/25
                    // TryComboAttack(normalizedTime);
                }

                // //quickly  end attack animation and return to locomotion 
                if (normalizedTime >= .70f && stateMachine.InputReader.MovementValue.magnitude > 0 &&
                    !stateMachine.InputReader.IsAttack)
                {
                    ReturnToLocomotion();
                }
            }
            else
            {
                ReturnToLocomotion();
            }

            if (shouldCombo && normalizedTime >= characterAction.ComboAttackTime)
            {
                playerBlocks.EnterAttackingState(characterAction.NextComboStateIndex);
            }


            previousFrameTime = normalizedTime;
        }

        void HandleEarlyExit(float normalizedTime)
        {
            canSwitch = normalizedTime >= .25f;
        }

        void ChangeDirectionThirdPerson(float deltaTime)
        {
            if (stateMachine.isThirdPerson && canChangeDirection &&
                stateMachine.InputReader.MovementValue.magnitude > .1f)
            {
                direction = CalculateMovementAgainstCamera();
                FaceMovementDirection(direction, deltaTime);
                return;
            }

        }

        void ChangeDirectionOrAttackTargetTopDown(float deltaTime)
        {
            if (GetInputType() is InputType.gamePad)
            {
                RotateByMouseTopDown(false);
                if (canChangeDirection && stateMachine.InputReader.MovementValue.magnitude > .1f)
                {
                    direction = CalculateMovementAgainstCamera();
                    FaceMovementDirection(direction, deltaTime);
                    return;
                }

                if (stateMachine.InputReader.MovementValue.magnitude <= .1f && IsLockEnabled())
                {
                    OnRotateTowardsTarget();
                }
            }
            else if (GetInputType() is InputType.keyboard)
            {
                if (!stateMachine.isThirdPerson)
                {
                    if (HasTarget() && IsLockEnabled())
                    {
                        OnRotateTowardsTarget();
                        RotateByMouseTopDown(false);
                        return;
                    }

                    if (canChangeDirection)
                    {
                        direction = CalculateMovementAgainstCamera();
                        RotateByMouseTopDown(true);
                    }
                }
            }
        }

        protected override void OnAttackDown()
        {
            if (normalizedTimeBeforeCombo > .20f)
                shouldCombo = true;
        }


        protected override void FaceMovementDirection(Vector3 movement, float deltaTime)
        {
            // _stateMachine.transform.rotation = Quaternion.LookRotation(movement);

            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
                Quaternion.LookRotation(movement), deltaTime * RotationDampening);

            canChangeDirection = false;
        }

        public override void Exit()
        {
            stateMachine.Health.SetSturdy(false);
            stateMachine.WeaponHandler.DisableAllMeleeWeapons();

            if (CombatManager.Instance != null)
                CombatManager.Instance.SetPlayerAttacking(false);

            // stateMachine.InputReader.SouthButtonEvent -= OnSouthButtonDown;
            // stateMachine.InputReader.AttackEvent -= OnAttackButton;

            DeRegisterEvents();
        }
    }
}


//EXECUTE CODE 
// bool IfCanExecute()
// {
//     _enemyStateMachine =
//         stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget.GetStateMachine();
//
//
//     if (_enemyStateMachine == null || _enemyStateMachine.Health == null ||
//         stateMachine.PlayerComponents.LockOnController.CurrentEnemyTarget.isObject ||
//         _enemyStateMachine.Health.IsDead || _enemyStateMachine.Health.IsSturdy)
//         return false;
//
//     return _enemyStateMachine.CalculateExecution(stateMachine.transform);
// }
//
// void HandleExecution()
// {
//     stateMachine.SwitchState(new PlayerExecutingState(stateMachine, _enemyStateMachine));
// }