using System;
using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class SpawnEffects : MonoBehaviour
    {
        [SerializeField] bool isMoveToPosition;
        [Header("Effect Settings")]
        [SerializeField] GameObject spawnEffect;
        [SerializeField] GameObject projectileEffect;


        [Header("Move Settings")]
        [SerializeField] float movementSpeed;
        // [SerializeField] float timeBeforeSpawn = .1f;


        EnemyStateMachine prefabToSpawn;


        AIHealth aiHealth;
        Transform lineOrigination;
        Vector3 spawnPosition;

        bool isLine;
        bool isSpawned;
        bool isEffectSpawned;

        WaitForSeconds waitBeforeSpawn = new(.2f);

        public void Init(EnemyStateMachine _prefab, Transform spawnController, Vector3 enemySpawnPos,
            AIHealth _aiHealth, bool _isLine)
        {
            aiHealth = _aiHealth;
            lineOrigination = spawnController;
            prefabToSpawn = _prefab;
            spawnPosition = isMoveToPosition ? enemySpawnPos : spawnController.position;
            isLine = _isLine;

            if (!isMoveToPosition)
                StartCoroutine(TimeBeforeSpawning());
        }

        void Update()
        {
            //Check if transforms X and Z are near SpawnPosition

            if (spawnPosition == Vector3.zero) return;
            if (!isMoveToPosition) return;
            MoveTowardsSpawnPosition();
            HandleDistanceLogic();
        }

        void MoveTowardsSpawnPosition()
        {
            transform.LookAt(spawnPosition);
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }

        void HandleDistanceLogic()
        {
            if (Vector3.Distance(transform.position, spawnPosition) < 0.2f)
            {
                if (!isEffectSpawned)
                {
                    SpawnEffect();
                    isEffectSpawned = true;
                }

                if (!isSpawned)
                {
                    StartCoroutine(TimeBeforeSpawning());
                    isSpawned = true;
                }
            }
        }

        IEnumerator TimeBeforeSpawning()
        {
            yield return waitBeforeSpawn;
            if (projectileEffect != null)
                projectileEffect.SetActive(false);

            SpawnEnemy();
        }

        void SpawnEffect()
        {
            //Can remove after testing new way to display spawn settings. 
            // var effect = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        }

        void SpawnEnemy()
        {
            var spawnedEnemy = Instantiate(prefabToSpawn, spawnPosition, transform.rotation);

            if (isLine)
            {
                spawnedEnemy.GetAIComponents().GetLineController().SetTarget(aiHealth, lineOrigination);
            }

            Destroy(gameObject);
        }
    }
}