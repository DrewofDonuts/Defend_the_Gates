using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class LedgeAngleDetection : MonoBehaviour
    {
        [SerializeField] Vector3 forwardRayOffset;
        [SerializeField] LayerMask ledgeLayers;
        [SerializeField] Transform playerTransform;

        [FormerlySerializedAs("ledgeHeightThreshold")]
        public float angleThreshold;


        void Update()
        {
            Debug.Log($"Is angled? {CheckLedgeAngleInFrontOfPlayer()}");
        }

        public bool CheckLedgeAngleInFrontOfPlayer()
        {
            //Used to check for ledges in front of the player
            var ledgeOrigin = transform.position;

            Debug.DrawRay(ledgeOrigin, transform.forward * 2f, Color.red);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f, ledgeLayers))
            {
                Debug.Log("Ledge hit");
                var angle = Vector3.Angle(playerTransform.forward, hit.normal);
                if (angle > angleThreshold)
                {
                    return true;
                }
            }

            return false;
        }
    }
}