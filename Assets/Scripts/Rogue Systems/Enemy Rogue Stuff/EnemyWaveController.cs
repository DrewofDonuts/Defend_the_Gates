using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class EnemyWaveController : MessengerClass
    {
        RunSceneData sceneData;
        [SerializeField] EnemySpawner enemySpawner;

        WaveType waveType;
        int numberOfWaves;
        float timeBetweenWaves;
        float timeBetweenPhases;


        [Header("Current States")]
        [ReadOnly]
        public int currentWave;
        [ReadOnly]
        public float waveTimer;

        void Start()
        {
            
            sceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);
            numberOfWaves = sceneData.NumberOfWaves;
            waveType = sceneData.WaveType;
            timeBetweenWaves = sceneData.TimeBetweenWaves;
            timeBetweenPhases = sceneData.TimeBetweenPhases;
            keyToReceive = sceneData.WaveKeyToReceive;
            keyToSend = sceneData.WaveKeyToSend;

            if (enemySpawner == null)
                enemySpawner = GetComponent<EnemySpawner>();
        }


        protected override void HandleReceivingKey()
        {
            base.HandleReceivingKey();
            Debug.Log("Received key");
            HandleNextWave();
        }

        void HandleNextWave()
        {
            if (currentWave < numberOfWaves)
            {
                StartCoroutine(StartWaveEvent());
            }
            else
            {
                Debug.Log("All enemies defeated");

                SendKey();
            }
        }

        IEnumerator StartWaveEvent()
        {
            Debug.Log("Starting wave");
            yield return new WaitForSeconds(timeBetweenPhases);
            currentWave++;
            enemySpawner.SpawnEnemies(-1);
        }

        void Update()
        {
            if (waveType != WaveType.timer) return;
            if (currentWave >= numberOfWaves) return;

            waveTimer += Time.deltaTime;

            if (waveTimer >= timeBetweenWaves)
            {
                enemySpawner.SpawnEnemies();
                waveTimer = 0;
                currentWave++;
            }
        }
    }


    public enum WaveType
    {
        enemyPhases = 0,
        timer = 1
    }
}