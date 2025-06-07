using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class PlayerProximityDetector : MonoBehaviour
    {
        [field: SerializeField] public CombatManager CombatManager { get; private set; }
        [field: SerializeField] public EnemyStateMachine EnemyStateMachine { get; private set; }
        
        public bool PlayerIsttacking => CombatManager.IsPlayerAttacking;
        
        // void Start()
        // {
        //     CombatManager = CombatManager.Instance;
        // }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (CombatManager == null) return;
                // CombatManager = CombatManager.Instance;
                if (collision.gameObject.transform == CharacterManager.Instance.Player.transform)
                {
                    if (CombatManager.IsPlayerAttacking)
                    {
                 
                    }
                }
            }
        }
        
        public void SetStateMachine(CombatManager combatManager)
        {
            CombatManager = combatManager;
        }
    }
}