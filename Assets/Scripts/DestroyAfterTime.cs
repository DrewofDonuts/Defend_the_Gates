using UnityEngine;

namespace Etheral
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float timeBeforeDestroy = 5f;

        void Start()
        {
            Destroy(gameObject, timeBeforeDestroy);
        }
    }
}