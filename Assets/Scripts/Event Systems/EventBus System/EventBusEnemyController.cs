using System;
using UnityEngine;

namespace Etheral
{
    public static class EventBusEnemyController
    {
        public static event Action<bool, Affiliation> OnIgnorePlayerCollision;
        public static event Action<StateMachine> OnEnemyDeath;
        public static event Action<StateMachine> OnEnemySpawn;

        //May be obsolete - player now controlling own layers to ignore 
        public static void IgnorePlayerCollision(object ob, bool _shouldIgnore,
            Affiliation affiliation = Affiliation.Fellowship)
        {
            OnIgnorePlayerCollision?.Invoke(_shouldIgnore, affiliation);
        }

        public static void EnemyDeath(object ob, StateMachine stateMachine)
        {
            OnEnemyDeath?.Invoke(stateMachine);
        }

        public static void EnemySpawn(object ob, StateMachine stateMachine)
        {
            OnEnemySpawn?.Invoke(stateMachine);
        }
    }
}