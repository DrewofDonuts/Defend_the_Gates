using System;
using UnityEngine;

namespace Etheral
{
    public class AIEffectsController : MonoBehaviour
    {
        [SerializeField] GameObject spawnEffect;

        
        public void SpawnEffect()
        {
            Instantiate(spawnEffect, transform.position, Quaternion.identity);
        }
    }
}