using System;
using UnityEngine;

namespace Etheral
{
    [RequireComponent(typeof(BoxCollider))]
    public class DoorTrigger : MonoBehaviour
    {
        [field: SerializeField] public float DoorInteractionTime { get; private set; } = 1f;
        [field: SerializeField] public OldDoor OldDoor { get; private set; }



        public bool isRegistered;
        public float doorTimer;
        public bool doorTriggered;
        public bool doorCanTrigger = true;

        void Awake()
        {
            var boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
        
        void Update()
        {
            if (doorTriggered)
                doorTimer += Time.deltaTime;

            if (doorTimer >= DoorInteractionTime)
            {
                doorCanTrigger = true;
                doorTriggered = false;
                doorTimer = 0;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InputReader inputReader))
            {
                if (!isRegistered)
                {
                    inputReader.SouthButtonEvent += HandleDoor;
                    isRegistered = true;
                }
            }
        }

        void HandleDoor()
        {
            isRegistered = true;
            doorTriggered = true;

            if (!doorCanTrigger) return;

            if (!OldDoor.IsDoorIsOpen)
                OldDoor.OpenDoor();
            else
                OldDoor.CloseDoor();

            doorCanTrigger = false;
        }

        void OnTriggerExit(Collider other)
        {
            isRegistered = false;
            if (other.TryGetComponent(out InputReader inputReader))
            {
                inputReader.SouthButtonEvent -= HandleDoor;
            }

            doorCanTrigger = false;
        }
    }
}