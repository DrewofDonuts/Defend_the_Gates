using System;
using UnityEngine;


namespace Etheral
{
    public class DefensesHealth : MonoBehaviour, ITakeHit
    {
        [SerializeField] int maxHealth = 100;

        [Header("Debug")]
        [SerializeField] int currentHealth;
        public Affiliation Affiliation { get; set; }
        public event Action OnDie;
        public bool isHooked { get; set; }

        public void TakeHit(IDamage damage, float angle = default)
        {
            if (currentHealth <= 0)
                return;

            // Apply damage
            currentHealth -= (int)damage.Damage;

            // Check for death
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie?.Invoke();
                Destroy(gameObject);
            }
        }

        public void TakeDotDamage(float damage) { }
    }
}