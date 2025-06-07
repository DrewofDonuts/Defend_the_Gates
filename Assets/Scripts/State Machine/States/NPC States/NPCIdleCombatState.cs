using UnityEngine;

namespace Etheral
{
    //Do not automatically SetHostile to false, so that the companion can attack enemies
    //Only following the player should set Hostile to false

    public class NPCIdleCombatState : NPCBaseState
    {
        bool shouldFollow;
        float timeBeforeFollow = .25f;
        float timer;
        float previousDistanceToPlayer;
        float distanceBeforeFollowingPlayer = 3f;


        public NPCIdleCombatState(NPCStateMachine npcStateMachine) : base(npcStateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime("Idle", .2f);
            Debug.Log("Switching to idle combat state");
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if (IsInMeleeRange() && !stateMachine.AITestingControl.blockAttack)
            {
                Debug.Log("Switching to attack");
                stateMachine.SwitchState(new NPCAttackState(stateMachine));
                return;
            }

            // Only call ChaseEnemyHandler() if stateMachine.GetHostile() is true
            if (stateMachine.GetHostile() && ChaseEnemyHandler()) return;
        }

        bool ChaseEnemyHandler()
        {
            //if companion has a target, switch to chase state
            if (stateMachine.GetNPCComponents.GetLockOnController().CurrentEnemyTarget != null)
            {
                stateMachine.SwitchState(new NPCChaseState(stateMachine));
                return true;
            }

            return false;
        }

        public override void Exit()
        {
        }
    }
}