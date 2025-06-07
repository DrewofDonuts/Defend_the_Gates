using UnityEngine;

namespace Etheral
{
    public class PlayerHolyChargeState : PlayerBaseState
    {
        //Movement Builup
        float maxForce;
        float accumulatedForce;
        float AccumulationPerSecond => maxForce * .65f;
        float _drag = 5f;

        //Damage Buildup


        //Movement Control
        Vector3 _dampingVelocity;
        bool isCharging;
        bool canMove;
        Vector3 rotation;

        float totalTime = .1f;
        float buildDamageTimer;
        HolyChargeController holyChargeController;

        public PlayerHolyChargeState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            characterAction = GetCharacterAction(stateMachine.PlayerCharacterAttributes.HolyCharge);
            holyChargeController = stateMachine.PlayerComponents.GetHolyChargeController();

            animationHandler.SetBool(FocusEnergyParameter, true);

            stateMachine.InputReader.CancelEvent += ExecuteCharge;
            animationHandler.CrossFadeInFixedTime(Focus, characterAction.TransitionDuration);

            stateMachine.PlayerComponents.TargetController.ToggleDirectionPointer(true);
            stateMachine.PlayerComponents.GetFeedbackHandler().PlayConstantFeedback(true);
            InitializeHolyChargeController();


            maxForce = characterAction.Forces[0];

            // StartBulletTime();
            isCharging = true;
        }

        void InitializeHolyChargeController()
        {
            holyChargeController.SetMaxDamageAndKnockBack(characterAction.Damage, characterAction.KnockBackForce,
                characterAction.KnockDownForce);
            holyChargeController.StartDamageBuildup();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            var normalizedValue = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);

            //not implemented yet
            BuildChargeForceOverTime(deltaTime);

            if (!canMove)
            {
                // rotation = CalculateMovementAgainstCamera();
                // FaceMovementDirection(rotation, deltaTime);
                
                HandleRotationBasedOnInputType(deltaTime, false, true);

            }

            if (canMove && normalizedValue >= characterAction.TimesBeforeForce[0])
            {
                Vector3 moveDirection = stateMachine.transform.forward * (accumulatedForce * _drag);

                // Lerp the magnitude to zero using drag
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, stateMachine.testFloat * deltaTime);

                Move(moveDirection, deltaTime);
                totalTime -= deltaTime;
            }


            if (totalTime <= 0)
                ReturnToLocomotion();
        }

        void ExecuteCharge()
        {
            if (canMove) return;
            holyChargeController.SetColliderActive(true);

            StartCooldown();
            stateMachine.PlayerComponents.GetFeedbackHandler().PlayConstantFeedback(false);
            stateMachine.PlayerComponents.TargetController.ToggleDirectionPointer(false);
            // EndBulletTime();
            stateMachine.GetCharComponents().GetCC().excludeLayers = stateMachine.LayerToIgnore;

            animationHandler.SetBool(FocusEnergyParameter, false);
            animationHandler.CrossFadeInFixedTime(characterAction);


            AudioProcessor.PlaySingleOneShot(stateMachine.CharacterAudio.WeaponSource,
                characterAction.Audio, AudioType.none);


            canMove = true;
        }

        void BuildChargeForceOverTime(float deltaTime)
        {
            if (isCharging)
            {
                accumulatedForce = Mathf.Min(accumulatedForce + AccumulationPerSecond * deltaTime, maxForce);
            }
        }

        public override void Exit()
        {
            stateMachine.GetCharComponents().GetCC().excludeLayers = default;
            stateMachine.InputReader.CancelEvent -= ExecuteCharge;
            holyChargeController.SetColliderActive(false);
        }
    }
}