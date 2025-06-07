using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class RewardSpawner : MessengerClass
    {
        [SerializeField] bool spawnOnStart;
        [SerializeField] RewardObject rewardSpawnerPrefab;
        
        RunSceneData sceneData;

        [ReadOnly]
        void Start()
        {
            if (spawnOnStart)
                StartCoroutine(SpawnReward());
            
            sceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);
            keyToReceive = sceneData.KeyToReceive;
        }


        protected override void HandleReceivingKey()
        {
            base.HandleReceivingKey();
            EventBusPlayerController.PlayerGetsXP(this, CalculateLevelXP());
            OnEnemyDeath();
        }


        void OnEnemyDeath()
        {
            if(sceneData.RewardsData.Length == 0)
                return;
            StartCoroutine(SpawnReward());
        }

        IEnumerator SpawnReward()
        {
            yield return new WaitForSeconds(1.5f);
            Instantiate(rewardSpawnerPrefab, transform.position, Quaternion.identity);
        }
        
        int CalculateLevelXP()
        {
            return sceneData.RoomXP;
        }
    }
}