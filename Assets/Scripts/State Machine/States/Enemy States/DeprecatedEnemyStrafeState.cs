using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Etheral
{
    [Obsolete]
    public class DeprecatedEnemyStrafeState : EnemyBaseState
    {
        // int randomStrafeDir;
        float randomWaitStrafeTime;
        int randomDirection;
        int direction;
        int randomAttack;
        bool shouldBlock;
        Image strafeIndicatorImage;
        EnemyStateBlocks _enemyStateBlocks;
        const float TimeBeforeChase = .5f;
        float timeBeforeWalkingForward = .25f;
        float chaseTimer;
        float moveTimer;

        #region AttackLogic
        //counts down 
        public float attackTimer { get; private set; } = ATTACKTIMER;

        //resets the timer once it hits zero
        const float ATTACKTIMER = 1f;

        //when false, goes into logic to assign random value
        bool isRolled;

        //will be assigneg random value to determine if should attack
        int attackChance;
        #endregion

        public DeprecatedEnemyStrafeState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            // _enemyStateBlocks = new EnemyStateBlocks(base.stateMachine);
        }

        public override void Enter()
        {
            enemyStateMachine.Animator.CrossFadeInFixedTime("StrafeState", CrossFadeDuration);

            SetRandomValues();
            // stateMachine.Agent.updatePosition = true;
            // stateMachine.Agent.updateRotation = true;

            strafeIndicatorImage = enemyStateMachine.stateIndicator;
            if (strafeIndicatorImage != null)
                strafeIndicatorImage.color = Color.blue;
        }


        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSnap();

            // MyStrafing(deltaTime);
            RandomAttack(deltaTime);

            //idle
            if (!IsInChaseRangeTarget())
            {
                enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
                return;
            }

            //Chase
            if (!IsInStrafeRange() && IsInChaseRangeTarget())
            {
                chaseTimer += deltaTime;

                if (chaseTimer >= TimeBeforeChase)
                    enemyStateMachine.SwitchState(new EnemyChaseState(enemyStateMachine));
                else if (!IsInChaseRangeTarget())
                {
                    chaseTimer = 0;
                }
            }
        
        }

        // void MyStrafing(float deltaTime)
        // {
        //     randomWaitStrafeTime -= deltaTime;
        //
        //     if (IsInMeleeRange())
        //     {
        //         _stateMachine.transform.RotateAround(_stateMachine.Player.transform.position, Vector3.up,
        //             _stateMachine.Angle * direction * deltaTime);
        //
        //         if (_stateBlocks.CheckIfShouldBlock())
        //         {
        //             shouldBlock = true;
        //             UpdateBlockingAnimator(0, direction, deltaTime);
        //         }
        //         else
        //         {
        //             UpdateAnimator(0, direction, deltaTime);
        //         }
        //
        //         //commented out for testing
        //         // RandomAttack(deltaTime);
        //     }
        //     // else if (!IsInMeleeRange() && !IsInStrafeRange())
        //     // {
        //     //     moveTimer += deltaTime;
        //     //     if (moveTimer >= timeBeforeWalkingForward)
        //     //     {
        //     //         MoveToPlayer(deltaTime);
        //     //         UpdateAnimator(1, 0, deltaTime);
        //     //     }
        //     // }
        //
        //     _stateMachine.Agent.velocity = _stateMachine.CharacterController.velocity;
        //
        //     if (randomWaitStrafeTime <= 0)
        //     {
        //         SetRandomValues();
        //         //uncommented for testing - unsure why it was commented out
        //         _stateMachine.SwitchState(new DeprecatedEnemyStrafeState(_stateMachine));
        //     }
        // }

        void SetRandomValues()
        {
            randomWaitStrafeTime = Random.Range(1f, 4f);
            randomDirection = Random.Range(0, 2);

            if (randomDirection is 0)
                direction = -1;
            else
                direction = 1;
        }

        void RandomAttack(float deltaTime)
        {
            attackTimer -= deltaTime;

            if (!isRolled)
            {
                isRolled = true;
                attackChance = Random.Range(0, 2); //0,2 value == 50% chance attack everytime the timer runs through
            }

            if (attackTimer <= 0)
            {
                if (attackChance == 0)
                {
                    enemyStateMachine.SwitchState(new EnemyAttackingState(enemyStateMachine,0));
                }
                else
                {
                    isRolled = false;
                    attackTimer = ATTACKTIMER;
                }
            }
        }

        void UpdateBlockingAnimator(int forward, int right, float deltaTime)
        {
            if (shouldBlock)
            {
                enemyStateMachine.Animator.CrossFadeInFixedTime("StrafeState", CrossFadeDuration);
                shouldBlock = false;
            }


            enemyStateMachine.Animator.SetFloat("ForwardSpeed", forward, .5f, deltaTime);
            enemyStateMachine.Animator.SetFloat("RightSpeed", right, .5f, deltaTime);
        }

        void UpdateAnimator(int forward, int right, float deltaTime)
        {
            enemyStateMachine.Animator.SetFloat("ForwardSpeed", forward, .5f, deltaTime);
            enemyStateMachine.Animator.SetFloat("RightSpeed", right, .5f, deltaTime);
        }


        public override void Exit()
        {
            // stateMachine.Agent.updatePosition = false;
            // stateMachine.Agent.updateRotation = false;
            // stateMachine.Agent.ResetPath();
            // stateMachine.Agent.velocity = Vector3.zero;
        }

        void MoveToPlayer(float deltaTime)
        {
            // if (stateMachine.Agent.isOnNavMesh)
            // {
            //     stateMachine.Agent.destination = stateMachine.Player.transform.position;
            //
            //     //.normalized will equal 1
            //     Move(stateMachine.Agent.desiredVelocity.normalized *
            //          stateMachine.CharacterAttributes.StrafeSpeed, deltaTime);
            // }
            //
            // //keep the agent in sync
            // stateMachine.Agent.velocity = stateMachine.CharacterController.velocity;
        }
    }
}