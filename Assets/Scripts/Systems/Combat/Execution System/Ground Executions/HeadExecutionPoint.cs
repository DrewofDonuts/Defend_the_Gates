using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class HeadExecutionPoint : MonoBehaviour, IHaveHealth
    {
        public bool CanBeHit { get; private set; }
        public bool isDead { get; private set; }

        //When player stomps, initiate RayFire + Instantiation of FX + Audio
        [field: SerializeField] public GameObject BloodFX { get; private set; }

        [field: SerializeField] public Transform HeadTransform { get; private set; }
        public event Action OnDestroyed;


        void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        void Update()
        {
            if (CanBeHit && HeadTransform != null)
                transform.position = HeadTransform.position;
        }

        public void BlowUpAndDie()
        {
            isDead = true;
            var offset = new Vector3(0, .5f, 0);
            Instantiate(BloodFX, transform.position + offset, Quaternion.identity);
            OnDie?.Invoke(this);
        }

        public void SetCanHit(bool isCanBeHit)
        {
            CanBeHit = isCanBeHit;
        }

        public void SetHeadTransform(Transform headTransform)
        {
            HeadTransform = headTransform;
        }

        public Affiliation Affiliation { get; set; }
        public Transform Transform { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public void Heal(float healthToAdd) { }
        public event Action<IHaveHealth> OnDie;
    }
}