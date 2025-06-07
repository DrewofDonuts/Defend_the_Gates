using UnityEngine;

namespace Etheral
{
    public class PlayerHangingState : PlayerBaseState
    {
        ClimbPoint currentPoint;
        float delayTime = 0.25f;

        public PlayerHangingState(PlayerStateMachine playerStateMachine, ClimbPoint currentClimbPoint) : base(
            playerStateMachine)
        {
            currentPoint = currentClimbPoint;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime("HangIdle", 0.2f);
            stateMachine.InputReader.SouthButtonEvent += HandleClimbAction;
        }

        public override void Tick(float deltaTime)
        {
            delayTime -= deltaTime;

            // if (stateMachine.InputReader.MovementValue.magnitude > .5f)
            // {
            //     //if neighbor == default, we cannot move in that direction, unless it's a mount or dismount point
            //     var neighbor = currentPoint.GetClimbingLedgeNeighbor(stateMachine.InputReader.MovementValue);
            //
            //     // if (neighbor != null)
            //     //     Debug.Log($"Neighbor: {neighbor.direction}");
            //
            //     //jump to the next ledge
            //     if (neighbor is { connectionType: ConnectionType.DynoJump } &&
            //         stateMachine.InputReader.IsLeftBumper)
            //     {
            //         stateMachine.SwitchState(new PlayerLedgeJumpingState(stateMachine, neighbor));
            //     }
            //
            //     //shimmy to the next ledge
            //     else if (neighbor is { connectionType: ConnectionType.Shimmy })
            //     {
            //         stateMachine.SwitchState(new PlayerShimmyState(stateMachine, neighbor));
            //     }
            //     //dismount
            //     else if (currentPoint.IsDismountBottomPoint && stateMachine.InputReader.MovementValue.y <= -.5f &&
            //              stateMachine
            //                  .InputReader.IsLeftBumper)
            //     {
            //         stateMachine.SwitchState(new PlayerDismountFromHangState(stateMachine));
            //         return;
            //     }
            //     //mount
            //     else if (currentPoint.IsTopPoint && stateMachine.InputReader.MovementValue.y >= .5f && stateMachine
            //                  .InputReader.IsLeftBumper)
            //     {
            //         stateMachine.SwitchState(new PlayerClimbsOnTopFromHangingState(stateMachine));
            //         return;
            //     }
            // }

            if (delayTime < 1000)
                delayTime = 0;
        }

        void HandleClimbAction()
        {
            if (stateMachine.InputReader.MovementValue.magnitude > .5f)
            {
                //if neighbor == default, we cannot move in that direction, unless it's a mount or dismount point
                var neighbor = currentPoint.GetClimbingLedgeNeighbor(stateMachine.InputReader.MovementValue);

                // if (neighbor != null)
                //     Debug.Log($"Neighbor: {neighbor.direction}");

                //jump to the next ledge
                if (neighbor is { connectionType: ConnectionType.DynoJump } &&
                    stateMachine.InputReader.IsSouthButton)
                {
                    stateMachine.SwitchState(new PlayerLedgeJumpingState(stateMachine, neighbor));
                }

                //shimmy to the next ledge
                else if (neighbor is { connectionType: ConnectionType.Shimmy })
                {
                    stateMachine.SwitchState(new PlayerShimmyState(stateMachine, neighbor));
                }

                //dismount
                else if (currentPoint.IsDismountBottomPoint && stateMachine.InputReader.MovementValue.y <= -.5f &&
                         stateMachine
                             .InputReader.IsSouthButton)
                {
                    stateMachine.SwitchState(new PlayerDismountFromHangState(stateMachine));
                    return;
                }

                //mount
                else if (currentPoint.IsTopPoint && stateMachine.InputReader.MovementValue.y >= .5f && stateMachine
                             .InputReader.IsSouthButton)
                {
                    stateMachine.SwitchState(new PlayerClimbsOnTopFromHangingState(stateMachine));
                    return;
                }
            }
        }

        public override void Exit()
        {
            stateMachine.InputReader.SouthButtonEvent -= HandleClimbAction;
        }
    }
}