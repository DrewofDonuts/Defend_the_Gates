using UnityEngine;

namespace Etheral
{
    public class NPCAttackState : NPCBaseState
    {
        float newAttackSpeed;
        protected static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");

        public NPCAttackState(CompanionStateMachine companionStateMachine, int attackIndex = 0) : base(companionStateMachine)
        {
            characterAction = companionStateMachine.AIAttributes.BasicAttacks[attackIndex];
        }


        public override void Enter()
        {
            Debug.Log("CompanionAttackState Enter");
            stateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();


            newAttackSpeed = stateMachine.GetAIComponents().GetStatHandler().GetAttackSpeed(stateMachine.AIAttributes);


            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            animationHandler.CrossFadeInFixedTime(characterAction);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            animationHandler.SetFloat(AttackSpeed, newAttackSpeed);


            // RotateTowardsTargetSmooth(stateMachine.AIAttributes.RotateSpeed);

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);

            actionProcessor.ApplyForceTimes(normalizedTime);
            actionProcessor.RightWeaponTimes(normalizedTime);

            if (normalizedTime >= 1)
            {
                stateMachine.SwitchState(new CompanionIdleCombatState(stateMachine));
            }
        }

        public override void Exit() { }
    }
}