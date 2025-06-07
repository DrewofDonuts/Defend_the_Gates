using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Etheral
{
    [Obsolete]
    public class TestEnemyStrafeState 
    {
    //     Image strafeIndicatorImage;
    //     EnemyStateBlocks _enemyStateBlocks;
    //     Transform player;
    //     Transform transform;
    //     NavMeshAgent agent;
    //
    //
    //     float strafeDistance;
    //
    //     float randomWaitStrafeTime;
    //     int randomDirection;
    //     float rotationSpeed;
    //
    //     #region AttackLogic
    //     //counts down 
    //     public float attackTimer { get; private set; } = ATTACKTIMER;
    //
    //     //resets the timer once it hits zero
    //     const float ATTACKTIMER = 1f;
    //
    //     //when false, goes into logic to assign random value
    //     bool isRolled;
    //
    //     //will be assigneg random value to determine if should attack
    //     int attackChance;
    //     float direction;
    //     #endregion
    //
    //
    //     public TestEnemyStrafeState(EnemyStateMachine stateMachine) : base(stateMachine)
    //     {
    //         // agent = stateMachine.Agent;
    //         // _enemyStateBlocks = new EnemyStateBlocks(base.stateMachine);
    //         player = base.stateMachine.target.transform;
    //         transform = base.stateMachine.transform;
    //         strafeDistance = base.stateMachine.CharacterAttributes.AdjacentAttackRange;
    //     }
    //
    //     public override void Enter()
    //     {
    //         stateMachine.Animator.CrossFadeInFixedTime("StrafeState", CrossFadeDuration);
    //         SetRandomValues();
    //
    //         stateMachine.StartCoroutine(TimeBeforeSwitchingStrafe());
    //     }
    //
    //     public override void Tick(float deltaTime)
    //     {
    //         Move(deltaTime);
    //         RotateTowardsPlayerSmooth(rotationSpeed);
    //     }
    //
    //     public override void Exit()
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //
    //     void StrafingLogic()
    //     {
    //         if (IsInMeleeRange())
    //         {
    //             StrafeMovement();
    //         }
    //     }
    //
    //     public void StrafeMovement()
    //     {
    //         Vector3 strafeDirection = Quaternion.Euler(0, 90, 0) * (direction * player.position - transform.position);
    //         agent.destination = player.position + strafeDirection.normalized * strafeDistance;
    //     }
    //
    //     //TODO: Remove
    //     // void LookTowards( )
    //     // {
    //     //     var lookPos = (player.position - transform.position).normalized;
    //     //     lookPos.y = 0;
    //     //    transform.rotation = Quaternion.LookRotation(lookPos);
    //     //     transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, Time.deltaTime * rotationSpeed);
    //     // }
    //
    //     IEnumerator TimeBeforeSwitchingStrafe()
    //     {
    //         yield return new WaitForSeconds(randomWaitStrafeTime);
    //         stateMachine.SwitchState(new TestEnemyStrafeState(stateMachine));
    //     }
    //
    //     void SetRandomValues()
    //     {
    //         SetRandomDirection();
    //         SetRandomStrafeTime();
    //     }
    //
    //     void SetRandomStrafeTime()
    //     {
    //         randomWaitStrafeTime = Random.Range(1f, 4f);
    //     }
    //
    //     void SetRandomDirection()
    //     {
    //         randomDirection = Random.Range(0, 2);
    //
    //         if (randomDirection is 0)
    //             direction = -1;
    //         else
    //             direction = 1;
    //     }
    }
}