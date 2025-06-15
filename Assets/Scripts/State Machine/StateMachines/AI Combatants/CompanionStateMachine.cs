using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using AIStateMachine = Etheral.AIStateMachine;

namespace Etheral
{
    public class CompanionStateMachine : AIStateMachine
    {
        CompanionStateMachineProcessor stateMachineProcessor = new();
        [SerializeField] NPCComponents npcComponents;
        public NPCComponents GetNPCComponents => npcComponents;

        [SerializeField] bool isNPC;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            EnterStartingState();

            if (AITestingControl != null && !AITestingControl.displayStateIndicator && stateIndicator != null)
                stateIndicator.enabled = false;
        }

        protected new void Update()
        {
            base.Update();

            targetCheckTimer += Time.deltaTime;

            if (targetCheckTimer < targetCheckInterval) return;
            if (aiComponents.GetAILockOnController() == null) return;
            if (currentState == null) return;
            currentTarget = GetLockedOnTarget();
            currentState.SetCurrentTarget(currentTarget);

            targetCheckTimer = 0f;
        }

        public override ITargetable GetLockedOnTarget()
        {
            var target = GetAIComponents().GetAILockOnController().GetTarget();

            if (target != null && target.Transform != Target)
                Target = target.Transform;

            // Target = target.GetCharComponents().GetHead().transform;

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
                SwitchState(new CompanionIdleCombatState(this));
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
            SwitchState(new CompanionIdleCombatState(this));
        }

#endif
    }
}