using UnityEngine;

namespace Etheral
{
    public abstract class PlayerBaseActionState : PlayerBaseState
    {
        protected bool canSwitch = true;
        protected PlayerBaseActionState(PlayerStateMachine _stateMachine) : base(_stateMachine) { }

        protected virtual void RegisterEvents()
        {
            stateMachine.InputReader.WeaponAbilityEvent += OnWeaponAbilityDown;
            stateMachine.InputReader.DDownEvent += OnDPadDownDown;
            stateMachine.InputReader.DRightEvent += OnDPadRightDown;
            stateMachine.InputReader.LeftBumperEvent += OnLeftBumperDown;
            stateMachine.InputReader.RightBumperEvent += OnRightBumperDown;
            stateMachine.InputReader.AttackEvent += OnAttackDown;

            // stateMachine.InputReader.EastButtonEvent += OnEastButtonDown;
            stateMachine.InputReader.SouthButtonEvent += OnSouthButtonDown;
            stateMachine.InputReader.NorthButtonEvent += OnNorthButtonDown;

            // stateMachine.InputReader.WestButtonEvent += OnWestButtonDown;

            stateMachine.InputReader.CancelEvent += OnCancelButtonDown;
            stateMachine.InputReader.TestButtonEvent += OnTestButtonDown;
            stateMachine.InputReader.OnTowerModeEvent += OnTowerModeDown;
        }

        void OnTowerModeDown()
        {
            //Disabling for Testing - 06/12/2025
            return;
            Debug.Log("Tower Mode Down");
            if (stateMachine.isThirdPerson)
            {
                stateMachine.ChangePerspective(false);
            }
            else
            {
                stateMachine.ChangePerspective(true);
            }
        }


        protected virtual void DeRegisterEvents()
        {
            stateMachine.InputReader.WeaponAbilityEvent -= OnWeaponAbilityDown;

            // stateMachine.InputReader.WestButtonEvent -= OnWestButtonDown;
            // stateMachine.InputReader.EastButtonEvent -= OnEastButtonDown;
            stateMachine.InputReader.NorthButtonEvent -= OnNorthButtonDown;
            stateMachine.InputReader.SouthButtonEvent -= OnSouthButtonDown;
            stateMachine.InputReader.AttackEvent -= OnAttackDown;

            stateMachine.InputReader.DDownEvent -= OnDPadDownDown;
            stateMachine.InputReader.DRightEvent -= OnDPadRightDown;
            stateMachine.InputReader.LeftBumperEvent -= OnLeftBumperDown;
            stateMachine.InputReader.RightBumperEvent -= OnRightBumperDown;

            stateMachine.InputReader.CancelEvent -= OnCancelButtonDown;
            stateMachine.InputReader.TestButtonEvent -= OnTestButtonDown;
            stateMachine.InputReader.OnTowerModeEvent -= OnTowerModeDown;
        }

        protected virtual void OnDPadDownDown()
        {
            playerBlocks.EnterBlessedGroundState();
        }

        protected virtual void OnDPadRightDown()
        {
            playerBlocks.EnterDrinkPotionState();
        }

        protected virtual void OnLeftBumperDown()
        {
            Debug.Log("Left Bumper Down");

            // HandleInitialClimbing();

            if (stateMachine.PlayerComponents.LockOnController.GetCurrentTarget() == null)
                return;

            var enemyStateMachine = PlayerComponents.LockOnController.GetCurrentTarget()
                .GetStateMachine<EnemyStateMachine>();

            if (PlayerComponents.GetExecutionController().CheckIfCanExecute(enemyStateMachine, stateMachine))
                stateMachine.SwitchState(new PlayerExecutingState(stateMachine, enemyStateMachine));
        }

        protected virtual void OnRightBumperDown()
        {
            if (PlayerComponents.GetAmmoController().HasAmmo())
                playerBlocks.EnterRangedAttackState();

            // playerBlocks.EnterCleaningStrikeState();
        }

        void OnWeaponAbilityDown()
        {
            if (stateMachine.WeaponInventory.RightEquippedWeapon.TypeOfWeapon == TypeOfWeapon.OneHandWeapon)
                playerBlocks.EnterCrusade();

            // playerBlocks.EnterHolyChargeState();
        }

        protected virtual void OnAttackDown()
        {
            // stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            playerBlocks.EnterAttackingState();
        }

        protected virtual void OnEastButtonDown()
        {
            playerBlocks.EnterPurificationState(PlayerComponents.GetCC().velocity.magnitude);
        }

        protected virtual void OnSouthButtonDown()
        {
            if (PlayerComponents.GetInteractor().TryUse())
                return;

            if (HandleInitialClimbing())
                return;

            if (stateMachine.StateType == StateType.Climbing)
                return;
            playerBlocks.EnterOffensiveDodgeState();
        }

        //This is temporary - future will split abilities by character
        protected virtual void OnNorthButtonDown()
        {
            var abilityType = playerStatsController.GetCurrentAbility();

            switch (abilityType)
            {
                case PlayerAbilityTypes.Leap:
                    playerBlocks.EnterLeapState();
                    break;
                case PlayerAbilityTypes.Purification:
                    playerBlocks.EnterPurificationState();
                    break;
                case PlayerAbilityTypes.HolyCharge:
                    playerBlocks.EnterHolyChargeState();
                    break;
                case PlayerAbilityTypes.BlessedGround:
                    playerBlocks.EnterBlessedGroundState();
                    break;
                case PlayerAbilityTypes.HolyShield:
                    playerBlocks.EnterHolyShieldState();
                    break;
            }
        }


        protected virtual void OnWestButtonDown()
        {
            Debug.Log("West Button Down");
            playerBlocks.EnterAttackingState();

            // playerBlocks.EnterShieldBashState();
        }

        protected virtual void OnCancelButtonDown() { }

        protected virtual void OnTestButtonDown() { }


        protected bool HandleInitialClimbing()
        {
            if (stateMachine.PlayerComponents.GetClimbController().LeapOrSwingCheck(out var leapHit))
            {
                var hitClimbPoint = leapHit.transform.GetComponent<ClimbPoint>();
                var neighbor = hitClimbPoint.GetLeapOrSwingConnection();

                if (neighbor != null)
                {
                    if (neighbor.connectionType == ConnectionType.Leap)
                        stateMachine.SwitchState(new PlayerJumpToLedgeFromGroundState(stateMachine, hitClimbPoint));

                    if (neighbor.connectionType == ConnectionType.SwingFromGround)
                        stateMachine.SwitchState(new PlayerJumpToSwingState(stateMachine, hitClimbPoint));
                }

                return true;
            }

            if (stateMachine.PlayerComponents.GetClimbController().ClimbDownCheck(out var climbPointHit,
                    out var climbDownPoint))
            {
                stateMachine.SwitchState(new PlayerStandToHangState(stateMachine, climbPointHit, climbDownPoint));
                return true;
            }

            // return;
            if (stateMachine.PlayerComponents.GetClimbController().ClimbLedgeCheck(stateMachine.transform.forward,
                    out var ledgeData, out ClimbPoint climbUpPoint))
            {
                Debug.Log($"Should be climbing ledge {climbUpPoint.name}");
                stateMachine.SwitchState(new PlayerGroundToLedgeState(stateMachine, ledgeData, climbUpPoint));
                return true;
            }

            var parkourAction = stateMachine.PlayerComponents.GetParkourController().CheckIfPossible();
            if (parkourAction.animName != null)
            {
                stateMachine.SwitchState(new PlayerParkourState(stateMachine, parkourAction));
                return true;
            }

            return false;
        }
    }
}