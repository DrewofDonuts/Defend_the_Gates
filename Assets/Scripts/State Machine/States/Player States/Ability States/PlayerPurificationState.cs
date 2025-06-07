using UnityEngine;

namespace Etheral
{
    public class PlayerPurificationState : PlayerBaseAttackState
    {
        new float rotation;
        bool hasCastSpell;
        float spellDuration = 3f;
        float timer;


        public PlayerPurificationState(PlayerStateMachine _stateMachine, float momentum) : base(_stateMachine)
        {
            movementSpeed = Mathf.Clamp(momentum, 0, stateMachine.PlayerCharacterAttributes.WalkSpeed);
        }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.Purification;
            stateMachine.OnChangeStateMethod(StateType.Special);
            
            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            
            animationHandler.CrossFadeInFixedTime("PurificationStatic");


            // RegisterEvents();
            StartCooldown();
            RegisterEvents();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            
            HandleRotationBasedOnInputType(deltaTime, false, true);

            // HandleAllLocomotionAndAnimation(deltaTime, true, lockOverride: false, rotateOverride: true);

            // var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName, 1);
            timer += deltaTime;

            if (timer >= spellDuration)
            {
                ReturnToLocomotion();
                return;
            }

            if (timer >= characterAction.TimesBeforeSpells[0] && !hasCastSpell)
            {
                actionProcessor.CastSpellWithoutNormalizedValue();
                hasCastSpell = true;
            }

            canSwitch = timer >= .25f;

            // HandleAnimationSwitchForAnimationLayersWithOverride(deltaTime);
        }


        public override void Exit()
        {
            // animationHandler.CrossFadeInFixedTime("Default1");
            // animationHandler.SetAnimatorLayer(1, 0);
            PlayerComponents.GetSpellHandler().EndActiveSpell();
            DeRegisterEvents();
        }
        

        void EndTest()
        {
            ReturnToLocomotion();
        }
    }
}