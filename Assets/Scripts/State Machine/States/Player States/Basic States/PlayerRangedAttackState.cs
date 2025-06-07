using UnityEngine;

namespace Etheral
{
    public class PlayerRangedAttackState : PlayerBaseActionState
    {
        string drawAnimation = "Draw";
        string reloadAnimation = "Reload";
        string aimAnimation = "Aim";
        string fire = "Fire";
        float aimTime = .1f;
        float timer;


        bool isAiming;
        bool isDrawing;
        bool hasPlayedFirstReloadAudio;
        bool hasFired;
        bool isRepeatToSkipDraw;
        bool doNotReduceWeight;
        bool hasPlayedSecondReloadAudio;
        bool hasUsedAmmo;
        float aimAccuracyModifier;

        public PlayerRangedAttackState(PlayerStateMachine _stateMachine, bool _isRepeatToSkipDraw = false, float momentum = 0f) : base(_stateMachine)
        {
            isRepeatToSkipDraw = _isRepeatToSkipDraw;
            movementSpeed = momentum;
        }

        public override void Enter()
        {
            characterAction = stateMachine.PlayerCharacterAttributes.RangedAttacks[0];

            aimTime = characterAction.AimTime;
            actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
            if (!isRepeatToSkipDraw)
                animationHandler.CrossFadeInFixedTime(characterAction.PreAnimation);
            else
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName, 0.1f);

            playerBlocks.EquipRangedWeapon();

            // animationHandler.CrossFadeInFixedTime(reloadAnimation, 0.1f);


            animationHandler.SetAnimatorLayer(1, 1);

            PassRangedDamage();

            PlayerComponents.GetRangedWeaponDamage().ToggleAimingArrow(true);
            aimAccuracyModifier = playerStatsController.GetAimAccuracy();
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            // HandleAllLocomotionAndAnimation(deltaTime, true);
            // HandleAllLocomotionAndAnimation(deltaTime, true, lockOverride: false, rotateOverride: true);
            
            HandleRotationBasedOnInputType(deltaTime, false, true);


            var normalizedDrawTime = animationHandler.GetNormalizedTime(characterAction.PreAnimation, 1);
            var normalizedFireTime = animationHandler.GetNormalizedTime(characterAction.AnimationName, 1);

            actionProcessor.FireProjectiles(normalizedFireTime, aimAccuracyModifier);

            if (normalizedFireTime >= characterAction.TimesBeforeProjectile[0] && !hasUsedAmmo)
            {
                PlayerComponents.GetAmmoController().UseAmmo();
                hasUsedAmmo = true;
            }


            HandleFirstDrawAudio(normalizedDrawTime);
            HandleSecondDrawAudio(normalizedFireTime);


            if (normalizedFireTime > characterAction.ComboAttackTime && stateMachine.InputReader.IsRightBumper &&
                PlayerComponents.GetAmmoController().HasAmmo())
            {
                doNotReduceWeight = true;
                playerBlocks.EnterRangedAttackState(true, movementSpeed);
            }
            else if (normalizedFireTime >= 1)
            {
                ReturnToLocomotion(true, movementSpeed);
                return;
            }

            HandleAnimationSwitchForAnimationLayersWithOverride(deltaTime);

            #region Separate pieces
            // var reloadNormalizedTime = animationHandler.GetNormalizedTime(reloadAnimation);
            // var drawNormalizedTime = animationHandler.GetNormalizedTime(drawAnimation);
            // var aimNormalizedTime = animationHandler.GetNormalizedTime(aimAnimation);
            // var fireProjectileNormalizedTime = animationHandler.GetNormalizedTime(fire);
            //
            // actionProcessor.FireProjectiles(fireProjectileNormalizedTime);
            //
            // //reload finishes, so draw weapon back
            // ReloadToDraw(reloadNormalizedTime);
            //
            // //play audio for drawing weapon
            // HandleDrawAudio(drawNormalizedTime);
            //
            // //draw weapon finishes, so aim
            // DrawToAim(drawNormalizedTime);
            //
            // //how long to aim
            // HandleAimTimer(deltaTime);
            //
            // //aiming finishes, so fire projectile
            // AimToFire();
            //
            // if (hasFired &&  fireProjectileNormalizedTime >= characterAction.ComboAttackTime &&
            //     stateMachine.InputReader.IsRangedAttacking)
            // {
            //     playerBlocks.EnterRangedAttackState();
            // }
            // else if (fireProjectileNormalizedTime >= 1f)
            // {
            //     ReturnToLocomotion();
            //     return;
            // }
            #endregion
        }

        public override void Exit()
        {
            animationHandler.CrossFadeInFixedTime("Default1");

            PlayerComponents.GetRangedWeaponDamage().ToggleAimingArrow(false);
            if (isRepeatToSkipDraw) return;
            animationHandler.SetAnimatorLayer(1, 0);
        }


        void PassRangedDamage()
        {
            if (stateMachine.WeaponInventory.RangedEquippedWeapon == null) return;
            stateMachine.WeaponHandler.RangedWeaponDamage.SetAttackStatDamage(characterAction.Damage,
                characterAction.KnockBackForce,
                characterAction.KnockDownForce);
        }

        void AimToFire()
        {
            if (timer >= aimTime && isAiming)
            {
                animationHandler.CrossFadeInFixedTime(fire);
                isAiming = false;
            }
        }

        void HandleAimTimer(float deltaTime)
        {
            if (isAiming)
            {
                timer += deltaTime;
            }
        }

        void DrawToAim(float drawNormalizedTime)
        {
            Debug.Log($"Normalized Time");
            if (drawNormalizedTime >= 1f && !isAiming)
            {
                Debug.Log("Should Draw");
                animationHandler.CrossFadeInFixedTime(aimAnimation);
                isAiming = true;
            }
        }

        void HandleFirstDrawAudio(float drawNormalizedTime)
        {
            if (drawNormalizedTime >= .13f && !hasPlayedFirstReloadAudio)
            {
                stateMachine.CharacterAudio.PlayOneShot(stateMachine.CharacterAudio.RangedDamageSource,
                    stateMachine.WeaponInventory.RangedEquippedWeapon.DrawAudio);

                hasPlayedFirstReloadAudio = true;
            }
        }

        void HandleSecondDrawAudio(float drawNormalizedTime)
        {
            return;
            if (drawNormalizedTime >= characterAction.TimeBeforeDrawAudio && !hasPlayedSecondReloadAudio)
            {
                stateMachine.CharacterAudio.PlayOneShot(stateMachine.CharacterAudio.RangedDamageSource,
                    stateMachine.WeaponInventory.RangedEquippedWeapon.DrawAudio);

                hasPlayedSecondReloadAudio = true;
            }
        }

        void ReloadToDraw(float reloadNormalizedTime)
        {
            if (reloadNormalizedTime >= 1 && !isDrawing)
            {
                animationHandler.CrossFadeInFixedTime(drawAnimation);
                isDrawing = true;
            }
        }
    }
}