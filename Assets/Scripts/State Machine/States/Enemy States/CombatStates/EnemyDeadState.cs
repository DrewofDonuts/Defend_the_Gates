using UnityEngine;

namespace Etheral
{
    public class EnemyDeadState : EnemyBaseState
    {
        bool isBloodPool;
        float timeToWaitbeforeDisappearing = 4f;

        public EnemyDeadState(EnemyStateMachine stateMachine, string deathAnimation = "Dead") : base(stateMachine) { }

        static readonly int Dead = Animator.StringToHash("Dead");

        public override void Enter()
        {
            stateMachine.OnChangeStateMethod(StateType.Dead);

            enemyStateMachine.Health.SetIsDead();

            enemyStateMachine.CharacterAudio.PlayRandomEmote(enemyStateMachine.CharacterAudio.AudioLibrary.DeathEmote,
                AudioType.death);
            enemyStateMachine.Animator.CrossFadeInFixedTime(Dead, CrossFadeDuration);
            stateMachine.GetCharComponents().GetCC().enabled = false;
            aiComponents.GetNavMeshAgentController().SetIsStopped(true);
            enemyStateMachine.WeaponHandler.DisableAllMeleeWeapons();
            Collider[] colliders = enemyStateMachine.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }

            GameObject.Destroy(enemyStateMachine.gameObject, 10f);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            // stateMachine.transform.Translate(0, -.1f * deltaTime, 0);
            //
            // var normalizedTime = GetNormalizedTime(stateMachine.Animator, "Death");
            //
            // if (normalizedTime >= 1)
            // {
            //     if (!isBloodPool)
            //     {
            //         stateMachine.Health.EnableBloodPool();
            //         isBloodPool = true;
            //     }
            //
            //     if (timeToWaitbeforeDisappearing <= 0)
            //         EnemyDisappear(deltaTime);
            // }
        }

        public override void Exit() { }
    }
}