using System;
using System.Collections.Generic;
using Etheral;
using UnityEngine;

namespace Etheral
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemySpawnLocation : MonoBehaviour
    {
        bool isSomethingInTrigger;
        public bool isSpawned;

        List<Collider> colliders = new();

        Renderer objectRenderer;


        void Start()
        {
            objectRenderer = GetComponent<Renderer>();
        }


        public void SetIsSpawned(bool spawned)
        {
            isSpawned = spawned;
        }

        public bool CheckIfSpaceIsAvailable(out Vector3 position)
        {
            position = transform.position;

            return true;
            // return colliders.Count == 0 && !isSpawned && !CameraUtil.IsVisibleFrom(Camera.main, objectRenderer);
        }

        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<StateMachine>(out var stateMachine))
            {
                if (!colliders.Contains(other))
                {
                    colliders.Add(other);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<StateMachine>(out var stateMachine))
            {
                if (colliders.Contains(other))
                {
                    colliders.Remove(other);
                }
            }
        }
    }
}