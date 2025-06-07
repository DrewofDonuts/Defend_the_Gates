namespace Etheral
{
    public class EnemyNPCState : EnemyBaseState
    {
        public EnemyNPCState(EnemyStateMachine _stateMachine) : base(_stateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Locomotion);
            enemyStateMachine.OnChangeStateMethod(StateType.NPC);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
        }

        public override void Exit()
        {
        }
    }
}