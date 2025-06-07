using Etheral.Combat;
using UnityEngine;

namespace Etheral
{
    public class LifeStealReceiver : MonoBehaviour
    {
        [field: SerializeField] public WeaponDamage RightSword { get; private set; }
        [field: SerializeField] public Health health { get; private set; }
        [field: SerializeField] public SpellHandler SpellHandler { get; private set; }
        [SerializeField] WeaponHandler _weaponHandler;

        public float totalTime;

        void Update()
        {
            totalTime -= Time.deltaTime;

            if (totalTime <= 0)
                gameObject.SetActive(false);
        }

        void OnEnable()
        {
            // totalTime = SpellHandler.LifeSteal.ActiveTime;
            _weaponHandler._currentRightHandDamage.OnLifeSteal += SendLifeStealToHealth;
        }

        void OnDisable()
        {
            RightSword.OnLifeSteal -= SendLifeStealToHealth;
            Debug.Log("Disabled");
        }

        void SendLifeStealToHealth(int healthSteal)
        {
            health.Heal(healthSteal);
        }
    }
}