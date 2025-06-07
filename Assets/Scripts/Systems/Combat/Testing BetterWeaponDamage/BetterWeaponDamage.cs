using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
   
    [Obsolete("LIKELY TO REMOVE OR CONTINUE TO BUILD WHEN REFACTORING")]
    public class BetterWeaponDamage : MonoBehaviour
    {
        // [field: SerializeField] public MeleeWeaponItem EquippedWeaponItem { get; private set; }
        // [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        //
        //
        // StateMachine _stateMachine;
        // Collider MyCollider => _stateMachine.CharacterController;
        // List<Collider> _alreadyCollidedWith = new();
        //
        //
        // public Transform Transform { get; set; }
        // public float Damage { get; set; }
        // public float DotDamage { get; set; }
        // public float KnockBackForce { get; set; }
        // public float KnockDownForce { get; set; }
        // public bool IsShieldBreak { get; set; }
        // public bool IsExecution { get; set; }
        // public Vector3 Direction { get; set; }
        // public AttackType AttackType { get; set; }
        // public AudioImpact AudioImpact
        // {
        //     get { return EquippedWeaponItem.WeaponAudio.AudioImpact; }
        //     set { }
        // }
        //
        //
        // void Start()
        // {
        //     Transform = MyCollider.transform;
        // }
        //
        // void Update()
        // {
        // }
        //
        // void OnTriggerEnter(Collider collision)
        // {
        //     if (collision == MyCollider || MyCollider.gameObject.CompareTag(collision.gameObject.tag)) return;
        //     if (_alreadyCollidedWith.Contains(collision) || !enabled) return;
        //
        //     _alreadyCollidedWith.Add(collision);
        //     Vector3 direction = default;
        //     if (collision.TryGetComponent(out IGetKnocked getKnocked))
        //         direction = WeaponDamageProcessor.CalculateKnockBack(Transform, collision, this);
        //
        //     var angle = WeaponDamageProcessor.CheckAngleToTarget(MyCollider.transform, collision.transform);
        //
        //     if (collision.TryGetComponent(out ITakeHit iTakeHit))
        //     {
        //         iTakeHit.TakeHit(this, angle);
        //     }
        // }
        //
        //
        // public void PassAttackBaseDamage(CharacterAction _characterAction)
        // {
        //     Damage = _characterAction.Damage;
        //     DotDamage = _characterAction.DotDamage;
        //     KnockBackForce = _characterAction.KnockBackForce;
        //     KnockDownForce = _characterAction.KnockDownForce;
        //     IsShieldBreak = _characterAction.IsShieldBreak;
        //     IsExecution = _characterAction.IsExecution;
        //     AttackType = _characterAction.AttackType;
        //     AudioImpact = _characterAction.AudioImpact;
        // }
    }
}