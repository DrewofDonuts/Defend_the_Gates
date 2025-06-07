using UnityEngine;

namespace Etheral
{
    public class PlayerCrusadeStateOld : PlayerBaseAbilityState
    {
        float attackForceTime;
        float chargeUpTime;
        float endTime;
        float attackReadyTime = .50f;
        const float TURNSPEED = .75f;
        bool hasAttacked;


        public PlayerCrusadeStateOld(PlayerStateMachine stateMachine, Ability ability) : base(stateMachine, ability)
        {
        }

        public override void Enter()
        {
            Debug.Log("Entering Crusade State");

            // ability = stateMachine.SpellHandler.Crusade;

            stateMachine.InputReader.CancelEvent += EndPhase;
            // stateMachine.Animator.CrossFadeInFixedTime(stateMachine.SpellHandler.Crusade.Animation,
            //     CrossFadeDuration);

            // _stateMachine.Animator.SetBool("IsBlocking", true);
            stateMachine.Health.SetBlocking(true);
            stateMachine.Health.SetSturdy(true);

            endTime = ability.EndTime;
            chargeUpTime = ability.ForceTime;
            attackForceTime = ability.PreForceTime;

            stateMachine.WeaponHandler._currentLeftHandDamage.SettAttackStatDamage(ability.DirectDamage,
                ability.KnockBackForce,
                ability.KnockDownForce);

            PassAttackBasedDamage();
        }

        void PassAttackBasedDamage()
        {
            stateMachine.WeaponHandler._currentRightHandDamage.SettAttackStatDamage(ability.DirectDamage,
                ability.KnockBackForce,
                ability.KnockDownForce);
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public override void Tick(float deltaTime)
        {
            Vector3 movement = CalculateMovement();

            chargeUpTime -= deltaTime;
            if (stateMachine.InputReader.IsWestButton)
            {
                attackReadyTime -= Mathf.Max(deltaTime, 0);

                if (chargeUpTime <= 0)
                {
                    Move(movement * ability.MovementForce, deltaTime);
                    var rotation = CalculateFacingDirection(deltaTime);
                    RotatePlayer(rotation, deltaTime);
                }
            }
            else if (!stateMachine.InputReader.IsWestButton && attackReadyTime <= 0)
            {
                Move(movement * ability.MovementForce, deltaTime);

                attackForceTime -= Mathf.Max(deltaTime, 0);

                if (attackForceTime <= 0)
                {
                    stateMachine.Health.SetBlocking(false);
                    AbilityForce(ability.PreMovementForce);
                }

                if (!hasAttacked)
                {
                    EndPhase();
                }


                float normalizedValue =
                    GetNormalizedTime(stateMachine.Animator, "CrusadeEnd");

                if (normalizedValue >= 1)
                    ReturnToLocomotion();


                //     endTime -= deltaTime;
                //     if (endTime <= 0)
                //     {
                //         ReturnToLocomotion();
                //     }
                // }
                // else
                //     ReturnToLocomotion();
            }
        }

        void EndPhase()
        {
            hasAttacked = true;
            stateMachine.Animator.CrossFadeInFixedTime("CrusadeEnd", CrossFadeDuration);

            // _stateMachine.Animator.SetBool("IsBlocking", false);
        }

        public override void Exit()
        {
            stateMachine.InputReader.CancelEvent -= EndPhase;
            stateMachine.Health.SetBlocking(false);
            stateMachine.Health.SetSturdy(false);
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
                Quaternion.LookRotation(movement), deltaTime * TURNSPEED);
        }
    }
}