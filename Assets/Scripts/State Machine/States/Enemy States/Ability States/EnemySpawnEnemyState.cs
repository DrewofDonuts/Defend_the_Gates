using System.Linq;
using UnityEngine;


namespace Etheral
{
    public class EnemySpawnEnemyState : EnemyBaseState
    {
        EnemyControlledSpawner enemySpawner;
        int spawnNumber;
        bool spawnedEffect;
        bool hasSpawned;

        public EnemySpawnEnemyState(EnemyStateMachine _stateMachine, int spawnNumberOverride = 0) :
            base(_stateMachine)
        {
            spawnNumber = spawnNumberOverride;
        }

        public override void Enter()
        {
            enemySpawner = enemyStateMachine.GetAIComponents().GetEnemySpawner();
            characterAction = stateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "SpawnEnemy");

            if (characterAction != null)
            {
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName);
            }

            StartCooldown(characterAction);
            
            PlayEmote(characterAction, AudioType.attackEmote);
            

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (normalizedTime >= characterAction.TimesBeforeSpells[0] && !hasSpawned)
            {
                Debug.Log($"Total enemies to spawn: " +  spawnNumber);
                enemySpawner.StartSpawner(spawnNumber);
                hasSpawned = true;
            }

            SpawnEffect(normalizedTime);

            if (normalizedTime >= 1f)
            {
                enemyStateBlocks.CheckLocomotionStates();
            }
        }

        public override void Exit() { }

        void SpawnEffect(float normalizedTime)
        {
            if (!characterAction.HasEffect) return;
            if (spawnedEffect) return;


            if (normalizedTime >= characterAction.TimeBeforeEffect)
            {
                var effect = Object.Instantiate(characterAction.Effect, stateMachine.transform.position,
                    stateMachine.transform.rotation);

                Object.Destroy(effect, 10f);
                spawnedEffect = true;
            }
        }
    }
}