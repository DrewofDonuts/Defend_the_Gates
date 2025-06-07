using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class FleshWardenHookState : EnemyBaseState
    {
        HookController hookController;
        bool hasThrownHook;
        bool hasBegunPull;

        public FleshWardenHookState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            hookController = enemyStateMachine.GetComponent<HookController>();
            characterAction = enemyStateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Hook");
            animationHandler.CrossFadeInFixedTime(characterAction.PreAnimation);

            StartCooldown(characterAction);

            hookController.hook.OnReturnHook += HandleHitSomething;
            
            AttackEffects();
            PlayEmote(characterAction, AudioType.attackEmote);
        }

        void HandleHitSomething()
        {
            animationHandler.CrossFadeInFixedTime(characterAction.AnimationName);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            
            

            if (!hasThrownHook && !hookController.hook.isHooked)
                RotateTowardsTargetSmooth(120f);

            if (hookController.hook.isHooked)
                RotateTowardsTargetSmooth(200f);


            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.PreAnimation);
            var normalizedReturnTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (normalizedTime >= characterAction.TimesBeforeSpells[0] && !hasThrownHook)
            {
                ThrowHook();
                hasThrownHook = true;
            }

            if (normalizedReturnTime >= 1f)
            {
                // if (EventBusPlayerController.PlayerStateMachine.StateType == StateType.KnockedDown && IsInMeleeRange())
                // {
                //     enemyStateBlocks.SwitchToGroundAttack();
                //     return;
                // }

                enemyStateBlocks.CheckLocomotionStates();
                return;
            }
        }

        void ThrowHook()
        {
            hookController.ThrowHook();
        }

        public override void Exit()
        {
            hookController.hook.OnReturnHook -= HandleHitSomething;
            hookController.hook.ResetHookToStartingState();
            hookController.hook.StopAllCoroutines();
        }
    }
}