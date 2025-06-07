using System;
using System.Collections;
using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshClimbWall : MonoBehaviour
{
    public CharacterController characterController;
    public NavMeshAgent agent;
    public Transform destination;
    public Transform model;
    bool isUsingLink;
    bool isSyncAgentToTransform;

    float syncThreshold = 0.4f;
    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }


    void Update()
    {
        isUsingLink = agent.isOnOffMeshLink;

        agent.updatePosition = agent.isOnOffMeshLink;
        agent.SetDestination(destination.position);
        characterController.enabled = !agent.isOnOffMeshLink;

        if (isUsingLink)
        {
            agent.velocity = agent.desiredVelocity;
            agent.speed = agent.speed;
        }
        else
        {
            agent.velocity = characterController.velocity;
            characterController.Move(agent.desiredVelocity * (1f * Time.deltaTime));
        }

        isSyncAgentToTransform = IsAgentOutOfSync();


        Debug.Log($"Is agent on OffMeshLink: {agent.isOnOffMeshLink}");
        Debug.Log($"Is agent out of sync: {isSyncAgentToTransform}");

        model.transform.up = Vector3.up;
    }


    bool IsAgentOutOfSync()
    {
        // Calculate the distance between the agent's position and the transform's position
        float distance = Vector3.Distance(agent.transform.position, transform.position);

        // Return true if the distance exceeds the threshold
        return distance > syncThreshold;
    }

    [Button("Restart Position")]
    public void RestartPosition()
    {
        agent.updatePosition = false;
        characterController.enabled = false;
        agent.Warp(startingPosition);
        transform.position = startingPosition;
        characterController.enabled = true;
    }
}