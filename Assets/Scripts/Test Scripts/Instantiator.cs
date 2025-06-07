using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class Instantiator : MonoBehaviour
    {
        public GameObject prefab;
        public Transform parent;

        [Button("Instantiate Object")]
        public void InstantiateObject()
        {
            var instance = Instantiate(prefab, parent.position, Quaternion.identity);
            
            if (instance.TryGetComponent<EntitySaver>(out var entitySaver))
                entitySaver.isSpawned = true;
        }
    }
}