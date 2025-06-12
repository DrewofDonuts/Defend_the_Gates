using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    [CreateAssetMenu(fileName = "EnemyDataList", menuName = "Etheral/Defend The Gates/EnemyDataList", order = 1)]
    public class EnemySpawnDataObject : ScriptableObject
    {
        [SerializeField] List<EnemySpawnData> enemySpawnData = new();
        [SerializeField] bool isGateAttacking = true;
        
        public List<EnemySpawnData> EnemySpawnData => enemySpawnData;
       
        

        void OnValidate()
        {
            //check if the combined ratio of all enemies is 1

            float totalRatio = 0f;
            foreach (var data in enemySpawnData)
            {
                totalRatio += data.spawnRatio;
            }

            if (Mathf.Approximately(totalRatio, 1f)) return;
            
            Debug.LogError($"EnemySpawnDataObject {name}  combined spawn ratio of all enemies must equal 1. Current total ratio: " + totalRatio);

        }
    }
}