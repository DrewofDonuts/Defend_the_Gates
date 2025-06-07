namespace Etheral
{
    public class EnemyShieldBashState : EnemyBaseState
    {
        // EnemyStateBlocks _enemyStateBlocks;

        // CharacterAction characterAction;
        // bool _alreadyAppliedForce;

        public EnemyShieldBashState(EnemyStateMachine stateMachine, bool isCounterAction = false) : base(stateMachine)
        {
            this.isCounterAction = isCounterAction;
        }

        public override void Enter()
        {
            if (isCounterAction)
                characterAction = enemyStateMachine.AIAttributes.CounterCharacterAction;

            // _enemyStateBlocks = new EnemyStateBlocks(stateMachine);
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();

            enemyStateMachine.Animator.CrossFadeInFixedTime(characterAction.AnimationName,
                characterAction.TransitionDuration);

            enemyStateMachine.GetAIComponents().navMeshAgentController.DisableAgentUpdate();
            enemyStateMachine.Health.SetSturdy(true);

            actionProcessor.SetupActionProcessorForThisAction(enemyStateMachine, characterAction);

            AttackEffects();
            PlayEmote(enemyStateMachine.CharacterAudio.AudioLibrary.AttackEmote, AudioType.attackEmote);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            float normalizedTime = GetNormalizedTime(enemyStateMachine.Animator, characterAction.AnimationName);

            if (normalizedTime >= 1)
            {
                enemyStateBlocks.CheckLocomotionStates();
            }

            // if (normalizedTime >= _characterAction.TimeBeforeForce)
            // {
            //     TryApplyForce();
            // }

            actionProcessor.ApplyForceTimes(normalizedTime);
            actionProcessor.LeftWeaponTimes(normalizedTime);

            if (normalizedTime < characterAction.TimesBeforeForce[0])
                RotateTowardsTargetSmooth(4);
        }

        // protected void TryApplyForce()
        // {
        //     if (_alreadyAppliedForce) return;
        //
        //     _stateMachine.ForceReceiver.AddForce(_stateMachine.transform.forward * characterAction.Force);
        //     _alreadyAppliedForce = true;
        // }

        public override void Exit()
        {
            //Should not enable agent update - that will actually move the enemy with the nav mesh agent
            // enemyStateMachine.GetAIComponents().navMeshAgentController.EnableAgentUpdate();
            enemyStateMachine.Health.SetSturdy(false);
            enemyStateMachine.GetAIComponents().navMeshAgentController.ResetNavAgent();
        }
    }
}