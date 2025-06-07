using UnityEngine;

namespace Etheral
{
    public class EnemyFallingState : EnemyBaseState
    {
        public EnemyFallingState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }
        float timeBeforeDeath = 1.5f;
        float timer;
        bool hasTriggeredDeath;


        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("Falling", 0.1f);
            aiComponents.GetNavMeshAgentController().DisableAgentComponent();
            stateMachine.Health.SetSturdy(true);

            var audioToPlay = enemyStateMachine.CharacterAudio.AudioLibrary.FallingDeath ??
                              enemyStateMachine.CharacterAudio.AudioLibrary.DeathEmote;
            
            enemyStateMachine.CharacterAudio.PlayRandomEmote(audioToPlay,
                AudioType.death);
            Debug.Log("Falling to death");
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            timer += deltaTime;

            if (timer >= timeBeforeDeath && !hasTriggeredDeath)
            {
                Debug.Log("Triggering death after falling");
                stateMachine.Health.SetExecution();
                hasTriggeredDeath = true;
            }
        }

        public override void Exit() { }
    }
}