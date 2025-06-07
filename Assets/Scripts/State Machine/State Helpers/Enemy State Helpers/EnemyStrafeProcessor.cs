using UnityEngine;

namespace Etheral
{
    public class EnemyStrafeProcessor
    {
        EnemyStateMachine _stateMachine;
        EnemyStrafeState _enemyStrafeState;
        bool canStrafe = true;

        const float MINIMUM_STRAFE_TIME = .5f;
        float strafeTimer;
        float pauseTimer;
        float attackTimer;

        public EnemyStrafeProcessor(EnemyStateMachine stateMachine, EnemyStrafeState enemyStrafeState)
        {
            _stateMachine = stateMachine;
            _enemyStrafeState = enemyStrafeState;
        }

        public float SetRandomStrafeTime()
        {
            float randomWaitStrafeTime = Random.Range(MINIMUM_STRAFE_TIME, _enemyStrafeState.maxStrafeTime);
            return randomWaitStrafeTime;
        }

        public int GetRandomDirection(int min, int max)

        {
            // int randomDirection = Random.Range(-1, 3);
            int randomDirection = Random.Range(min, max + 1);
            return randomDirection;
        }

        public void Tick(float deltaTime)
        {
            HandleStrafing(deltaTime);
            HandleAttackWhileStrafing(deltaTime);
        }

        void HandleAttackWhileStrafing(float deltaTime)
        {
            attackTimer += deltaTime;

            if (attackTimer >= _enemyStrafeState.checkToAttackTime)
            {
                var attackChance = Random.Range(0, 101);
                if (attackChance <= _stateMachine.AIAttributes.AttackRateFromStrafe)
                {
                    if (_enemyStrafeState.CheckPriorityAndTokenBeforeActions())
                    {
                        _enemyStrafeState.SwitchToAttack();
                    }
                }

                // _stateMachine.SwitchState(new EnemyAttackingState(_stateMachine));

                attackTimer = 0f;
            }
        }

        void HandleStrafing(float deltaTime)
        {
            strafeTimer += deltaTime;

            if (strafeTimer >= _enemyStrafeState.randomWaitStrafeTime)
            {
                _enemyStrafeState.SetDirection();

                SetRandomStrafeTime();
                strafeTimer = 0;
            }
        }
    }
}