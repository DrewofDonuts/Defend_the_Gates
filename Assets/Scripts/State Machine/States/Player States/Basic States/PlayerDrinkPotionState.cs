using System.Linq;

namespace Etheral
{
    public class PlayerDrinkPotionState : PlayerBaseState
    {
        bool hasPlayedAudio;
        bool hasGottenBottle;
        bool hasHealed;
        public PlayerDrinkPotionState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            characterAction =
                stateMachine.PlayerCharacterAttributes.Actions.FirstOrDefault(x => x.Name == "DrinkPotion");

            animationHandler.CrossFadeInFixedTime(characterAction);

            stateMachine.PlayerComponents.GetHealController().UsePotion();
            stateMachine.WeaponHandler.SetWeaponGO(true, false);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (normalizedTime >= characterAction.TimeBeforeAudio && !hasPlayedAudio)
            {
                stateMachine.CharacterAudio.PlayOneShot(stateMachine.CharacterAudio.OneShotSource,
                    characterAction.Audio);

                hasPlayedAudio = true;
            }

            //Hasn't gotten bottle yet and is before drinking
            if (normalizedTime >= characterAction.Action[0].TimeBeforeAction && !hasGottenBottle)
            {
                stateMachine.PlayerComponents.GetHealController().SetBottleGO(true);
                hasGottenBottle = true;
            }

            //has gotten bottle, and finished drinking
            if (normalizedTime >= characterAction.Action[1].TimeBeforeAction && !hasHealed)
            {
                // stateMachine.PlayerComponents.GetHealController().SetBottleGO(false);
                HealPlayer();
                hasHealed = true;
            }


            if (normalizedTime >= 1)
                ReturnToLocomotion();
        }

        void HealPlayer()
        {
            stateMachine.Health.Heal(stateMachine.Health.MaxHealth * 25f);
        }

        public override void Exit()
        {
            stateMachine.PlayerComponents.GetHealController().SetBottleGO(false);
            stateMachine.WeaponHandler.SetWeaponGO(true, true);

        }
    }
}