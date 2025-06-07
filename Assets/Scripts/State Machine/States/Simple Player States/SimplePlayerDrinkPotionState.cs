using System.Linq;

namespace Etheral
{
    public class SimplePlayerDrinkPotionState : SimplePlayerBaseState
    {
        bool hasPlayedAudio;
        bool hasGottenBottle;
        bool hasHealed;

        public SimplePlayerDrinkPotionState(SimplePlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterActions
                .FirstOrDefault(x => x.CharacterAction.Name == "DrinkPotion")
                ?.CharacterAction;

            animationHandler.CrossFadeInFixedTime(characterAction);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            //Hasn't gotten bottle yet and is before drinking
            if (normalizedTime >= characterAction.Action[0].TimeBeforeAction && !hasGottenBottle)
            {
                stateMachine.GetPlayerComponents().GetPotionPrefab().SetActive(true);
                hasGottenBottle = true;
            }

            if (normalizedTime >= characterAction.TimeBeforeAudio && !hasPlayedAudio)
            {
                characterAudio.PlayOneShot(characterAudio.OneShotSource,
                    characterAction.Audio);

                hasPlayedAudio = true;
            }

            if (normalizedTime >= 1)
            {
                stateMachine.SwitchState(new SimplePlayerMovementState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            EtheralMessageSystem.SendKey(this, "HIDEICON");
            stateMachine.SetHasPotion(false);
            stateMachine.GetPlayerComponents().GetPotionPrefab().SetActive(false);
        }
    }
}