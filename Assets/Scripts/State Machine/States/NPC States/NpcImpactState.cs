using UnityEngine;

namespace Etheral
{
    public class NpcImpactState : NPCBaseState
    {
        readonly int Impact = Animator.StringToHash("Impact");

        public NpcImpactState(NPCStateMachine npcStateMachine) : base(npcStateMachine)
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
                stateMachine.SwitchState(new NPCIdleCombatState(stateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}