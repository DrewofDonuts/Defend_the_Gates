using Etheral;
using UnityEngine;

public class AOEDOTTest : MonoBehaviour
{
    public float damage = 10f;


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITakeHit takeHit))
        {
            takeHit.TakeDotDamage(damage * Time.deltaTime);
        }
    }
}
