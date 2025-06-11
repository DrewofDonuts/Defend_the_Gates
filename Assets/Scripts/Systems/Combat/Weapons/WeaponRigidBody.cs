using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(BoxCollider))]
    public class WeaponRigidBody : MonoBehaviour, IAffiliate
    {
        [SerializeField] HitBox hitBox;
        [field: SerializeField] public Rigidbody Rigidbody { get; set; }
        [field: SerializeField] public CapsuleCollider CapsuleCollider { get; set; }
        [field: SerializeField] public SphereCollider SphereCollider { get; set; }

        [field: SerializeField] public BoxCollider BoxCollider { get; set; }
        [field: SerializeField] public WeaponDamage WeaponDamage { get; set; }

        [field: SerializeField] public Transform OriginalParent;

        [field: SerializeField] public bool IsLeft { get; set; }
        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;

        public List<Collider> _alreadyCollidedWith = new();

        public Collider currentCollider;

        public void ToggleCurrentCollider(bool isActive)
        {
            _alreadyCollidedWith.Clear();
            currentCollider.enabled = isActive;
        }


        ITakeHit myCollider => hitBox;

        void Awake()
        {
            if (Rigidbody == null)
            {
                Rigidbody = GetComponent<Rigidbody>();
            }

            if (Rigidbody != null)
                SetupRigidBody();


            if (hitBox == null)
                hitBox = GetComponentInParent<HitBox>();

            // gameObject.SetActive(false);

            if (hitBox == null)
                hitBox = transform.parent.GetComponentInChildren<HitBox>();
            ConfigCollider();
        }
        
        

        void ConfigCollider()
        {
            if (CapsuleCollider == null)
            {
                CapsuleCollider = GetComponent<CapsuleCollider>();
                CapsuleCollider.isTrigger = true;
            }

            if (SphereCollider == null)
            {
                SphereCollider = GetComponent<SphereCollider>();
                SphereCollider.isTrigger = true;
            }

            if (BoxCollider == null)
            {
                BoxCollider = GetComponent<BoxCollider>();
                BoxCollider.isTrigger = true;
            }
        }

        void OnTriggerStay(Collider other)
        {
            
            CheckBroaderHit(other);
        }

        void CheckBroaderHit(Collider other)
        {
            // if (WeaponDamage?. isActive != true) return;
            if (!other.TryGetComponent(out ITakeHit takeHit)) return;
            if (takeHit.Affiliation == Affiliation) return;
            if (takeHit == myCollider) return;
            if (WeaponDamage == null) return;
            if (_alreadyCollidedWith.Contains(other)) return;
            
            Debug.Log($"WeaponRigidBody collided with: {other.name}");
            
                _alreadyCollidedWith.Add(other);
                WeaponDamage.HandleCollision(other);
                
            // if (WeaponDamage.isActive)
            // {
            // }
        }


        void Update()
        {
            if (transform.localPosition != Vector3.zero)
                transform.localPosition = Vector3.zero;

            if (WeaponDamage != null && Rigidbody != null)
            {
                if (WeaponDamage.DamageData.Transform != null)
                    Rigidbody.MovePosition(WeaponDamage.DamageData.Transform.localPosition);
            }
        }

        public void SetupRigidBody()
        {
            Rigidbody.isKinematic = true;
            Rigidbody.detectCollisions = true;
            Rigidbody.useGravity = false;
            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        public void SetupWeapon(WeaponDamage weaponDamage)
        {
            gameObject.SetActive(true);
            WeaponDamage = weaponDamage;
            transform.parent = WeaponDamage.WeaponPivot;

            var collider = WeaponDamage.Collider;

            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1, 1, 1);


            if (WeaponDamage.Collider is CapsuleCollider)
            {
                var newCollider = collider as CapsuleCollider;
                if (newCollider == null) return;
                SphereCollider.enabled = false;
                BoxCollider.enabled = false;
                CapsuleCollider.enabled = true;
                CapsuleCollider.isTrigger = true;

                CapsuleCollider.radius = newCollider.radius;
                CapsuleCollider.height = newCollider.height;
                CapsuleCollider.center = newCollider.center;
                CapsuleCollider.direction = newCollider.direction;
                CapsuleCollider.center = new Vector3(newCollider.center.x, newCollider.center.y,
                    newCollider.center.z);

                currentCollider = CapsuleCollider;
            }

            if (WeaponDamage.Collider is SphereCollider)
            {
                var newCollider = collider as SphereCollider;
                if (newCollider == null) return;
                CapsuleCollider.enabled = false;
                BoxCollider.enabled = false;
                SphereCollider.enabled = true;
                SphereCollider.isTrigger = true;


                SphereCollider.radius = newCollider.radius;
                SphereCollider.center = newCollider.center;

                currentCollider = SphereCollider;
            }

            if (WeaponDamage.Collider is BoxCollider)
            {
                var newCollider = collider as BoxCollider;
                if (newCollider == null) return;
                CapsuleCollider.enabled = false;
                SphereCollider.enabled = false;
                BoxCollider.enabled = true;
                BoxCollider.isTrigger = true;
                BoxCollider.size = newCollider.size;
                BoxCollider.center = newCollider.center;

                currentCollider = BoxCollider;
            }
            
            currentCollider.enabled = false;

            weaponDamage.SetCollider(false);

            // Collider = weaponDamage.gameObject.GetComponent<CapsuleCollider>();
        }

        public void ResetRB()
        {
            transform.parent = OriginalParent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            WeaponDamage = null;
            gameObject.SetActive(false);
        }


#if UNITY_EDITOR
        [Button("LoadComponents")]
        public void LoadComponents()
        {
            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();

            if (CapsuleCollider == null)
                CapsuleCollider = GetComponent<CapsuleCollider>();

            if (SphereCollider == null)
                SphereCollider = GetComponent<SphereCollider>();

            if (BoxCollider == null)
                BoxCollider = GetComponent<BoxCollider>();

            hitBox = transform.parent.GetComponentInChildren<HitBox>();

            SetupRigidBody();
        }
#endif
    }
}