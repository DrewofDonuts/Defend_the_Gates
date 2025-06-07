using Sirenix.OdinInspector;
using UnityEngine;


namespace Etheral
{
//CALLED FROM UNITY EVENTS FOR NOW
    public class InstantiateObject : MessengerClass
    {
        public GameObject objectToInstantiate;
        public bool isParented;
        public Transform parent;


        public void InstantiateObjectFunction()
        {
            if (!isParented)
            {
                var instantiatedObject = Instantiate(objectToInstantiate, transform.position,
                    transform.rotation);
            }
            else
            {
                var instantiatedObject = Instantiate(objectToInstantiate, parent);
                instantiatedObject.transform.rotation = objectToInstantiate.transform.rotation;
            }
        }

        protected override void HandleReceivingKey()
        {
            InstantiateObjectFunction();
        }

#if UNITY_EDITOR
        [Button("Spawn Object")]
        void SpawnObject()
        {
            InstantiateObjectFunction();
        }
#endif
    }
}