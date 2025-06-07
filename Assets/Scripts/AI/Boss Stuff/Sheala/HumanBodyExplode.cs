using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class HumanBodyExplode : MonoBehaviour
    {
        public float explosionForce = 100f;
        public float explosionRadius = 10f;

        public Rigidbody[] explodeObjectPrefabs;


        public GameObject bloodExplosionEffect;
        public GameObject[] bloodEffects;


        void Start()
        {
            ExplodeRigidbodyPrefabs();
            Destroy(gameObject, 2.6f);
        }

        void OnDestroy()
        {
            Debug.Log("Human body explosion destroyed");
        }


        [Button("Explode rigidbody prefabs")]
        void ExplodeRigidbodyPrefabs()
        {
            HandleBlood();


            HandleBodies();
        }

        void HandleBodies()
        {
            // foreach (var pool in objectPooler.pools)
            // {
            //     Vector3 randomOffset = new Vector3(Random.Range(-.5f, .5f), 1f, Random.Range(-.5f, .5f));
            //     Rigidbody explodeObject = pool.objectPool.Get().GetComponent<Rigidbody>();
            //     explodeObject.transform.position = transform.position + randomOffset;
            //
            //     explodeObject.gameObject.SetActive(true);
            //     Destroy(explodeObject.gameObject, 2.5f);
            //
            //     ApplyForces(explodeObject);
            // }
            
            foreach (var explodeObjectPrefab in explodeObjectPrefabs)
            {
                // Instantiate objects slightly offset from the explosion center
                Vector3 randomOffset = new Vector3(
                    Random.Range(-.5f, .5f),
                    1f, // Keeping Y offset zero since we want to control it with the explosion force
                    Random.Range(-.5f, .5f));
            
                var explodeObject =
                    Instantiate(explodeObjectPrefab, transform.position + randomOffset, Quaternion.identity);
            
                Destroy(explodeObject.gameObject, 2.5f);
            
                ApplyForces(explodeObject);
            }
        }

        void ApplyForces(Rigidbody explodeObject)
        {
            // Add explosion force with a random outward direction
            Vector3 explosionDirection =
                (explodeObject.transform.position - transform.position).normalized + Vector3.up;
            explodeObject.AddExplosionForce(explosionForce, transform.position,
                explosionRadius);

            // Apply random angular velocity for spinning effect
            explodeObject.angularVelocity = new Vector3(
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f));
        }

        void HandleBlood()
        {
            var explosionEffect = Instantiate(bloodExplosionEffect, transform.position + new Vector3(0, 0f, 0),
                Quaternion.identity);
            Destroy(explosionEffect, 1.5f);

            int bloodCounter = 0;

            while (bloodCounter < 4)
            {
                var randomIndex = Random.Range(0, bloodEffects.Length);
                var blood = Instantiate(bloodEffects[randomIndex], transform.position + new Vector3(0, 1f, 0),
                    Quaternion.identity);

                // Destroy(blood, 2.5f);
                bloodCounter++;
            }
        }
    }
}