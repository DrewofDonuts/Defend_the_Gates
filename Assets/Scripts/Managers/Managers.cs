using UnityEngine;

namespace Etheral
{
#if UNITY_EDITOR
    public class Managers : MonoBehaviour
    {
        static Managers instance;
        public static Managers Instance => instance;
        
        
        [SerializeField] GameObject managersObject;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
    }
#endif
}