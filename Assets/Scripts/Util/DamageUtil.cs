using Etheral;
using UnityEngine;

public static class DamageUtil
{
    // public static float CheckAngleToTarget(Transform MyCollider, Transform targetTransform)
    // {
    //     var attackAngle = Vector3.Angle(targetTransform.transform.forward,
    //         MyCollider.transform.position - targetTransform.transform.position);
    //     
    //     Debug.Log("Attack Angle: " + attackAngle);
    //
    //     return attackAngle;
    // }

    public static float CalculateAngleToTarget(Transform MyCollider, Transform targetTransform)
    {
        // Get the forward direction of the target and project it onto the XZ plane
        Vector3 targetForward = targetTransform.forward;
        targetForward.y = 0; // Ignore the Y component'
        targetForward.Normalize();

        // Get the direction from the target to MyCollider and project it onto the XZ plane
        Vector3 directionToCollider = MyCollider.position - targetTransform.position;
        directionToCollider.Normalize();
        directionToCollider.y = 0; // Ignore the Y component

        // Calculate the angle between the projected forward direction and the direction to MyCollider
        float attackAngle = Vector3.Angle(targetForward, directionToCollider);

        return attackAngle;
    }


    public static Vector3 CalculateKnockBack(Transform attackingTransform, Transform receivingTransform,
        float knockBackForce)
    {
        if (attackingTransform == null || receivingTransform == null)
            return Vector3.zero;
        Vector3 direction = (receivingTransform.position - attackingTransform.transform.position).normalized;
        direction.y = 0;
        return direction * knockBackForce;
    }
    
    
public static bool CalculateIfInRange(Transform targetTransform, Transform MyCollider, float maxDistance)
    {
        // Calculate the distance between the two transforms
        float distance = Vector3.Distance(targetTransform.position, MyCollider.position);

        // Check if the distance is within the specified range
        return distance <= maxDistance;
    }


    public static void AttackHitsTarget(IDamage iDamage, ITakeHit iTakeHit, WeaponDamage weaponDamage, float angle)
    {
        iTakeHit.TakeHit(iDamage, angle);
    }
}