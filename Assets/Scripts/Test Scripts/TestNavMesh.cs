using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        GetComponent<NavMeshAgent>().speed = 10f;
        GetComponent<NavMeshAgent>().SetDestination(player.position);
    }
}