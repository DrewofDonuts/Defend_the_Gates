using UnityEngine;

namespace Etheral
{
    public class PlayerShieldBashState : PlayerBaseState
    {
        float forceTime;

        public PlayerShieldBashState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.ShieldBash;
            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            animationHandler.CrossFadeInFixedTime(characterAction);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            HasTarget();
            forceTime -= Mathf.Max(deltaTime, 0);

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);

            if (forceTime <= 0)
            {
                stateMachine.Health.SetSturdy(true);

                if (stateMachine.InputReader.IsAttack)
                {
                    playerBlocks.EnterAttackingState();
                    return;
                }
            }

            actionProcessor.ApplyForceTimes(normalizedTime);
            actionProcessor.LeftWeaponTimes(normalizedTime);

            if (normalizedTime >= 1)
                ReturnToLocomotion();
        }

        public override void Exit()
        {
            stateMachine.Health.SetSturdy(false);
        }
    }
}