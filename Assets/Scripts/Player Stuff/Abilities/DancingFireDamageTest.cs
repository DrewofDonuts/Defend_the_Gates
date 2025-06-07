using System.Collections.Generic;
using UnityEngine;


namespace Etheral
{
    public class DancingFireDamageTest : MonoBehaviour, IAffiliate
    {
        [SerializeField] DamageData damageData;
        public Affiliation Affiliation { get; set; }

        List<ITakeHit> hitTargets = new List<ITakeHit>();

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITakeHit takeHit))
            {
                if (hitTargets.Contains(takeHit))
                    return;
                takeHit.TakeHit(damageData);
                hitTargets.Add(takeHit);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out ITakeHit takeHit))
            {
                DamageOverTime(takeHit);
            }
        }

        void DamageOverTime(ITakeHit takeHit)
        {
            takeHit.TakeDotDamage(damageData.Damage * Time.deltaTime);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ITakeHit takeHit))
            {
                if (hitTargets.Contains(takeHit))
                    hitTargets.Remove(takeHit);
            }
        }
    }
}