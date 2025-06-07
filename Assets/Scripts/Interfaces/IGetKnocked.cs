using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetKnocked
{
    public void AddForce(Vector3 force);
    
}



    // public void CheckForKnockForce(Transform attacker,float knockBackForce, float knockDownForce);
    // public void KnockedBack(Transform attacker, float knockbackForce);
    // public void KnockedDown(Transform attacker, float knockbackForce);


// if (enemyStats != null)
// {
//     Rigidbody rb = collision.GetComponent<Rigidbody>();
//
//     if (rb != null)
//     {
//         //TODO move to weapon specific
//         Vector3 direction = collision.transform.position - transform.position;
//         direction.y = 0;
//         rb.AddForce(direction.normalized * 20, ForceMode.Impulse);
//         print("rb found");
//     }
//
//     enemyStats.TakeDamage(_weaponItem.Damage);
//     print("hit");
// }