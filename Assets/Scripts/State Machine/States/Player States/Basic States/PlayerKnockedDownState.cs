using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class PlayerKnockedDownState : PlayerBaseState
    {
        float duration = .50f;
        bool isGetUp;
        bool hasAttemptedFocus;

        readonly int KnockDown = Animator.StringToHash("KnockDown");
        readonly int GetUp = Animator.StringToHash("GetUp");

        public PlayerKnockedDownState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.OnChangeStateMethod(StateType.KnockedDown);
            EventBusPlayerStatesToDeprecate.PlayerSwitchedState(this, StateType.KnockedDown);
            stateMachine.Animator.CrossFadeInFixedTime(KnockDown, CrossFadeDuration);
            stateMachine.Health.SetIsInvulnerable(true);


            stateMachine.Health.OnTakeGroundHit += TakeGroundHit;



            stateMachine.PlayerComponents.GetFocusHandler().StartFocus();
            stateMachine.InputReader.RightBumperEvent += AttemptFocus;

            // StartBulletTime(.2f, .15f);

            // stateMachine.StartCoroutine(EndBulletTimeCoroutine());
        }



        void AttemptFocus()
        {
            if (!stateMachine.PlayerComponents.GetFocusHandler().isActive) return;
            if (stateMachine.PlayerComponents.GetFocusHandler().AttemptFocus() && !hasAttemptedFocus)
            {
                EventBusPlayerController.IsGroundAttacking(stateMachine, false);
                stateMachine.SwitchState(new PlayerGroundDodgeState(stateMachine));

                // EndBulletTime();
            }
        }

        void TakeGroundHit(IDamage damage)
        {
            stateMachine.StartCoroutine(CheckNormalizedTimeThenResumeKnockedDownLoop());
        }

        IEnumerator CheckNormalizedTimeThenResumeKnockedDownLoop()
        {
            var index = Random.Range(0, 3);
            animationHandler.CrossFadeInFixedTime("GroundImpact" + index);

            while (animationHandler.GetNormalizedTime("GroundImpact") < 1)
                yield return null;

            animationHandler.CrossFadeInFixedTime("KnockdownLoop");
        }


        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            duration -= deltaTime;

            if (duration <= 0 && !isGetUp)
            {
                stateMachine.Animator.CrossFadeInFixedTime(GetUp, CrossFadeDuration);
                isGetUp = true;
                stateMachine.Health.SetIsInvulnerable(false);
            }

            if (GetNormalizedTime(stateMachine.Animator, "GetUp") >= 1)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            EndBulletTime();
            
            stateMachine.Health.OnTakeGroundHit -= TakeGroundHit;
            stateMachine.InputReader.RightBumperEvent -= AttemptFocus;
            stateMachine.Health.SetSturdy(false);
        }
    }
}