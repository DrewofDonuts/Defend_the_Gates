using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class EnemyHealState : EnemyBaseState
    {
        public EnemyHealState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Healing State");

            characterAction = enemyStateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Heal");

            if (characterAction != null)
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName);

            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);
            StartCooldown(characterAction);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);


            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (normalizedTime >= 1f)
                enemyStateBlocks.CheckLocomotionStates();


            actionProcessor.CastSpells(normalizedTime);
        }

        public override void Exit() { }
    }
}