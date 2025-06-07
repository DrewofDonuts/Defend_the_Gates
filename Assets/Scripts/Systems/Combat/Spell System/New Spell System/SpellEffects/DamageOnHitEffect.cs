using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class DamageOnHitEffect : BaseEffect
    {
        [Header("Effect Settings")]
        public LayerMask layersToHit;
        [Tooltip("Time damage can be applied")]
        public float activeDuration = 5f;
        [Tooltip("Can only apply damage once if enabled")]
        public bool disableOnCollision;

        [Header("IDamage Settings")]
        public DamageData damageData = new();


        [Header("References")]
        public CollisionConfig collisionEffect;

        ITakeHit hitTaker;


        bool isActive = true;
        float durationTimer;
        bool canApplyDamage = true;


        // List<Collider> alreadyCollidedWith = new();

        public override void Initialize(SpellObject _spellObject, ICastSpell iCastSpell)
        {
            base.Initialize(_spellObject, iCastSpell);
            damageData.Transform = spellObject.transform;
        }


        public override void Tick(float deltaTime)
        {
            if (disable) return;
            base.Tick(deltaTime);

            if (!isActive) return;
            durationTimer += deltaTime;
            if (durationTimer >= activeDuration)
            {
                isActive = false;
            }
        }

        public override void ApplyOnTriggerEnter(Collider other)
        {
            if (disable) return;
            if (DoNotHitSelf(other)) return;
            if ((layersToHit & (1 << other.gameObject.layer)) == 0) return;
            if (!isActive) return;
            if (!canApplyDamage) return;

            if (other.gameObject.TryGetComponent(out ITakeHit takeHit))
                HandleHittingTarget(other, takeHit);

            // else
            //     HandleHittingObjects(other);
        }


        void HandleHittingTarget(Collider other, ITakeHit takeHit)
        {
            if (IsAffiliationSameAsCaster(takeHit.Affiliation)) return;
            if (hitTaker == takeHit) return;

            // if (alreadyCollidedWith.Contains(other)) return;
            HandleCollisionEffect(other);

            damageData.Direction = (other.transform.position - caster.transform.position).normalized *
                                   damageData.knockBackForce;

            damageData.Transform = spellObject.transform;

            float angle = DamageUtil.CalculateAngleToTarget(spellObject.transform, other.transform);

            hitTaker = takeHit;
            takeHit.TakeHit(damageData, angle);

            // alreadyCollidedWith.Add(other);
            HandleDestroyOnCollision();
        }

        void HandleHittingObjects(Collider other)
        {
            // if (alreadyCollidedWith.Contains(other)) return;

            HandleCollisionEffect(other);
            HandleDestroyOnCollision();
        }

        void HandleDestroyOnCollision()
        {
            if (disableOnCollision)
            {
                canApplyDamage = false;
                spellObject.DisableChildObjects();
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