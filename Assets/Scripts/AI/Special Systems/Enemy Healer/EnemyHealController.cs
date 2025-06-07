using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyHealController : MonoBehaviour
    {
        public List<Health> nearbyEnemies = new();


        public bool CanHealEnemies()
        {
            var injuredEnemies = nearbyEnemies.FindAll(enemy => enemy.CurrentHealth <= enemy.MaxHealth * .90f);

            return injuredEnemies.Count >= 1;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Health aiHealth))
            {
                if (aiHealth.Affiliation != Affiliation.Enemy) return;
                if (nearbyEnemies.Contains(aiHealth)) return;
                nearbyEnemies.Add(aiHealth);
                aiHealth.OnDie += RemoveFromList;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Health aiHealth))
            {
                if (aiHealth.Affiliation != Affiliation.Enemy) return;
                if (nearbyEnemies.Contains(aiHealth)) return;
                nearbyEnemies.Add(aiHealth);
                aiHealth.OnDie += RemoveFromList;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Health aiHealth))
            {
                if (aiHealth.Affiliation != Affiliation.Enemy) return;
                if (!nearbyEnemies.Contains(aiHealth)) return;
                nearbyEnemies.Remove(aiHealth);
                aiHealth.OnDie -= RemoveFromList;
            }
        }

        void RemoveFromList(Health health)
        {
            nearbyEnemies.Remove(health);
        }
    }
}