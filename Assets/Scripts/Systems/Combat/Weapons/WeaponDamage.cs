using System;
using System.Collections;
using System.Collections.Generic;
using Etheral.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class WeaponDamage : MonoBehaviour, IGetBlocked
    {
        [field: SerializeField] public MeleeWeaponItem EquippedWeaponItem { get; private set; }

        [field: SerializeField] public TrailRenderer TrailRenderer { get; private set; }
        [field: SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Transform WeaponPivot { get; private set; }

        public Collider MyCollider { get; set; }
        public CharacterAudio CharacterAudio { get; private set; }
        StateMachine _stateMachine;
        List<Collider> _alreadyCollidedWith = new();

      [field: SerializeField]  public DamageData DamageData { get; private set; } = new();


        public bool isActive { get; private set; }

        public event Action<int> OnLifeSteal;



        void Start()
        {
            ConfigureComponents();

            isActive = false;

            if (TrailRenderer != null)
                TrailRenderer.emitting = false;

            DamageData.SetGetBlocked(this);
        }

        public void EnableWeaponDamage()
        {
            isActive = true;

            if (!Collider.isTrigger)
                Collider.isTrigger = true;

            if (CharacterAudio != null)
                CharacterAudio.PlayRandomOneShot(CharacterAudio.WeaponSource, EquippedWeaponItem.WeaponAudio.Swooshes,
                    AudioType.woosh, .90f, 1.10f, .90f, 1.10f);

            // CharacterAudio.PlayRandomOneShot(CharacterAudio.WeaponSource, EquippedWeaponItem.Swooshes);
            if (TrailRenderer != null)
                TrailRenderer.emitting = true;

            _alreadyCollidedWith.Clear();
        }

        public void DisableWeaponDamage()
        {
            isActive = false;
            ResetDamageValues();
            _alreadyCollidedWith.Clear();

            if (TrailRenderer != null)
                TrailRenderer.emitting = false;
        }

        void OnDisable()
        {
            isActive = false;
            ResetDamageValues();
            _alreadyCollidedWith.Clear();

            if (TrailRenderer != null)
                TrailRenderer.emitting = false;
        }


        public void SetCharacterComponents(StateMachine stateMachine, CharacterAudio characterAudio)
        {
            _stateMachine = stateMachine;
            MyCollider = stateMachine.GetCharComponents().GetCC();

            // Transform = MyCollider.transform;
            CharacterAudio = characterAudio;

            DamageData.Transform = MyCollider.transform;
        }


        public void HandleCollision(Collider collision)
        {
            if (collision == MyCollider || MyCollider.gameObject.CompareTag(collision.gameObject.tag)) return;
            if (_alreadyCollidedWith.Contains(collision)) return;

            // if (enabled == false) return;

            _alreadyCollidedWith.Add(collision);

            var angle = DamageUtil.CalculateAngleToTarget(MyCollider.transform, collision.transform);

            if (collision.TryGetComponent(out ITakeHit iTakeHit))
            {
                iTakeHit.TakeHit(DamageData, angle);

                OnLifeSteal?.Invoke((int)(DamageData.Damage * .25f));


                if (_stateMachine.ActiveAbility)
                    StartCoroutine(ClearList());
            }
        }


        IEnumerator ClearList()
        {
            yield return new WaitForSeconds(.15f);
            _alreadyCollidedWith.Clear();
        }

        //TODO: Remove this and use other Overload method. Requires removing Ability system and using CharacterAction instead
        [Obsolete("Deprecated. Use SettAttackStatDamage(CharacterAction) instead")]
        public void SettAttackStatDamage(float damage, float knockBack, float knockDown, bool isShieldBreak = false,
            FeedbackType feedbackType = FeedbackType.Medium, bool isExecution = false)
        {
            DamageData.damage = damage;
            DamageData.knockBackForce = knockBack;
            DamageData.knockDownForce = knockDown;
            DamageData.isShieldBreak = isShieldBreak;
            DamageData.isExecution = isExecution;
            DamageData.feedbackType = feedbackType;
            DamageData.audioImpact = EquippedWeaponItem.WeaponAudio.AudioImpact;
        }

        public void SettAttackStatDamage(CharacterAction _characterAction, float damageModifier = 0, int id = -1)
        {
            float incomingDamage = _characterAction.Damage;

            if (damageModifier > 0)
                incomingDamage += incomingDamage * damageModifier;

            DamageData.damage = incomingDamage;
            DamageData.dotDamage = _characterAction.DotDamage;
            DamageData.knockBackForce = _characterAction.KnockBackForce;
            DamageData.knockDownForce = _characterAction.KnockDownForce;
            DamageData.isShieldBreak = _characterAction.IsShieldBreak;
            DamageData.isExecution = _characterAction.IsExecution;
            DamageData.feedbackType = _characterAction.FeedbackType;
            DamageData.audioImpact = _characterAction.AudioImpact;
            DamageData.attackerID = id;
        }

        public void ResetDamageValues()
        {
            DamageData.damage = 0;
            DamageData.dotDamage = 0;
            DamageData.knockBackForce = 0;
            DamageData.knockDownForce = 0;
            DamageData.isShieldBreak = false;
            DamageData.isExecution = false;
            DamageData.Direction = Vector3.zero;
        }

        public void SetWeaponItem(MeleeWeaponItem weaponItem)
        {
            // Rigidbody = GetComponent<Rigidbody>();
            EquippedWeaponItem = weaponItem;

            if (TrailRenderer == null)
                TrailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        void ConfigureComponents()
        {
            Collider.isTrigger = true;

            if (TrailRenderer != null)
                TrailRenderer.emitting = false;
        }

        public void SetCollider(bool isActive)
        {
            Collider.enabled = isActive;
        }


#if UNITY_EDITOR

        [Button("Get Components")]
        public void LoadComponents()
        {
            if (Collider == null)
                Collider = GetComponentInChildren<Collider>();

            // Rigidbody = GetComponent<Rigidbody>();
            TrailRenderer = GetComponentInChildren<TrailRenderer>();
            ConfigureComponents();
        }
#endif
        public void HandleBlocked()
        {
            //Spark VFX
            //Invoke an event. 

            Debug.Log("Blocked");
            _stateMachine.HandleGettingBlocked();
        }
    }
}