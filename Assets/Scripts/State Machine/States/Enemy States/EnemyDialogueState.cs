namespace Etheral
{
    public class EnemyDialogueState : EnemyBaseState
    {
        public EnemyDialogueState(EnemyStateMachine _stateMachine) : base(_stateMachine)
        {
        }

        public override void Enter()
        {
            animationHandler.CrossFadeInFixedTime(Locomotion);
            enemyStateMachine.OnChangeStateMethod(StateType.Idle);
            enemyStateMachine.Health.SetIsInvulnerable(true);

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