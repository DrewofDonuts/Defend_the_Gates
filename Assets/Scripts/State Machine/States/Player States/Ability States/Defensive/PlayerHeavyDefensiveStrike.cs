
namespace Etheral
{
    public class PlayerHeavyDefensiveStrike : PlayerBaseAbilityState
    {
        int maxDamage = 50;
        float damageIncreasePerSecond = 5f;
        float totalAccumulatedDamage;
        float minTimeBeforeHeavyAttack = .25f;

        public PlayerHeavyDefensiveStrike(PlayerStateMachine stateMachine, Ability ability) : base(stateMachine, ability)
        {
        }

        public override void Enter()
        {
            // ability = _stateMachine.SpellHandler.HeavyOffensiveStrike;
            forceTime = ability.ForceTime;
            PassAttackBasedDamage();

            stateMachine.Animator.CrossFadeInFixedTime(ability.Animation, CrossFadeDuration);
            stateMachine.Animator.SetBool("FocusEnergy", true);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            minTimeBeforeHeavyAttack -= deltaTime;

            if (stateMachine.InputReader.IsEastButton && totalAccumulatedDamage < maxDamage)
            {
                HasTarget();
                totalAccumulatedDamage += (damageIncreasePerSecond * deltaTime);
            }

            if (!stateMachine.InputReader.IsEastButton && minTimeBeforeHeavyAttack <= 0)
            {
                HandleHeavyAttack(deltaTime);
            }
            else if (!stateMachine.InputReader.IsEastButton)
                ReturnToLocomotion();

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "HeavyAttack");

            if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
            {
            }
            else
                ReturnToLocomotion();

            previousFrameTime = normalizedTime;
        }

        void HandleHeavyAttack(float deltaTime)
        {
            forceTime -= deltaTime;

            // AbilitySecondaryForce(ability.PreMovementForce);

            if (forceTime <= 0)
            {
                AbilityForce(ability.MovementForce);
            }

            stateMachine.Animator.SetBool("IsBlocking", stateMachine.InputReader.IsBlocking);
            stateMachine.Animator.SetBool("FocusEnergy", false);
        }

        void PassAttackBasedDamage()
        {
        
            stateMachine.WeaponHandler._currentRightHandDamage.SettAttackStatDamage(
                ability.DirectDamage + (int)totalAccumulatedDamage,
                ability.KnockBackForce, ability.KnockDownForce);

        
            // if (_stateMachine.WeaponInventory.RightHandTypeOfWeapon is TypeOfWeapon.OneHandWeapon)
            //     _stateMachine.WeaponHandler.Right_1H_WeaponDamage.SettAttackStatDamage(
            //         ability.DirectDamage + (int)totalAccumulatedDamage,
            //         ability.KnockBackForce, ability.KnockDownForce);
            // else
            //     _stateMachine.WeaponHandler.Right_2H_WeaponDamage.SettAttackStatDamage(
            //         ability.DirectDamage + (int)totalAccumulatedDamage,
            //         ability.KnockBackForce, ability.KnockDownForce);
        }

        public override void Exit()
        {
        }
    }
}