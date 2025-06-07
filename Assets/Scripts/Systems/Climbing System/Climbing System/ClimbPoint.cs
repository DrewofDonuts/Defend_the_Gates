using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

// using VHierarchy.Libs;

namespace Etheral
{
    public class ClimbPoint : MonoBehaviour
    {
        [field: SerializeField] public List<Neighbor> neighbors { get; private set; }
        [field: SerializeField] public bool IsTopPoint { get; private set; }
        [field: SerializeField] public bool IsDismountBottomPoint { get; private set; }
        [SerializeField] ClimbPoint climbPointPrefab;

        void Awake()
        {
            // return;
            var twoWayNeighbors = neighbors.Where(n => n.isTwoWay);
            foreach (var neighbor in twoWayNeighbors)
            {
                neighbor.climbPoint?.CreateReturnConnection(this, neighbor.climbDirection,
                    neighbor.connectionType,
                    neighbor.isTwoWay, neighbor.isDifferentPlane);
            }
        }


        void CreateReturnConnection(ClimbPoint _climbPoint, ClimbDirection _climbDirection,
            ConnectionType _neighborConnectionType,
            bool _neighborIsTwoWay = true, bool _isDifferentPlane = false)
        {
            //checks if the neighbor already exists
            if (neighbors.Any(n => n.climbPoint == _climbPoint))
                return;

            ClimbDirection tempClimbDirection = default;

            if (_climbDirection == ClimbDirection.Up)
                tempClimbDirection = ClimbDirection.Down;
            else if (_climbDirection == ClimbDirection.Down)
                tempClimbDirection = ClimbDirection.Up;
            else if (_climbDirection == ClimbDirection.Left)
                tempClimbDirection = ClimbDirection.Right;
            else if (_climbDirection == ClimbDirection.Right)
                tempClimbDirection = ClimbDirection.Left;

            var neighbor = new Neighbor()
            {
                climbPoint = _climbPoint,
                climbDirection = tempClimbDirection,
                connectionType = _neighborConnectionType,
                isTwoWay = _neighborIsTwoWay,
                isDifferentPlane = _isDifferentPlane
            };


            Debug.Log($"Adding neighbor: {neighbor.climbPoint.name} to {name}");
            neighbors.Add(neighbor);
        }

        public Neighbor GetLeapOrSwingConnection()
        {
            return neighbors.FirstOrDefault(n =>
                n.connectionType is ConnectionType.Leap or ConnectionType.SwingToSwing or ConnectionType.SwingToGround
                    or ConnectionType.SwingFromGround);
        }

        public Neighbor GetClimbingLedgeNeighbor(Vector2 direction)
        {
            direction = direction.normalized;
            ClimbDirection _climbDirection = default;

            Neighbor neighbor = null;


            if (Mathf.Abs(direction.y) > 0.5f)
            {
                _climbDirection = (direction.y > 0) ? ClimbDirection.Up : ClimbDirection.Down;
            }
            else if (Mathf.Abs(direction.x) > 0.5f)
            {
                _climbDirection = (direction.x > 0) ? ClimbDirection.Right : ClimbDirection.Left;
            }

            // Return the found neighbor, or null if no neighbor was found
            return neighbors.FirstOrDefault(n => n.climbDirection == _climbDirection);
        }

        void OnDrawGizmosSelected()
        {
            Debug.DrawRay(transform.position, transform.forward, Color.blue);

            foreach (var _neighbor in neighbors)
            {
                if (_neighbor.climbPoint != null)
                    Debug.DrawLine(transform.position, _neighbor.climbPoint.transform.position,
                        (_neighbor.isTwoWay) ? Color.red : Color.gray);
            }
        }


#if UNITY_EDITOR


        [Button("Create Next Climb Point")]
        public void CreateNextClimbPoint()
        {
            var newClimbPoint = Instantiate(climbPointPrefab, transform.parent);
            newClimbPoint.transform.position = transform.position + transform.up * 2;
            newClimbPoint.transform.forward = transform.forward;
            newClimbPoint.transform.parent = transform.parent;

            newClimbPoint.neighbors = new List<Neighbor>();

            neighbors.Add(new Neighbor
            {
                climbPoint = newClimbPoint,
                climbDirection = ClimbDirection.Up,
                isTwoWay = true,
                isDifferentPlane = false,
                connectionType = ConnectionType.DynoJump
            });

            newClimbPoint.neighbors.Add(new Neighbor
            {
                climbPoint = this,
                climbDirection = ClimbDirection.Down,
                isTwoWay = true,
                isDifferentPlane = false,
                connectionType = ConnectionType.DynoJump
            });

            newClimbPoint.CreateConnections();
            CreateConnections();


            UnityEditor.Selection.activeGameObject = newClimbPoint.gameObject;
        }

        [Button("Create Return Connections")]
        public void CreateConnections()
        {
            var twoWayNeighbors = neighbors.Where(n => n.isTwoWay);
            foreach (var neighbor in twoWayNeighbors)
            {
                neighbor.climbPoint?.CreateReturnConnection(this, neighbor.climbDirection,
                    neighbor.connectionType,
                    neighbor.isTwoWay, neighbor.isDifferentPlane);
            }
        }


        public void GetConfigurationFromParentClimbpoint(ClimbPoint parentClimbPoint)
        {
            IsTopPoint = parentClimbPoint.IsTopPoint;
            IsDismountBottomPoint = parentClimbPoint.IsDismountBottomPoint;
            neighbors = parentClimbPoint.neighbors;
            CreateConnections();
        }


#endif
    }


    [Serializable]
    public class Neighbor
    {
        public ClimbPoint climbPoint;

        public ConnectionType connectionType;
        public bool isTwoWay = true;
        public ClimbDirection climbDirection;
        public bool isDifferentPlane;
    }
}