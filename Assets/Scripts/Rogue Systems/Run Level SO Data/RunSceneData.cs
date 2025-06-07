using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [CreateAssetMenu(fileName = "Run Scene Data", menuName = "Etheral/Scene/Rogue Run Scene Data")]
    [InlineEditor]
    public class RunSceneData : ScriptableObject
    {
        [Tooltip("Must match the scene name")]
        [SerializeField] string sceneName;

        void OnValidate()
        {
#if UNITY_EDITOR
            string assetPath = AssetDatabase.GetAssetPath(this);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            sceneName = fileName;

            if (keyToReceive.IsNullOrWhitespace())
                keyToReceive = "ALL_WAVES_COMPLETED";

            if (enemySpawnKeyToSend.IsNullOrWhitespace())
                enemySpawnKeyToSend = "ALL_ENEMIES_KILLED";

            if (waveKeyToReceive.IsNullOrWhitespace())
                waveKeyToReceive = "ALL_ENEMIES_KILLED";

            if (waveKeyToSend.IsNullOrWhitespace())
                waveKeyToSend = "ALL_WAVES_COMPLETED";

#endif
        }

        [TabGroup("Scene Settings", TextColor = "white")]
        [SerializeField] RunSceneData[] sceneList;
        [TabGroup("Scene Settings")]
        [SerializeField] string keyToReceive = "ALL_WAVES_COMPLETED";
        [TabGroup("Scene Settings")]
        [SerializeField] int roomXp = 10;

        [TabGroup("Enemy Spawner", TextColor = "yellow")]
        [Header("Spawn Settings")]
        [SerializeField] string enemySpawnKeyToReceive;
        [TabGroup("Enemy Spawner")]
        [SerializeField] string enemySpawnKeyToSend = "ALL_ENEMIES_KILLED";

        [TabGroup("Enemy Spawner")]
        [Header("Enemy  Data")]
        [SerializeField] EnemySpawnData[] enemySpawnDataList;
        [TabGroup("Enemy Spawner")]
        [SerializeField] int numberOfEnemies = 5; // Number of enemies to spawn
        [TabGroup("Enemy Spawner")]
        [SerializeField] float minSpawnDistance = 2f; // Minimum distance between spawn points
        [TabGroup("Enemy Spawner")]
        [SerializeField] bool spawnOnStart = true;
        [TabGroup("Enemy Spawner")]
        [SerializeField] [MaxValue(1)] float chanceToPatrol;


        [FormerlySerializedAs("bossSpawnData")]
        [TabGroup("Boss Spawner", TextColor = "red")]
        [Header("Boss Data")]
        [SerializeField] EnemyPrefabSO _bossSo;
        [TabGroup("Boss Spawner")]
        [SerializeField] bool spawnBossOnStart = true;
        [TabGroup("Boss Spawner")]
        [SerializeField] string bossKeyToReceive;
        [TabGroup("Boss Spawner")]
        [SerializeField] string bossKeyToSend = "BOSS_CLEARED";


        [TabGroup("Enemy Waves", TextColor = "blue")]
        [SerializeField] WaveType waveType;
        [TabGroup("Enemy Waves")]
        [SerializeField] string waveKeyToReceive = "ALL_ENEMIES_KILLED";
        [TabGroup("Enemy Waves")]
        [SerializeField] string waveKeyToSend = "ALL_WAVES_COMPLETED";
        [TabGroup("Enemy Waves")]
        [SerializeField] int numberOfWaves = 1;
        [TabGroup("Enemy Waves")]
        [Header("Time Settings")]
        [ShowIf("waveType", WaveType.timer)]
        [SerializeField] float timeBetweenWaves = 15f;
        [TabGroup("Enemy Waves")]
        [Header("Wave Phase Settings")]
        [ShowIf("waveType", WaveType.enemyPhases)]
        [SerializeField] float timeBetweenPhases = 2f;

        [TabGroup("Reward Settings", TextColor = "purple")]
        [SerializeField] RewardData[] rewardData;
        [TabGroup("Reward Settings", TextColor = "purple")]
        [SerializeField] bool canSpawnAbilityItems;
        
        
        public string SceneName => sceneName;
        public RunSceneData[] SceneList => sceneList;
        public string KeyToReceive => keyToReceive;
        public EnemySpawnData[] EnemySpawnDataList => enemySpawnDataList;
        public int NumberOfEnemies => numberOfEnemies;
        public float MinSpawnDistance => minSpawnDistance;
        public bool SpawnOnStart => spawnOnStart;
        public float ChanceToPatrol => chanceToPatrol;
        public WaveType WaveType => waveType;
        public int NumberOfWaves => numberOfWaves;
        public float TimeBetweenWaves => timeBetweenWaves;
        public float TimeBetweenPhases => timeBetweenPhases;
        public RewardData[] RewardsData => rewardData;
        public string EnemySpawnKeyToReceive => enemySpawnKeyToReceive;
        public string EnemySpawnKeyToSend => enemySpawnKeyToSend;
        public EnemyPrefabSO BossSO => _bossSo;
        public bool SpawnBossOnStart => spawnBossOnStart;
        public string BossKeyToReceive => bossKeyToReceive;
        public string BossKeyToSend => bossKeyToSend;
        public string WaveKeyToReceive => waveKeyToReceive;
        public string WaveKeyToSend => waveKeyToSend;
        public int RoomXP => roomXp;
        public bool CanSpawnAbilityItems => canSpawnAbilityItems;
    }
}