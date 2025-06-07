using System;
using UnityEngine;

namespace Etheral
{
    public class HolyShield : MonoBehaviour, ITakeHit
    {
        [field: SerializeField] public float ShieldHealth { get; private set; } = 30f;
        [SerializeField] LayerMask layersToCollideWith;
        [field: SerializeField] public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation affiliation) => Affiliation = affiliation;


        public event Action OnDie;
        public bool isHooked { get; set; }

        void Start()
        {
            Destroy(gameObject, 6);

            GetComponent<Collider>().includeLayers = layersToCollideWith;
        }

        public void DamageModifier(int damage)
        {
            throw new System.NotImplementedException();
        }


        public void TakeHit(IDamage damage, float angle = 0)
        {
            ShieldHealth -= damage.Damage;

            if (ShieldHealth <= 0)
                HandleDeath();
        }

        public void TakeDotDamage(float damage)
        {
            ShieldHealth -= damage;
            
            if (ShieldHealth <= 0)
                HandleDeath();
        }


        void HandleDeath()
        {
            Destroy(gameObject);
        }

        public bool IsBlocking { get; }
        public bool IsDodging { get; }

        public void AddForce(Vector3 force) { }
    }
}