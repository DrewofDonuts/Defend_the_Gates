using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Etheral
{
    public class NavmeshManager : MonoBehaviour
    {
        public float avoidanceTime = 2f;
        public int pathFindingIterations = 100;

        // Update is called once per frame


        void Update()
        {
            if (NavMesh.avoidancePredictionTime != avoidanceTime)
                NavMesh.avoidancePredictionTime = avoidanceTime;

            if (NavMesh.pathfindingIterationsPerFrame != pathFindingIterations)
                NavMesh.pathfindingIterationsPerFrame = pathFindingIterations;
            
            
        }
    }
}