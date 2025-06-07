using UnityEngine;

namespace Etheral
{
    public class OnGizmos : MonoBehaviour
    {
        [SerializeField] Color gizmoColor = Color.red;
        [SerializeField] float gizmoSize = 0.5f;

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, gizmoSize);
        }
    }
}