using System;
using UnityEngine;

namespace Etheral
{
    public class ProjectileKinematic : Projectile
    {
        void OnEnable()
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        void Update()
        {
            if (isCollided) return;
            transform.Translate((Vector3.forward + Vector3.right * aimOffset) * speed * Time.deltaTime);
        }
    }
}