using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class WeepingMotherScreamState : EnemyBaseState
    {
        EnemyControlledSpawner enemySpawner;
        Transform target;
        bool hasSpawned;
        bool hasStartedSpellAnimation;
        bool playedAudio;

        public WeepingMotherScreamState(EnemyStateMachine _stateMachine) : base(
            _stateMachine) { }

        public override void Enter()
        {
            enemySpawner = enemyStateMachine.GetAIComponents().GetEnemySpawner();
            Debug.Log("WeepingMotherScreamState");
            characterAction = stateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Scream");

            animationHandler.CrossFadeInFixedTime(characterAction.PreAnimation);
            StartCooldown(characterAction);

            SpawnScreamEffect();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            HandleRotation();

            var normalizedTimePreAnimation = animationHandler.GetNormalizedTime(characterAction.PreAnimation);
            var normalizedTimeSpellAnimation = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            //Pre Animation is done
            if (normalizedTimePreAnimation >= 1 && !hasStartedSpellAnimation)
            {
                animationHandler.CrossFadeInFixedTime(characterAction, .4f);
                hasStartedSpellAnimation = true;
            }

            if (normalizedTimePreAnimation >= characterAction.TimeBeforeAudio && !playedAudio)
            {
                stateMachine.CharacterAudio.PlayOneShot(stateMachine.CharacterAudio.EmoteSource, characterAction.Audio);
                playedAudio = true;
            }

            //Pre Animation reaches the time to spawn the enemy
            if (normalizedTimePreAnimation >= characterAction.TimesBeforeSpells[0] && !hasSpawned)
            {
                enemySpawner.StartSpawner();
                hasSpawned = true;
            }


            //Spell Animation is done and should switch state
            if (normalizedTimeSpellAnimation >= .25)
                enemyStateBlocks.CheckLocomotionStates();
        }

        void HandleRotation()
        {
            if (!hasStartedSpellAnimation)
                RotateTowardsAnything(target, 120);

            if (hasStartedSpellAnimation)
                RotateTowardsTargetSmooth(60);
        }

        void SpawnScreamEffect()
        {
            if (characterAction.Effect != null)
            {
                var effect = Object.Instantiate(characterAction.Effect, stateMachine.transform.position,
                    stateMachine.transform.rotation);
                Object.Destroy(effect, 10f);
            }
        }


        public override void Exit() { }
    }
}