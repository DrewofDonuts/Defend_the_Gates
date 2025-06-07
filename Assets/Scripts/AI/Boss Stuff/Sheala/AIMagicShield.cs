using System;
using UnityEngine;

namespace Etheral
{
    public class AIMagicShield : MonoBehaviour, ITakeHit
    {
        [SerializeField] GameObject shield;
        [SerializeField] float MaxHealth;

        float currentHealth;

        public event Action OnDie;
        public bool isHooked { get; set; }

        public Affiliation Affiliation { get; set; }

        void Start()
        {
            currentHealth = MaxHealth;
        }


        public void SetAffiliation(Affiliation _affiliation) { }

        public void TakeHit(IDamage damage, float angle = default)
        {
            currentHealth -= damage.Damage;
            if (currentHealth <= 0)
            {
                shield.SetActive(false);
                OnDie?.Invoke();
            }
        }

        public void TakeDotDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                shield.SetActive(false);
                OnDie?.Invoke();
            }
        }


        public void ResetShield()
        {
            shield.SetActive(true);
        }
    }
}