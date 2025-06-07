using System;
using UnityEngine;

namespace Etheral
{
    public class Charger : MonoBehaviour, IAffiliate
    {
        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) { }
        public DamageData damageData;

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
            CheckBroaderHit(other);
            CheckIfHitWallLayer(other);
        }

        void CheckIfHitWallLayer(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                //Play sound
                //Display hit effect
            }
        }

        void CheckBroaderHit(Collider other)
        {
            if (!other.TryGetComponent(out ITakeHit takeHit)) return;
            if (takeHit.Affiliation == Affiliation) return;
            
            takeHit.TakeHit(damageData);
        }
    }
}