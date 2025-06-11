using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public abstract class EnemyBaseState : AIBaseState
    {
        protected bool isCounterAction;
        protected float timeBeforePatrol = 3f;
        protected float timerBeforePatrol;
        public AIComponentHandler aiComponents;
        protected List<TargetType> targetTypes;
        protected int gateIndex = -1;
        protected int fellowshipIndex = -1;

        protected EnemyStateMachine enemyStateMachine;
        protected EnemyStateBlocks enemyStateBlocks;

        public EnemyBaseState(EnemyStateMachine _stateMachine) : base(_stateMachine)
        {
            enemyStateMachine = _stateMachine;
            aiComponents = enemyStateMachine.GetAIComponents();
            enemyStateBlocks = new EnemyStateBlocks(enemyStateMachine, this);
            animationHandler = enemyStateMachine.GetAIComponents().GetAnimationHandler();
            targetTypes = aiComponents.GetAILockOnController().AIPriorities;

            if (targetTypes.Contains(TargetType.Gate))
                gateIndex = targetTypes.IndexOf(TargetType.Gate);

            if (targetTypes.Contains(TargetType.Fellowship))
                fellowshipIndex = targetTypes.IndexOf(TargetType.Fellowship);
        }


        #region Effects
        protected void PlayEmote(CharacterAction _characterAction, AudioType audioType)
        {
            enemyStateMachine.CharacterAudio.PlayEmote(_characterAction.Audio,
                AudioType.attackEmote);
        }

        protected void PlayEmote(AudioClip[] audioClips, AudioType audioType)
        {
            enemyStateMachine.CharacterAudio.PlayRandomEmote(enemyStateMachine.CharacterAudio.AudioLibrary?.AttackEmote,
                AudioType.attackEmote);
        }

        protected void AttackEffects()
        {
            aiComponents.GetHighlightEffectController().TriggerAttackEffect();
        }
        #endregion


        #region AI Behavior Checking
        protected void CheckCombatWIthTimer(float deltaTime)
        {
            checkNextActionTimer += deltaTime;
            
            if (checkNextActionTimer > checkNextActionInterval)
            {
                if (CheckPriorityAndTokenBeforeActions() && !enemyStateMachine.AITestingControl.idleAndImpactOnly)
                {
                    timerBeforePatrol = 0;

                    if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                        return;

                    if (enemyStateBlocks.CanStrafe())
                    {
                        enemyStateBlocks.SwitchToStrafe();
                        return;
                    }

                    if (!IsInMeleeRange())
                    {
                        enemyStateBlocks.SwitchToChase();
                        return;
                    }
                }
                else
                {
                    //do this if the enemy is not in chase range
                    enemyStateMachine.RemoveTokenAndQueue();
                }

                checkNextActionTimer = 0;
            }
        }


        //Check if the enemy has a token before performing an action, but should enable action if
        //TokenManager is not present
        public bool CheckPriorityAndTokenBeforeActions(bool isPlayerAttacking = false)
        {
            return ShouldEngageInTarget(isPlayerAttacking) && CheckToken();
        }

        //IMPORTANT LOGIC TO DETERMINE PLAYER AND TARGETING PRIORITY
        bool ShouldEngageInTarget(bool isPlayerAttacking)
        {
            Debug.Log($"Current Target is: {currentTarget?.Transform.name}");

            if (currentTarget == null) return false;

            if (!targetTypes.Contains(currentTarget.TargetType))
                return false;

            if (IsInChaseRangeTarget())
                return true;


            //If fellowship is not a priority, or not on the list when player is attacking, then false
            return false;
        }

        bool CheckToken()
        {
            //if the enemy does not use a token, return true
            if (!stateMachine.UsesToken) return true;

            if (TokenManager.Instance != null)
            {
                //if the enemy already has a token, return true
                if (stateMachine.GetCurrentAttackToken() != null)
                    return true;

                return stateMachine.RequestToken();
            }

            return true;
        }
        #endregion


        protected void EnemyDisappear(float deltaTime, float speed = .15f, float acceleration = .01f)
        {
            if (enemyStateMachine.Animator.applyRootMotion)
                enemyStateMachine.Animator.applyRootMotion = false;

            speed += acceleration * deltaTime;

            Vector3 newPosition = new Vector3(enemyStateMachine.transform.position.x,
                enemyStateMachine.transform.position.y - speed, enemyStateMachine.transform.position.z);

            // Interpolate the position of the object using Lerp
            enemyStateMachine.transform.position =
                Vector3.Lerp(enemyStateMachine.transform.position, newPosition, Time.deltaTime);
        }


        protected void HandlePatrolLogic(float deltaTime)
        {
            if (patrolController.GetIfThisIsAPatrollingEnemy())
            {
                timerBeforePatrol += deltaTime;
                if (timerBeforePatrol >= timeBeforePatrol)
                    enemyStateMachine.SwitchState(new EnemyPatrolState(enemyStateMachine));
            }
        }


        protected void StartCooldown(CharacterAction characterAction)
        {
            if (enemyStateBlocks.IsCooldownManagerActive())
                CooldownManager.Instance.StartCooldown(characterAction);
        }
    }
}