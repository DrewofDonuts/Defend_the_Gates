using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    public class EnemyGroup : MonoBehaviour, IGetTriggered
    {
        [Header("References")]
        [SerializeField] List<EnemyStateMachine> enemies;

        [Header("Settings")]
        [SerializeField] EnemyGroupAction groupAction;
        [SerializeField] bool isHostile = true;
        [SerializeField] bool groupAttack;


        [Header("On Death and Spawn Events")]
        [SerializeField] UnityEvent onGroupDeath;
        [SerializeField] UnityEvent onGroupSpawn;


        public int deathCounter;
        public int totalEnemies;


        public ITrigger Trigger { get; set; }


        void Start()
        {
            Trigger = GetComponent<ITrigger>();
            Trigger.OnReceive += HandleReceivingKey;

            if (enemies.Count == 0)
                AddEnemies();

            if (enemies.Count > 0)
                SubscribeToEvents();

            totalEnemies = enemies.Count;
        }

        protected void OnDisable()
        {
            Trigger.OnReceive -= HandleReceivingKey;
            foreach (var enemy in enemies)
            {
                enemy.Health.OnDie -= HandleEnemyDeath;
            }
        }


        void SubscribeToEvents()
        {
            foreach (var enemy in enemies)
            {
                Debug.Log($"Subscribing to enemy: {enemy.name}");
                enemy.Health.OnDie += HandleEnemyDeath;

                // enemy.Health.OnExecuted += HandleEnemyDeath;
                enemy.SetHostile(isHostile);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!groupAttack) return;
            if (enemies.Count == 0) return;

            if (other.TryGetComponent(out PlayerStateMachine player))
            {
                TriggerGroupAttack();
            }
        }

        void TriggerGroupAttack()
        {
            foreach (var enemy in enemies)
            {
                if (enemy.StateType != StateType.Dead)
                {
                    enemy.gameObject.SetActive(true);
                    enemy.SetHostile(true);
                    enemy.SwitchState(new EnemyAbsoluteChaseState(enemy));
                }
            }
        }


        void HandleReceivingKey()
        {
            if (groupAction == EnemyGroupAction.AbsoluteChase)
            {
                TriggerGroupAttack();
            }
        }


        void HandleEnemyDeath(Health health)
        {
            deathCounter++;

            health.OnDie -= HandleEnemyDeath;
            enemies.RemoveAll(enemy => enemy.Health.IsDead);
            CheckDeathCounter();
        }

        void CheckDeathCounter()
        {
            if (deathCounter >= totalEnemies)
            {
                HandleGroupDeath();
            }
        }

        void HandleGroupDeath()
        {
            onGroupDeath?.Invoke();

            if (!string.IsNullOrWhiteSpace(Trigger.KeyToSend))
            {
                Debug.Log($"Group {gameObject.name} has died. Total deaths: {deathCounter}/{totalEnemies}");
                Trigger.SendKeyAndMessage();
            }
        }

        public void AddEnemies()
        {
            var enemiesToAdd = GetComponentsInChildren<EnemyStateMachine>();

            foreach (var enemyStateMachine in enemiesToAdd)
            {
                if (enemies.Contains(enemyStateMachine)) continue;
                enemies.Add(enemyStateMachine);
                enemyStateMachine.Health.OnDie += HandleEnemyDeath;
            }

            totalEnemies = enemies.Count;
            SubscribeToEvents();
        }


#if UNITY_EDITOR

        [Button("Add Enemies")]
        void AddEnemiesFromEditor()
        {
            enemies.Clear();
            AddEnemies();
        }

        [Button("Disable Enemies")]
        void DisableEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }
        }

        [Button("Set Hostilities")]
        void SetHostility(bool _isHostile)
        {
            foreach (var enemy in enemies)
            {
                enemy.SetHostile(_isHostile);
            }
        }

        [Button("Test Group Death")]
        void TestGroupDeath()
        {
            deathCounter = totalEnemies;

            HandleGroupDeath();
        }

#endif
    }

    public enum EnemyGroupAction
    {
        AbsoluteChase = 0,
    }
}