namespace Etheral
{
    public class EnemyTimelineState : EnemyBaseState
    {
        public EnemyTimelineState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            aiComponents.GetCC().enabled = false;


            EtheralMessageSystem.OnCondition += OnCondition;
        }

        void OnCondition(string obj)
        {
            if (obj == "SWITCHSTATE")
                enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
        }

        public override void Tick(float deltaTime) { }

        public override void Exit()
        {
            if (stateMachine.ForceReceiver.enabled == false)
                stateMachine.ForceReceiver.enabled = true;
            aiComponents.GetCC().enabled = true;

            // aiComponents.GetNavMeshAgentController().EnableAgentUpdate();
            aiComponents.GetNavMeshAgentController().ResetNavAgent();
            EtheralMessageSystem.OnCondition -= OnCondition;
        }
    }
}