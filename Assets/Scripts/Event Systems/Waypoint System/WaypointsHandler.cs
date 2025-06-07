using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    public class WaypointsHandler : MonoBehaviour
    {
        [field: InlineButton("CreateEventKey", "New")]
        [field: SerializeField] public EventKey EventKey { get; private set; }
        [SerializeField] List<WaypointEtheral> waypoints;
        [SerializeField] WaypointEtheral waypointPrefab;
        [ SerializeField] Color gizmoColor  = Color.blue;

        public List<Transform> GetWaypoints()
        {
            List<Transform> waypointTransforms = new List<Transform>();

            for (int i = 0; i < waypoints.Count; i++)
            {
                waypointTransforms.Add(waypoints[i].transform);
            }

            return waypointTransforms;
        }

        public List<WaypointEtheral> GetWaypointsList()
        {
            return waypoints;
        }


        IEnumerator Start()
        {
            yield return new WaitUntil(() => EventManager.Instance);
            if (EventKey != null)
                EventManager.Instance.Register(this);
        }

        void OnDisable()
        {
            if (EventKey != null)
                EventManager.Instance.Unregister(this);
        }

        void OnDrawGizmos()
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawSphere(waypoints[i].transform.position, 0.3f);

                if (i < waypoints.Count - 1)
                {
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                }
            }
        }


#if UNITY_EDITOR

        public void AddWaypoint(WaypointEtheral waypoint)
        {
            if (waypoints.Contains(waypoint)) return;
            waypoints.Add(waypoint);
        }

        public void RemoveWaypoint(WaypointEtheral waypoint)
        {
            waypoints.Remove(waypoint);
        }

        [ShowIf("@EventKey == null")]
        [Button("There's an Empty Trigger!", ButtonSizes.Small), GUIColor(1f, .25f, .25f)]
        void EmptyTrigger()
        {
        }

        public void CreateEventKey()
        {
            EventKey = AssetCreator.NewEventKey();
        }


        [Button("Create Waypoint", ButtonSizes.Large)]
        public void CreateWaypoint()
        {
            var newWaypoint = Instantiate(waypointPrefab, transform);
            newWaypoint.name = "Waypoint";
            newWaypoint.SetGizmoColor(gizmoColor);

            if (waypoints.Count > 0)
            {
                newWaypoint.transform.position = waypoints[waypoints.Count - 1].transform.forward * 2 +
                                                 waypoints[waypoints.Count - 1].transform.position;
                newWaypoint.transform.rotation = waypoints[waypoints.Count - 1].transform.rotation;
            }

            if (waypoints.Count == 0)
            {
                var transform1 = newWaypoint.transform;
                transform1.position = transform.position;
                transform1.rotation = transform.rotation;
            }

            waypoints.Add(newWaypoint);
        }


        [Button("Clear Waypoints", ButtonSizes.Large)]
        public void ClearWaypoints()
        {
            foreach (var waypoint in waypoints)
            {
                DestroyImmediate(waypoint.gameObject);
            }

            waypoints.Clear();
        }

        [Button("Set waypoint Gizmo Color", ButtonSizes.Small)]
        public void SetGizmoColor()
        {
            foreach (var waypoint in waypoints)
            {
                waypoint.SetGizmoColor(gizmoColor);
            }
        }


#endif
    }
}