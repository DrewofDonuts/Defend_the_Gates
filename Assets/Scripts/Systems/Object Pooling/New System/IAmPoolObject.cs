using UnityEngine;

namespace Etheral
{
    public interface IAmPoolObject<T> where T : MonoBehaviour
    {
        [field: SerializeField] public T Prefab { get; set; }
        [field: SerializeField] public string PoolKey { get; set; }

        public void Release();
    }
}