using Etheral.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class NPCComponents : MonoBehaviour
    {
        [SerializeField] CompanionLockOnController companionLockOnController;

        public CompanionLockOnController GetLockOnController() => companionLockOnController;
        
        
        #if UNITY_EDITOR
        [Button("Load Components")]
        void LoadComponents()
        {
            companionLockOnController = GetComponentInChildren<CompanionLockOnController>();
        }
        #endif
        
    }
}