using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class NPCStateMachine : AIStateMachine
    {
        CompanionStateMachineProcessor stateMachineProcessor = new();
        [SerializeField] NPCComponents npcComponents;
        public NPCComponents GetNPCComponents => npcComponents;

        [SerializeField] bool isNPC;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            EnterStartingState();

            if (AITestingControl != null && !AITestingControl.displayStateIndicator)
                stateIndicator.enabled = false;
        }

        public override ITargetable GetTarget()
        {
            var target = GetAIComponents().GetEnemyLockOnController().GetTarget();

            if (target == null) return null;
            
            Target = target.Transform;

            return target;
        }

        protected override void HandlePlayerStateChange(StateType newstatetype) { }


        protected override void HandleDeath(IHaveHealth health)
        {
            stateMachineProcessor.HandleDead(this);
        }

        protected override void HandleTakeHit(IDamage iDamage)
        {
            if (AITestingControl.blockSwitchState) return;
            stateMachineProcessor.TakeHit(iDamage, this);
        }

        protected override void HandleBlock(IDamage iDamage) { }

        protected override void RegisterWithCharacterManager() { }

        protected override void DeRegisterWithCharacterManager() { }

        public override void TriggerImpactTimer() { }

        protected override void DisableCanvasGroup() { }


        public override void ReturnToken() { }

        public override bool RequestToken()
        {
            return default;
        }

        protected override void EnterStartingState()
        {
            if (isNPC)
                SwitchState(new NPCIdleState(this));
            else
                SwitchState(new NPCIdleCombatState(this));
        }

        public override AttackToken GetCurrentAttackToken()
        {
            return default;
        }

#if UNITY_EDITOR

        [Button(ButtonSizes.Medium), GUIColor(.25f, .50f, 0f)]
        public void LoadComponents()
        {
            LoadCommonComponents();
            ForceReceiver.LoadComponents();
        }

        [Button("Enter Companion Idle State"), GUIColor(.25f, .50f, 0f)]
        public void EnterIdleState()
        {
            SwitchState(new NPCIdleCombatState(this));
        }

#endif
    }
}