using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class CombatManager : MonoBehaviour
    {
        [field: SerializeField] public bool IsPlayerAttacking { get; private set; }
        [field: SerializeField] public bool IsPlayerBlocking { get; private set; }
        [field: SerializeField] public ITargetable CurrentEnemyTarget { get; private set; }

        // [field: SerializeField] public Health Player { get; private set; }
        [field: SerializeField] public Transform ExeuctionPoint { get; private set; }

        static CombatManager _instance;
        public static CombatManager Instance => _instance;

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
            
            // if (Player == null)
            //     Player = FindObjectOfType<PlayerStateMachine>().Health;
        }

        IEnumerator Start()
        {
            yield return new WaitUntil(() =>
                CharacterManager.Instance != null && CharacterManager.Instance.Player != null);
            {
                ExeuctionPoint = CharacterManager.Instance.Player.gameObject.GetComponent<PlayerStateMachine>()
                    .PlayerComponents.ExecutionPoint;
            }
        }
        
        
        
        
        
        
        //All code below likely to be removed 04/15/2024
        public void SetPlayerAttacking(bool isPlayerAttacking)
        {
            IsPlayerAttacking = isPlayerAttacking;
        }

        public void SetPlayerBlocking(bool isPlayerBlocking)
        {
            IsPlayerBlocking = isPlayerBlocking;
        }

        public void SetPlayerCurrentTarget(ITargetable currentEnemyTarget)
        {
            CurrentEnemyTarget = currentEnemyTarget;
        }
    }
}