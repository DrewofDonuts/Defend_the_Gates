using System;
using System.Collections.Generic;
using Etheral.Audio;
using UnityEngine;

//To Deprecate

namespace Etheral.Combat
{
    public class DeprecatedWeaponDamage : MonoBehaviour
    {
        [SerializeField] WeaponInventory _weaponInventory;
        [SerializeField] Collider _myCollider;
        [SerializeField] CharacterAudio _attackerAudio;

        [field: SerializeField] public bool IsLeft { get; private set; }
        [field: SerializeField] public bool IsShield { get; private set; }
        [field: SerializeField] public bool Is_2H_Collider { get; private set; }


        public Transform AttackingTransform => _myCollider.transform;

        public Transform Transform { get; }
        public float Damage => attackDamage + abilityDamage;
        public float DotDamage { get; }

        public float KnockBackForce => attackKnockback + abilityKnockback;
        public float KnockDownForce => attackKnockDown + abilityKnockDown;
        public bool IsShieldBreak { get; }
        public bool IsExecution { get; }
        public Vector3 Direction { get; }
        public FeedbackType FeedbackType { get; }
        public AudioImpact AudioImpact { get; }

        List<Collider> _alreadyCollidedWith = new List<Collider>();
        int attackDamage;
        float attackKnockback;
        float attackKnockDown;

        int abilityDamage;
        float abilityKnockback;
        float abilityKnockDown;

        MeleeWeaponItem _loadedWeapon;
        public event Action<int> OnHit;

        void Awake()
        {
            LoadEquippedWeapon();
        }

        void OnEnable()
        {
            if (_loadedWeapon != null)
                _attackerAudio.PlayRandomOneShot(_attackerAudio.WeaponSource, _loadedWeapon.WeaponAudio.Swooshes, AudioType.none);
            _alreadyCollidedWith.Clear();
            // _weaponInventory.ChangedWeaponEvent += LoadEquippedWeapon;
            
            Debug.Log("WeaponDamage " + gameObject.name);
        }

        void OnDisable()
        {
            ResetDamageValues();
            // _weaponInventory.ChangedWeaponEvent -= LoadEquippedWeapon;
        }

        void OnTriggerEnter(Collider collision)
        {
            if (collision == _myCollider) return;
            if (_myCollider.gameObject.CompareTag(collision.gameObject.tag)) return;
            if (_alreadyCollidedWith.Contains(collision)) return;

            _alreadyCollidedWith.Add(collision);
            if (collision.TryGetComponent(out IGetKnocked getKnocked))
                Debug.Log(getKnocked);

            var angle = CheckAngleToTarget(collision.transform);
            var direction = CalculateKnockBack(collision);


            if (collision.TryGetComponent(out ITakeHit health))
            {
                Debug.Log(health);
                CheckHitOrBlock(health, getKnocked, angle, direction);
            }
            else
            {
                getKnocked?.AddForce(direction);
            }
        }

        Vector3 CalculateKnockBack(Collider collision)
        {
            Vector3 direction = (collision.transform.position - _myCollider.transform.position).normalized;
            return direction * KnockBackForce;
        }

        void CheckHitOrBlock(ITakeHit health, IGetKnocked getKnocked, float angle, Vector3 force)
        {
            // if (health.IsBlocking && angle is >= 0f and <= 70f)
            // {
            //     AttackIsBlocked(force, getKnocked);
            // }
            //
            // // else if (health.IsBlocking)
            // //     Debug.Log("Is Dodging");
            // //add health.IsDodging to implement this
            // else
            // {
            //     AttackHitsTarget(force, health);
            // }
        }

        void AttackIsBlocked(Vector3 force, IGetKnocked iGetKnocked)
        {
            // _attackerAudio.PlayRandomOneShot(_attackerAudio.BlockSource, _loadedWeapon.Blocked);
            // if (iGetKnocked != null)
            // {
            //     iGetKnocked.AddForce(force * KnockBackForce * .50f);
            // }
        }

        void AttackHitsTarget(Vector3 force, ITakeHit health)
        {
            //weapon's Hit sound - TODO: possible remove this and only rely on the target's impact sounds
            // _attackerAudio.PlayRandomOneShot(_attackerAudio.WeaponSource, loadedWeapon.TargetHit);
            // health.TakeDamage(this);
            
            Debug.Log(health);
            // health.TakeHit(this, force, _loadedWeapon.AudioImpact);

            //LifeSteal receiver subscribes to this
            OnHit?.Invoke((int)(Damage * .25f));
        }

        public void LoadEquippedWeapon()
        {
            if (IsLeft && _weaponInventory.LeftEquippedWeapon != null && !_weaponInventory.LeftEquippedWeapon.IsRanged)
            {
                _loadedWeapon = _weaponInventory.LeftEquippedWeapon;
            }
            else if (!IsLeft && _weaponInventory.RightEquippedWeapon != null &&
                     !_weaponInventory.RightEquippedWeapon.IsRanged)
            {
                _loadedWeapon = _weaponInventory.RightEquippedWeapon;
            }
            else
                _loadedWeapon = null;
        }


        float CheckAngleToTarget(Transform targetTransform)
        {
            var attackAngle = Vector3.Angle(targetTransform.transform.forward,
                _myCollider.transform.position - targetTransform.transform.position);

            return attackAngle;
        }

        public void SettAttackStatDamage(int damage, float knockBack, float knockDown)
        {
            attackDamage = damage;
            attackKnockback = knockBack;
            attackKnockDown = knockDown;
        }

        public void SetAbilityStats(int damage, float knockBack, float knockDown)
        {
            abilityDamage = damage;
            abilityKnockback = knockBack;
            abilityKnockDown = knockDown;
        }

        public void ResetDamageValues()
        {
            abilityDamage = 0;
            abilityKnockback = 0f;
            abilityKnockDown = 0f;
            attackDamage = 0;
            attackKnockback = 0f;
            attackKnockDown = 0f;
        }

        public void HookComponents(WeaponInventory weaponInventory, Collider charCollider,
            CharacterAudio characterAudio)
        {
            _weaponInventory = weaponInventory;
            _myCollider = charCollider;
            _attackerAudio = characterAudio;
        }
    }
}