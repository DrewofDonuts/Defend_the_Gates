using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    //Used to apply instant damage after a delay

    public class DamageAfterDelayEffect : BaseEffect
    {
        [Header("Settings")]
        public LayerMask layersToHit;
        public float activeDuration = 5f;
        public float delayTime = 1f;
        public bool stopDamageOnCollision;

        [Header("IDamage Settings")]
        public DamageData damageData = new();
        
        [Header("References")]
        public CollisionConfig collisionEffect;

        bool setToDisable;
        bool delayHasCompleted;
        float delayTimer;
        float durationTimer;
        bool canApplyDamage;

        List<Collider> alreadyCollidedWith = new();

        public override void Initialize(SpellObject _spellObject, ICastSpell iCastSpell)
        {
            base.Initialize(_spellObject, iCastSpell);
            damageData.Transform = spellObject.transform;
        }

        public override void Tick(float deltaTime)
        {
            if (disable) return;
            base.Tick(deltaTime);

            if (!delayHasCompleted)
            {
                delayTimer += deltaTime;
                if (delayTimer >= delayTime)
                {
                    delayHasCompleted = true;
                    canApplyDamage = true;
                }
            }

            if (canApplyDamage)
            {
                durationTimer += deltaTime;
                if (durationTimer >= activeDuration)
                {
                    canApplyDamage = false;
                }
            }
        }

        public override void ApplyOnStay(Collider other)
        {
            if (disable) return;
            if (!delayHasCompleted) return;
            if (DoNotHitSelf(other)) return;
            if ((layersToHit & (1 << other.gameObject.layer)) == 0) return;
            if (alreadyCollidedWith.Contains(other)) return;
            if (!canApplyDamage) return;

            if (other.gameObject.TryGetComponent(out ITakeHit takeHit))
                HandleHittingTarget(other, takeHit);

            alreadyCollidedWith.Add(other);
        }

        void HandleHittingTarget(Collider other, ITakeHit takeHit)
        {
            if (IsAffiliationSameAsCaster(takeHit.Affiliation)) return;

            // if (!setToDisable)
            //     damageData.Direction = (other.transform.position - caster.transform.position).normalized *
            //                 damageData.KnockBackForce;
            

            float angle = DamageUtil.CalculateAngleToTarget(spellObject.transform, other.transform);

            takeHit.TakeHit(damageData, angle);

            HandleCollisionEffect(other);
            HandleDestroyOnCollision();
        }

        void HandleDestroyOnCollision()
        {
            if (stopDamageOnCollision)
            {
                canApplyDamage = false;
                setToDisable = true;
            }
        }

        void HandleCollisionEffect(Collider other)
        {
            if (collisionEffect != null)
            {
                collisionEffect.InstantiateEffect(other, spellObject.transform.position);
            }
        }
    }
}