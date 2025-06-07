using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [RequireComponent(typeof(Rigidbody))]
    public class HolyChargeController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Collider shieldCollider;
        [SerializeField] Rigidbody rb;
        [SerializeField] GameObject shieldObject;
        [SerializeField] CharacterAudio characterAudio;
        [SerializeField] AudioClip chargedSound;

        [ReadOnly]
        public DamageData damageData;
        bool startDamageBuildup;

        float maxDamage;
        float maxKnockBackForce;
        float maxKnockDownForce;

        float knockBackAPS => maxKnockBackForce * .65f;
        float knockDownAPS => maxKnockBackForce * .65f;
        float damageAPS => maxDamage * .65f;

        bool maxHit;
        bool canDamage;

        void Start()
        {
            rb.isKinematic = true;
            shieldCollider.isTrigger = true;
            shieldCollider.enabled = false;
            damageData.Transform = transform;
            shieldObject.SetActive(false);
        }

        public void SetMaxDamageAndKnockBack(float damage, float knockBackForce, float KnockDownForce)
        {
            maxDamage = damage;
            maxKnockBackForce = knockBackForce;
            maxKnockDownForce = KnockDownForce;
        }


        void Update()
        {
            if (!startDamageBuildup) return;
            BuildDamageOverTime();
        }

        public void StartDamageBuildup() => startDamageBuildup = true;

        public void BuildDamageOverTime()
        {
            damageData.damage = Mathf.Min(damageData.damage + damageAPS * Time.deltaTime, maxDamage);
            damageData.knockBackForce = Mathf.Min(damageData.knockBackForce + knockBackAPS * Time.deltaTime,
                maxKnockBackForce);
            damageData.knockDownForce = Mathf.Min(damageData.knockDownForce + knockDownAPS * Time.deltaTime,
                maxKnockDownForce);

            if (damageData.damage >= maxDamage && !maxHit)
            {
                characterAudio.PlayOneShot(characterAudio.MagicSource, chargedSound);
                maxHit = true;
            }
        }

        public void SetColliderActive(bool active)
        {
            canDamage = active;
            shieldCollider.enabled = active;
            shieldObject.SetActive(active);

            if (!active)
                Reset();
        }

        void Reset()
        {
            shieldObject.SetActive(false);
            startDamageBuildup = false;
            damageData.damage = 0;
            damageData.knockBackForce = 0;
            damageData.knockDownForce = 0;
            maxHit = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ITakeHit takeHit))
            {
                if (takeHit.Affiliation == Affiliation.Fellowship) return;
                if (!canDamage) return;
                takeHit.TakeHit(damageData);
            }
        }
    }
}