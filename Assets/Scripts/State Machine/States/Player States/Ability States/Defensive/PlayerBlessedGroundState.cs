using UnityEngine;

namespace Etheral
{
    public class PlayerBlessedGroundState : PlayerBaseActionState
    {
        bool hasCasted;

        public PlayerBlessedGroundState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.BlessedGround;

            animationHandler.CrossFadeInFixedTime(characterAction);

            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);

            stateMachine.PlayerComponents.GetHealController().UseHeal();
            StartCooldown();
            
            actionProcessor.CastSpellWithoutNormalizedValue();
            RegisterEvents();
        }


        public override void Tick(float deltaTime)
        {
            float normalizedValue = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);
            // actionProcessor.CastSpells(normalizedValue);

            if (normalizedValue >= 1f)
            {
                ReturnToLocomotion();
            }
            
            if(normalizedValue >.1f && stateMachine.InputReader.MovementValue.magnitude > 0)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            DeRegisterEvents();
        }
    }
}