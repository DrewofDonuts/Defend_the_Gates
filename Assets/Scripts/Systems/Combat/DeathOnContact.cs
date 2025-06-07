using UnityEngine;

namespace Etheral
{
    public class DeathOnContact : MonoBehaviour
    {
        [SerializeField] DamageData damageData;
        [SerializeField] LayerMask targetLayerMask;


        void OnTriggerEnter(Collider other)
        {
            if (other.transform.IsChildOf(transform.parent)) return;
            // if ((targetLayerMask.value & (1 << other.gameObject.layer)) == 0) return;
            
            if (other.TryGetComponent(out ITakeHit takeHIt))
            {
                // Apply damage to the health component
                var angle = DamageUtil.CalculateAngleToTarget(transform, other.transform);
                damageData.Transform = transform;
                takeHIt.TakeHit(damageData, angle);
            }
        }
    }
}