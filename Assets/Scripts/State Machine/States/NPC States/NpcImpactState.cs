using UnityEngine;

namespace Etheral
{
    public class NpcImpactState : NPCBaseState
    {
        readonly int Impact = Animator.StringToHash("Impact");

        public NpcImpactState(CompanionStateMachine companionStateMachine) : base(companionStateMachine)
        {
            
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Impact);
            stateMachine.WeaponHandler.DisableAllMeleeWeapons();
        }

        public override void Tick(float deltaTime)
        {
            //Move is called to make sure gravity is active
            Move(deltaTime);

            float normalizedvalue = GetNormalizedTime(stateMachine.Animator, "Impact");

            if (normalizedvalue >= 1)
            {
                stateMachine.SwitchState(new CompanionIdleCombatState(stateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}