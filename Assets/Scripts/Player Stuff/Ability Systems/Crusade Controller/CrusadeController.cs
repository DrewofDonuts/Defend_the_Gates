using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class CrusadeController : MonoBehaviour
    {
        [SerializeField] Collider shieldCollider;
        [SerializeField] Rigidbody rb;
        [SerializeField] GameObject shieldObject;
        [SerializeField] CharacterAudio characterAudio;

        bool canDamage;
        public DamageData damageData;

        List<ITakeHit> hitTargets = new();

        void Start()
        {
            rb.isKinematic = true;
            shieldCollider.isTrigger = true;
            shieldCollider.enabled = false;
            damageData.Transform = transform;
            shieldObject.SetActive(false);
        }

        public void SetIfCanDamage(bool _canDamage)
        {
            canDamage = _canDamage;

            shieldCollider.enabled = canDamage;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!canDamage) return;
            if (other.TryGetComponent(out ITakeHit takeHit))
            {
                if (hitTargets.Contains(takeHit)) return;
                if (takeHit.Affiliation != Affiliation.Enemy) return;
                Debug.Log($"CrusadeController hit: {takeHit}");


                EventBusPlayerController.FeedbackIgnoringDistanceFromPlayer(this, FeedbackType.Medium);
                hitTargets.Add(takeHit);
                takeHit.TakeHit(damageData);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ITakeHit takeHit))
            {
                if (!hitTargets.Contains(takeHit)) return;
                hitTargets.Remove(takeHit);
            }
        }

        public void SetColliderActive(bool active)
        {
            shieldCollider.enabled = active;
            shieldObject.SetActive(active);

            if (!active)
                shieldObject.SetActive(false);
        }
    }
}