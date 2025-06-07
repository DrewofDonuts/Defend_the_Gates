using UnityEngine;

namespace Etheral
{
    public class NpcFollowState : NPCBaseState
    {
        float targetSpeed;
        Coroutine decelerationCoroutine;
        float followRadius = 2.5f;

        bool shouldFollow;
        bool shouldAttack;
        float previousDistanceToPlayer;
        Vector3 previousTargetPosition;


        public NpcFollowState(NPCStateMachine npcStateMachine) : base(npcStateMachine) { }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(State.Locomotion, CrossFadeDuration);
            stateMachine.GetPlayer().OnStateChange += HandleStateChange;
            shouldFollow = true;
            animationHandler.SetFloatWithDampTime(ForwardSpeed, 1);
            stateMachine.SetHostile(false);
        }

        void HandleStateChange(StateType newstatetype)
        {
            if (newstatetype == StateType.Idle)
                shouldFollow = false;

            if (newstatetype == StateType.Attack & IsInMeleeRange())
            {
                stateMachine.SetHostile(true);
            }
        }


        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsPlayer();


            if (stateMachine.GetHostile() && IsInMeleeRange() && !stateMachine.AITestingControl.blockAttack)
            {
                Debug.Log("Switching to attack");
                stateMachine.SwitchState(new NPCAttackState(stateMachine));
                return;
            }

            if (stateMachine.GetHostile() && IsInChaseRangeTarget() && shouldAttack &&
                !stateMachine.AITestingControl.blockAttack)
            {
                stateMachine.SwitchState(new NPCChaseState(stateMachine));
                return;
            }

            var isFacingEachOther = stateMachine.AreGameObjectsFacingEachOther(stateMachine.GetPlayer().transform,
                stateMachine.transform, stateMachine.testFloat);


            var currentDistanceToPlayer = GetPlayerStuff(out var currentTargetPosition, out var playerVelocity);


            if (shouldFollow && !isFacingEachOther)
            {
                previousTargetPosition = currentTargetPosition;

                Move(currentTargetPosition, playerVelocity, deltaTime);
                animationHandler.SetFloatWithDampTime(ForwardSpeed,
                    stateMachine.GetCharComponents().GetCC().velocity.magnitude, .2f,
                    deltaTime);
            }
            else if (shouldFollow && isFacingEachOther)
            {
                stateMachine.SwitchState(new NPCIdleCombatState(stateMachine));
                return;
            }
            else if ((!shouldFollow || isFacingEachOther))
            {
                stateMachine.SwitchState(new NPCIdleCombatState(stateMachine));
                return;
            }

            previousDistanceToPlayer = currentDistanceToPlayer;
        }

        float GetPlayerStuff(out Vector3 currentTargetPosition, out float playerVelocity)
        {
            float currentDistanceToPlayer = GetPlayerDistance();
            Vector3 playerPosition = stateMachine.GetPlayerPosition();
            Vector3 directionToPlayer = (playerPosition - stateMachine.transform.position).normalized;
            currentTargetPosition = playerPosition - directionToPlayer * followRadius;
            playerVelocity = stateMachine.GetPlayer().GetCharComponents().GetCC().velocity.magnitude;
            return currentDistanceToPlayer;
        }

        public override void Exit()
        {
            stateMachine.GetPlayer().OnStateChange -= HandleStateChange;
            if (decelerationCoroutine != null)
            {
                stateMachine.StopCoroutine(decelerationCoroutine);
            }

            stateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }
    }
}