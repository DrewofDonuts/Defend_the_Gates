using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Etheral
{
    public class AIForceReceiver : ForceReceiver
    {
        [SerializeField] NavMeshAgentController agentController;
        [SerializeField] LayerMask ignoreLayers;

        Vector3 predictImpact;


        protected override void Update()
        {
            // if (!agentController.GetAgent().isOnOffMeshLink)
            // {
            //     SetGravity(false);
            // }

            // IsGravity = !agentController.GetAgent().enabled;
            //
            // if (IsGravity)
                HandleGravity();


            // _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);
            predictImpact = Vector3.SmoothDamp(predictImpact, Vector3.zero, ref _dampingVelocity, _drag);
            _impact = predictImpact;

            if (predictImpact.sqrMagnitude < 0.1f * 0.1f)
            {
                predictImpact = Vector3.zero;
                _impact = Vector3.zero;
                agentController.GetAgent().enabled = true;

                // agentController.SetNextPosition(transform.position);
            }
        }


        public bool AddForceAndCheckIfShouldFallOffLedge(Vector3 force, float checkDistance, bool isLowHealth)
        {
            var isOnLedge = IsLedgeAhead(force, checkDistance);

            if (isOnLedge)
            {
                if (isLowHealth)
                {
                    predictImpact = force * 4f; // Apply predicted impact
                    characterController.detectCollisions = false; // Disable collisions to allow falling off the ledge
                    characterController.excludeLayers = ignoreLayers; // Exclude layers to prevent collisions
                }
                else
                {
                    Debug.Log($"Ledge Ahead, but not low health");
                    predictImpact = Vector3.zero; // Reset predicted impact if ledge is ahead
                }
            }
            else
            {
                predictImpact += force; // Apply predicted impact
            }

            return isOnLedge && isLowHealth;
        }

        public override void AddForce(Vector3 force)
        {
            agentController.DisableAgentComponent();
            predictImpact += force;

            // _impact += force;
        }


#if UNITY_EDITOR

        [Button("Load Components")]
        public new void LoadComponents()
        {
            agentController = GetComponent<NavMeshAgentController>();
            characterController = GetComponent<CharacterController>();
        }
#endif
    }
}