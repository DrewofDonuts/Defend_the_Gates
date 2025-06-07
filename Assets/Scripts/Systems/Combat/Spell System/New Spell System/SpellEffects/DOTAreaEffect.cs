using UnityEngine;

namespace Etheral
{
    public class DOTAreaEffect : BaseEffect
    {
        [Header("Settings")]
        public LayerMask layersToHit;
        [Tooltip("Damage is Damage Per Second")]
        public DamageData damageData = new();


        public override void ApplyOnStay(Collider other)
        {
            if (disable) return;
            if (DoNotHitSelf(other)) return;
            if ((layersToHit & (1 << other.gameObject.layer)) == 0) return;

            if (other.gameObject.TryGetComponent(out ITakeHit takehit))
            {
                if (IsAffiliationSameAsCaster(takehit.Affiliation)) return;

                DamageOverTime(takehit);
            }
        }

        void DamageOverTime(ITakeHit takeHit)
        {
            takeHit.TakeDotDamage(damageData.Damage * Time.deltaTime);
        }
    }
}