using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class WeepingMotherShriekState : EnemyBaseState
    {
        bool playedAudio;
        public WeepingMotherShriekState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            characterAction = stateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Shriek");
            animationHandler.CrossFadeInFixedTime(characterAction);

            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(60);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (!playedAudio && normalizedTime >= characterAction.TimeBeforeAudio)
            {
                enemyStateMachine.CharacterAudio.PlayOneShot(stateMachine.CharacterAudio.EmoteSource,
                    characterAction.Audio);
                playedAudio = true;
            }

            actionProcessor.CastSpells(normalizedTime);

            if (normalizedTime >= 1f)
                enemyStateBlocks.CheckLocomotionStates();
        }

        public override void Exit()
        {
            actionProcessor.ClearAll();
        }
    }
}