using System;
using UnityEngine;


namespace Etheral
{
    public class HitBox : TriggerSuccessOrFailureMonoBehavior, ITakeHit
    {
        public Health health;
        public Affiliation Affiliation { get; set; }
        
        

        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;

        public event Action OnDie;
        public bool isHooked { get; set; }


        public void TakeHit(IDamage damage, float angle)
        {
            health.TakeHit(damage, angle);
        }

        public void TakeDotDamage(float damage)
        {
            health.TakeDot(damage);
        }

    }
}