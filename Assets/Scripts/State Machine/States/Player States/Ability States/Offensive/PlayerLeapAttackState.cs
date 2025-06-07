using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class PlayerLeapAttackState : PlayerBaseAttackState
    {
        bool canLeap;
        bool alreadyLeaped;
        bool isSpellCast;

        // CharacterAction _characterAction;
        Vector3 moveDirection;
        Vector3 jumpLocation;
        float normalizedTime;

        public PlayerLeapAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            characterAction = stateMachine.PlayerCharacterAttributes.Leap;
        }

        public override void Enter()
        {
            stateMachine.InputReader.CancelEvent += Leap;
            stateMachine.PlayerComponents.TargetController.EnableTargetter();

            if (GetInputType() is InputType.gamePad)
                stateMachine.PlayerComponents.GetFeedbackHandler().PlayConstantFeedback(true);

            stateMachine.Health.SetSturdy(true);
            stateMachine.SetCharacterControllerCollisionLayer(true);

            // StartBulletTime(.2f, .25f);

            animationHandler.SetBool(FocusEnergyParameter, true);
            animationHandler.CrossFadeInFixedTime(Focus, characterAction.TransitionDuration);
            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            RegisterEvents();
        }

        public override void Tick(float deltaTime)
        {
            // Move(deltaTime);
             normalizedTime = GetNormalizedTime(stateMachine.Animator, characterAction.AnimationName);


            //Applies the effects and damage of the spell
            if (normalizedTime >= characterAction.TimesBeforeSpells[0] && !isSpellCast)
            {
                //THIS IS WHERE WE WILL CAST THE DAMAGE ON COLLISION WITH AUDIO

                if (GetInputType() is InputType.gamePad)
                    PlayerComponents.GetFeedbackHandler().HandleHapticInput(FeedbackType.Heavy);

                PlayerComponents.GetFeedbackHandler().HandleCameraShake(FeedbackType.Heavy);
                actionProcessor.CastSpellWithoutNormalizedValue();

                // stateMachine.PlayerComponents.GetCharacterCollision().enabled = true;

                stateMachine.SetCharacterControllerCollisionLayer(false);
                stateMachine.Health.SetIsInvulnerable(false);
                isSpellCast = true;
            }

            canSwitch = normalizedTime >= .5f;
            
            if (canLeap && !alreadyLeaped)
                MoveToLeapTarget(deltaTime);

            HandleReturnToLocomotion();
            if (Vector3.Distance(stateMachine.transform.position, jumpLocation) < .2f)
            {
                stateMachine.PlayerComponents.TargetController.DisableTargetter();
                alreadyLeaped = true;
            }
        }


        void HandleReturnToLocomotion()
        {
            if (normalizedTime >= 1f)
                ReturnToLocomotion();

            if (normalizedTime >= .85f && stateMachine.InputReader.MovementValue.magnitude > 0)
            {
                ReturnToLocomotion();
                return;
            }
        }


        public void Leap()
        {
            if (canLeap) return;

            stateMachine.Health.SetIsInvulnerable(true);

            // EventBusEnemyController.IgnorePlayerCollision(this, true);
            // SetCharacterControllerCollisionLayer(true);

            stateMachine.SetCharacterControllerCollisionLayer(true);


            // stateMachine.PlayerComponents.GetCharacterCollision();

            StartCooldown();

            // EndBulletTime();
            stateMachine.PlayerComponents.GetFeedbackHandler().PlayConstantFeedback(false);

            // stateMachine.GetCharComponents().GetCC().excludeLayers = stateMachine.LayerToIgnore;
            stateMachine.CharacterAudio.PlayRandomEmote(stateMachine.CharacterAudio.AudioLibrary.AttackEmote);
            stateMachine.PlayerComponents.TargetController.DisableTargetter();
            jumpLocation = stateMachine.PlayerComponents.TargetController.GetHitLocation();

            animationHandler.SetBool(FocusEnergyParameter, false);

            // stateMachine.Animator.CrossFadeInFixedTime(characterAction.AnimationName, 0.1f);
            animationHandler.CrossFadeInFixedTime(characterAction);
            canLeap = true;
        }


        public void MoveToLeapTarget(float deltaTime)
        {
            var target = jumpLocation;
            Vector3 direction = target - stateMachine.transform.position;
            Vector3 movement = direction * Mathf.MoveTowards(2, characterAction.Forces[0], .75f);


            RotateTowardsTargetter(deltaTime);
            stateMachine.StartCoroutine(TimeBeforeLeap(movement, deltaTime));

            // Move(movement, deltaTime);
        }

        IEnumerator TimeBeforeLeap(Vector3 movement, float deltaTime)
        {
            yield return new WaitForSeconds(.05f);
            stateMachine.Animator.SetBool(FocusEnergyParameter, false);
            Move(movement, deltaTime);
        }

        void RotateTowardsTargetter(float deltaTime)
        {
            var lookPos = jumpLocation - stateMachine.transform.position;
            lookPos.y = 0f;

            var targetRotation = Quaternion.LookRotation(lookPos);

            stateMachine.transform.rotation =
                Quaternion.Lerp(stateMachine.transform.rotation, targetRotation, 10000 * deltaTime);
        }

        public override void Exit()
        {
            stateMachine.Health.SetIsInvulnerable(false);
            stateMachine.Health.SetSturdy(false);
            stateMachine.GetCharComponents().GetCC().excludeLayers = default;
            stateMachine.PlayerComponents.TargetController.DisableTargetter();
            stateMachine.InputReader.CancelEvent -= Leap;
            DeRegisterEvents();
        }
    }
}