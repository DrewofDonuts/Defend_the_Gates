using UnityEngine;

namespace Etheral
{
    public abstract class PlayerBaseAttackState : PlayerBaseState
    {
        protected bool canSwitch;
        

        protected PlayerBaseAttackState(PlayerStateMachine _stateMachine) : base(_stateMachine)
        {
            stateMachine.OnChangeStateMethod(StateType.Attack);
        }

  protected virtual void RegisterEvents()
        {
            stateMachine.InputReader.WeaponAbilityEvent += OnWeaponAbilityDown;
            stateMachine.InputReader.DDownEvent += OnDPadDownDown;
            stateMachine.InputReader.DRightEvent += OnDPadRightDown;
            stateMachine.InputReader.LeftBumperEvent += OnLeftBumperDown;
            stateMachine.InputReader.RightBumperEvent += OnRightBumperDown;
            stateMachine.InputReader.AttackEvent += OnAttackDown;

            stateMachine.InputReader.EastButtonEvent += OnEastButtonDown;
            stateMachine.InputReader.SouthButtonEvent += OnSouthButtonDown;
            stateMachine.InputReader.NorthButtonEvent += OnNorthButtonDown;
            stateMachine.InputReader.WestButtonEvent += OnWestButtonDown;

            stateMachine.InputReader.CancelEvent += OnCancelButtonDown;
            stateMachine.InputReader.TestButtonEvent += OnTestButtonDown;
        }


        protected virtual void DeRegisterEvents()
        {
            stateMachine.InputReader.WeaponAbilityEvent -= OnWeaponAbilityDown;
            stateMachine.InputReader.WestButtonEvent -= OnWestButtonDown;
            stateMachine.InputReader.EastButtonEvent -= OnEastButtonDown;
            stateMachine.InputReader.NorthButtonEvent -= OnNorthButtonDown;
            stateMachine.InputReader.SouthButtonEvent -= OnSouthButtonDown;
            stateMachine.InputReader.AttackEvent -= OnAttackDown;

            stateMachine.InputReader.DDownEvent -= OnDPadDownDown;
            stateMachine.InputReader.DRightEvent -= OnDPadRightDown;
            stateMachine.InputReader.LeftBumperEvent -= OnLeftBumperDown;
            stateMachine.InputReader.RightBumperEvent -= OnRightBumperDown;

            stateMachine.InputReader.CancelEvent -= OnCancelButtonDown;
            stateMachine.InputReader.TestButtonEvent -= OnTestButtonDown;
        }

        protected virtual void OnDPadDownDown()
        {
            playerBlocks.EnterBlessedGroundState();
        }

        protected virtual void OnDPadRightDown()
        {
            playerBlocks.EnterDrinkPotionState();
        }

        protected virtual void OnLeftBumperDown() { }
        

        void OnWeaponAbilityDown()
        {
            if(!canSwitch) return;
            if (stateMachine.WeaponInventory.RightEquippedWeapon.TypeOfWeapon == TypeOfWeapon.OneHandWeapon)
                playerBlocks.EnterHolyChargeState();
        }

        protected virtual void OnAttackDown()
        {
            if(!canSwitch) return;
            
            // stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            playerBlocks.EnterAttackingState();
        }

        protected virtual void OnEastButtonDown()
        {
            if(!canSwitch) return;

            playerBlocks.EnterPurificationState(PlayerComponents.GetCC().velocity.magnitude);
        }

        protected virtual void OnSouthButtonDown()
        {
            if(!canSwitch) return;

            playerBlocks.EnterOffensiveDodgeState();
        }

        protected virtual void OnNorthButtonDown() => playerBlocks.EnterLeapState();


        protected virtual void OnWestButtonDown()
        {
            if(!canSwitch) return;

            Debug.Log("West Button Down");
            playerBlocks.EnterAttackingState();

            // playerBlocks.EnterShieldBashState();
        }

        protected virtual void OnCancelButtonDown() { }

        protected virtual void OnTestButtonDown() { }


        void OnRightBumperDown()
        {
            if (canSwitch)
            {
                if (PlayerComponents.GetAmmoController().HasAmmo())
                    playerBlocks.EnterRangedAttackState();
            }
        }
    }
}