using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class InstantiateRocks : MonoBehaviour
    {
        [SerializeField] GameObject rockPrefab;
        [SerializeField] int minNumberOfRocks = 3;
        [SerializeField] int maxNumberOfRocks = 5;
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip rockSpawnSound;
        [SerializeField] float timeBetweenRocks = .05f;

        [FormerlySerializedAs("chanceToSpawnRocks")]
        [Tooltip("The chance to spawn rocks when the player enters the trigger. 1 = 100% chance, 0 = 0% chance.")]
        [SerializeField] float maxChanceToSpawnRocks = 100;

        bool hasSpawnedRocks;

        void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ClimbController _climbController))
            {
                bool shouldSpawnRocks = Random.Range(0, 100) <= maxChanceToSpawnRocks;
                Debug.Log(_climbController.transform.name);

                if (hasSpawnedRocks) return;

                if (shouldSpawnRocks)
                {
                    Debug.Log("Spawning rocks");
                    InstantiateRocksAtPosition(transform.position);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ClimbController _climbController))
            {
                hasSpawnedRocks = false;
            }
        }

        public void InstantiateRocksAtPosition(Vector3 position)
        {
            int numberOfRocks = Random.Range(minNumberOfRocks, maxNumberOfRocks);
            StartCoroutine(SpawnRocks(numberOfRocks, position));
            if (audioSource != null)
                audioSource.PlayOneShot(rockSpawnSound);
            hasSpawnedRocks = true;
        }

        IEnumerator SpawnRocks(int numberOfRocks, Vector3 position)
        {
            for (int i = 0; i < numberOfRocks; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

                var rock = Instantiate(rockPrefab, position + randomOffset, Quaternion.identity);
                yield return new WaitForSeconds(timeBetweenRocks);
            }
        }


#if UNITY_EDITOR
        [Button("Load Components")]
        public void LoadComponents()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

#endif
    }
}