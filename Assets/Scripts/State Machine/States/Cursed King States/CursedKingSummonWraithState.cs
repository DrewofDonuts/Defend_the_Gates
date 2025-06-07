using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class CursedKingSummonWraithState : EnemyBaseState
    {
        public CursedKingSummonWraithState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            enemyStateMachine.Health.SetSturdy(true);
            characterAction =
                enemyStateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Summon Wraiths");

            animationHandler.CrossFadeInFixedTime(characterAction);

            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);
            
            PlayEmote(characterAction, AudioType.attackEmote);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            actionProcessor.CastSpells(normalizedTime);

            if (normalizedTime >= 1f)
            {
                enemyStateBlocks.CheckLocomotionStates();
            }
        }

        public override void Exit()
        {
            enemyStateMachine.Health.SetSturdy(false);
            aiComponents.GetCustomController<CursedKingController>().ResetWraithTimer();
        }
    }
}