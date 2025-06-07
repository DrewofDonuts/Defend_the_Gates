using System.Collections;
using UnityEngine;

namespace Etheral
{
    [SerializeField]
    public class DOTOnHitEffect : BaseEffect
    {
        [field: Header("Damage Over Time Settings")]
        public float timeBetweenTicks = .5f;
        public float duration = 5f;
        
        [Header("IDamage Settings")]
        public DamageData damageData = new();

        [Header("Effect Prefabs")]
        public GameObject affectedTargetPrefab;
        public CollisionConfig collisionEffect;

        bool isCollided;
        

        public override void ApplyOnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ITakeHit takeHit))
            {
                HandleHittingTarget(other, takeHit);
            }
        }

        void HandleHittingTarget(Collider other, ITakeHit takeHit)
        {
            if (takeHit.Affiliation == affiliation) return;
            if (isCollided) return;
            HandleCollisionEffect(other);

            damageData.Direction = (other.transform.position - caster.transform.position).normalized *
                        damageData.KnockBackForce;

            spellObject.StartCoroutine(ApplyDamageOverTime(takeHit));
            float angle = DamageUtil.CalculateAngleToTarget(caster.transform, other.transform);

            isCollided = true;
        }

        IEnumerator ApplyDamageOverTime(ITakeHit takeHit)
        {
            float timeElapsed = 0;

            while (timeElapsed < duration)
            {
                takeHit.TakeHit(damageData);
                timeElapsed += timeBetweenTicks;
                yield return new WaitForSeconds(timeBetweenTicks);
            }
        }

        void HandleCollisionEffect(Collider other)
        {
            if (affectedTargetPrefab != null)
            {
                var effect = Object.Instantiate(affectedTargetPrefab, other.transform);
                Object.Destroy(effect, duration);
            }

            if (collisionEffect != null)
            {
                var effect = Object.Instantiate(collisionEffect, other.transform.position,
                    Quaternion.identity);
                Object.Destroy(effect, 2f);
            }
        }
        
    }
}