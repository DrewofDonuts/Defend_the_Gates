using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class EnemyRangedAttackState : EnemyBaseState
    {
        Image attackIndicatorState;
        string drawAnimation = "Draw";
        string reloadAnimation = "Reload";
        string aimAnimation = "Aim";
        float aimTime = 1f;
        float timer;


        bool isAiming;
        bool isDrawing;
        bool hasPlayedReloadAudio;
        bool hasPlayedEffects;


        public EnemyRangedAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            // ResetNavAgent();
            // enemyStateMachine.ComponentHandler.navMeshAgentController.DisableAgentUpdate();

            var randomNumber = Random.Range(2, 6);

            if (enemyStateMachine.GetCurrentAttackCount() > randomNumber)
            {
                enemyStateMachine.SwitchState(new EnemyRangedMoveState(enemyStateMachine));
                return;
            }


            characterAction = RangedAttackSelector();
            aimTime = characterAction.AimTime;
            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);


            // RotateTowardsTargetSnap();
            PassRangedDamage();
            animationHandler.CrossFadeInFixedTime(reloadAnimation, 0.1f);

            attackIndicatorState = enemyStateMachine.stateIndicator;
            if (attackIndicatorState != null)
                attackIndicatorState.color = Color.yellow;

            enemyStateMachine.GetAIComponents().navMeshAgentController.SetIsStopped(true);

            enemyStateMachine.TrackAttacksForToken();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(30f);
            var reloadNormalizedTime = animationHandler.GetNormalizedTime(reloadAnimation);
            var drawNormalizedTime = animationHandler.GetNormalizedTime(drawAnimation);
            var aimNormalizedTime = animationHandler.GetNormalizedTime(aimAnimation);
            var fireProjectileNormalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            actionProcessor.FireProjectiles(fireProjectileNormalizedTime, 0);


            //reload finishes, so draw weapon back
            ReloadToDraw(reloadNormalizedTime);

            //play audio for drawing weapon
            HandleDrawAudio(drawNormalizedTime);

            //draw weapon finishes, so aim
            DrawToAim(drawNormalizedTime);

            //how long to aim
            HandleAimTimer(deltaTime);

            //aiming finishes, so fire projectile
            AimToFire();


            if (fireProjectileNormalizedTime >= 1f)
            {
                Debug.Log("Attack Complete");
                if (IsInRangedRange())
                {
                    enemyStateMachine.SwitchState(new EnemyRangedAttackState(enemyStateMachine));
                }
                else
                {
                    enemyStateBlocks.CheckLocomotionStates();
                }
            }
        }

        void AimToFire()
        {
            if (timer >= .95f && !hasPlayedEffects)
            {
                AttackEffects();
                hasPlayedEffects = true;
            }

            if (timer >= aimTime && isAiming)
            {
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName);
                isAiming = false;
            }
        }

        void HandleAimTimer(float deltaTime)
        {
            if (isAiming)
            {
                timer += deltaTime;
            }
        }

        void DrawToAim(float drawNormalizedTime)
        {
            if (drawNormalizedTime >= 1f && !isAiming)
            {
                animationHandler.CrossFadeInFixedTime(aimAnimation);
                isAiming = true;
            }
        }

        void HandleDrawAudio(float drawNormalizedTime)
        {
            if (drawNormalizedTime >= characterAction.TimeBeforeDrawAudio && !hasPlayedReloadAudio)
            {
                stateMachine.CharacterAudio.PlayOneShot(stateMachine.CharacterAudio.RangedDamageSource,
                    stateMachine.WeaponInventory.RangedEquippedWeapon.DrawAudio, AudioType.rangedWeapon);

                hasPlayedReloadAudio = true;
            }
        }

        void ReloadToDraw(float reloadNormalizedTime)
        {
            if (reloadNormalizedTime >= 1 && !isDrawing)
            {
                animationHandler.CrossFadeInFixedTime(drawAnimation);
                isDrawing = true;
            }
        }


        CharacterAction RangedAttackSelector()
        {
            var attackNumber = enemyStateMachine.AIAttributes.RangedAttackObjects.Count;
            int attack = Random.Range(0, attackNumber);

            return enemyStateMachine.AIAttributes.RangedAttackObjects[attack].CharacterAction;
        }

        void PassRangedDamage()
        {
            if (enemyStateMachine.WeaponInventory.RangedEquippedWeapon == null) return;
            enemyStateMachine.WeaponHandler.RangedWeaponDamage.SetAttackStatDamage(characterAction.Damage,
                characterAction.KnockBackForce,
                characterAction.KnockDownForce);
        }

        public override void Exit()
        {
            stateMachine.GetAIComponents().navMeshAgentController.SetIsStopped(false);
            stateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }
    }
}