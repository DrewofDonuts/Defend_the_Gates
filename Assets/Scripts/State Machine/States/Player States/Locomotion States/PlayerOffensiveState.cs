using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Etheral
{
    public class PlayerOffensiveState : PlayerBaseActionState
    {
        float timer;

        // Vector3 direction;
        bool isIdle;

        //constructor that takes in the stateMachine and uses the Base State's constructor since we don't do anything new
        public PlayerOffensiveState(PlayerStateMachine stateMachine, bool hasMomentum = false,
            float momentum = default) : base(stateMachine) { }

        public override void Enter()
        {
            if(stateMachine.isThirdPerson)
            PlayerComponents.GetCameraHandler().SetOffensiveCamera();

            stateMachine.OnChangeStateMethod(StateType.Move);
            if (!stateMachine.ForceReceiver.IsGravity)
                stateMachine.ForceReceiver.ToggleGravity(true);

            stateMachine.stateIndicator.color = Color.white;


            RegisterEvents();

            //Disabled to test goign into defensive animation for now
            animationHandler.CrossFadeInFixedTime("FreeLookBlendTree");
        }


        // ReSharper disable Unity.PerformanceAnalysis
        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if (DialogueManager.isConversationActive) return;
            playerBlocks.EnterDefenseState();

            HandleAllLocomotionAndAnimation(deltaTime);
            

            HasTarget();


            if (movementSpeed <= 0)
            {
                stateMachine.SwitchState(new PlayerIdleState(stateMachine));
                return;
            }
        }


        public override void Exit()
        {
            DeRegisterEvents();
        }

        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            stateMachine.InputReader.SprintActionEvent += OnSprintAction;

            // stateMachine.InputReader.EastButtonEvent += OnEastButtonDown;
        }

        protected override void DeRegisterEvents()
        {
            base.DeRegisterEvents();
            stateMachine.InputReader.SprintActionEvent -= OnSprintAction;

            // stateMachine.InputReader.EastButtonEvent -= OnEastButtonDown;
        }

        void OnSprintAction()
        {
            playerBlocks.EnterSprintAttackState();
        }


        #region Bumpers and D Pad
        // protected override void OnLeftBumperDown()
        // {
        //     Debug.Log("Left Bumper Down");
        //
        //     HandleInitialClimbing();
        //
        //     // playerBlocks.EnterGroundExecutionState();
        // }
        //
        #endregion


        #region Compass Buttons
        protected override void OnSouthButtonDown()
        {
            if (stateMachine.InputReader.MovementValue == Vector2.zero) return;
            if (stateMachine.isWalking) return;

            if (stateMachine.InputReader.CanDodge)
                playerBlocks.EnterOffensiveDodgeState();
        }
        #endregion
    }
}