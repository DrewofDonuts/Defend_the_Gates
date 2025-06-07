namespace Etheral
{
    public class PlayerCrusadeStrikeState : PlayerBaseActionState
    {
        public PlayerCrusadeStrikeState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.GetAbility("CrusadeStrike");
            animationHandler.CrossFadeInFixedTime(characterAction.AnimationName, CrossFadeDuration);

            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            actionProcessor.ApplyForceTimes(deltaTime);
            actionProcessor.LeftWeaponTimes(deltaTime);

            if (normalizedTime >= 1f)
            {   
                ReturnToLocomotion();
                return;
            }   
        }

        public override void Exit() { }
    }
}