using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    public class WaypointEtheral : MonoBehaviour, ITriggerSuccessOrFailure
    {
        [SerializeField] EtheralEvents waypointEvent;
        [field: SerializeField] public Transform LookAtTarget { get; private set; }

        [SerializeField] bool isLastWaypoint;
        public event Action OnTriggerSuccessEvent;
        public event Action OnTriggerFailedEvent;


        Color gizmoColor;

        public void SetGizmoColor(Color color)
        {
            gizmoColor = color;
        }


        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
 
        

#if UNITY_EDITOR


        public void AddWaypointToGroup()
        {
            var waypointHandler = GetComponentInParent<WaypointsHandler>();
            if (waypointHandler != null)
            {
                waypointHandler.AddWaypoint(this);
            }
        }

        [Button("Create next Wayponit", ButtonSizes.Large)]
        public void CreateNextWaypoint()
        {
            var newWaypoint = Instantiate(this, transform.parent);
            newWaypoint.transform.position = transform.position + transform.forward * 2;
            newWaypoint.transform.rotation = transform.rotation;
            newWaypoint.name = "Waypoint";
            newWaypoint.AddWaypointToGroup();
            newWaypoint.SetGizmoColor(gizmoColor);
            Selection.activeGameObject = newWaypoint.gameObject;
        }

        [Button("Delete Waypoint", ButtonSizes.Small), GUIColor(1f, .25f, .25f)]
        public void DeleteWaypoint()
        {
            var waypointHandler = GetComponentInParent<WaypointsHandler>();
            if (waypointHandler != null)
            {
                waypointHandler.RemoveWaypoint(this);
            }

            DestroyImmediate(gameObject);
        }

#endif
    }
}