using System;
using UnityEngine;
using UnityEngine.AI;

namespace Etheral
{
    public class TestCrossLink : MonoBehaviour
    {
        public CharacterController characterController;
        public NavMeshAgent agent;
        public Transform target;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
        }

        void Update()
        {
            agent.destination = target.position;
            agent.nextPosition = characterController.transform.position;
            characterController.Move(agent.desiredVelocity * Time.deltaTime);
        }
    }
}