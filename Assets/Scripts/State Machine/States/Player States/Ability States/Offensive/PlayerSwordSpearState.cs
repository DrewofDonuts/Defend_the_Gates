
namespace Etheral.Finite_State_Machine.States.Player_States.Ability_States.Offensive
{
    public class PlayerSwordSpearState : PlayerBaseAbilityState
    {
        float forceTime;

        public PlayerSwordSpearState(PlayerStateMachine stateMachine, Ability ability) : base(stateMachine, ability)
        {
        }

        public override void Enter()
        {
            // base.Enter();
        
            stateMachine.WeaponHandler._currentRightHandDamage.SettAttackStatDamage(ability.DirectDamage, ability.KnockBackForce,
                ability.KnockDownForce);
            HasTarget();
        }


        public override void Exit()
        {
            stateMachine.Health.SetSturdy(false);
        }
    }
}