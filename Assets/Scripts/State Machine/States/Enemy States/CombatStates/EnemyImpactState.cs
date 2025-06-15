using UnityEngine;

namespace Etheral
{
    public class EnemyImpactState : EnemyBaseState
    {
        IDamage attackerInfo;

        public EnemyImpactState(EnemyStateMachine stateMachine, IDamage _attackerInfo) : base(stateMachine)
        {
            attackerInfo = _attackerInfo;
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Impact);
            // RotateTowardsTargetSnap();
            // RotateTowardsAttackerSnap();

            

            enemyStateMachine.WeaponHandler.DisableAllMeleeWeapons();
        }

        void RotateTowardsAttackerSnap()
        {
            if (attackerInfo == null) return;
            if(attackerInfo.AttackerID == -1) return; 
            if (stateMachine.AITestingControl.blockRotate) return;
            
            if(attackerInfo.Transform == null)
                Debug.Log("Attacker Transform is null in Impact State");
            

            var lookPos = attackerInfo.Transform.position - enemyStateMachine.transform.position;
            lookPos.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
        }

        public override void Tick(float deltaTime)
        {
            //Move is called to make sure gravity is active
            Move(deltaTime);

            float normalizedvalue = GetNormalizedTime(enemyStateMachine.Animator, "Impact");

            if (normalizedvalue >= 1)
            {
                HandleStateFromImpact();
            }
        }

        void HandleStateFromImpact()
        {
            // stateMachine.CharacterAttributes.CounterAttackSelector.CounterStateSelector(stateMachine);

            if (CheckPriorityAndTokenBeforeActions(true))
            {
                if (CheckIfCanCounterAction())
                {
                    enemyStateMachine.AIAttributes.CounterAttackSelector.CounterStateSelector(enemyStateMachine);
                    return;
                }


                if (enemyStateBlocks.CheckAttacksFromLocomotionState())
                    return;
            }


            enemyStateBlocks.CheckLocomotionFromImpactState();
        }

        bool CheckIfCanCounterAction()
        {
            return enemyStateMachine.AIAttributes.CanCounterAction && enemyStateMachine.CheckIfCounterActionIsReady() &&
                   !enemyStateMachine.AITestingControl.blockCounterAttack &&
                   !enemyStateMachine.AITestingControl.idleAndImpactOnly &&
                   !enemyStateMachine.AITestingControl.blockAttack;
        }

        public override void Exit() { }
    }
}