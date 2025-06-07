using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class CharacterManager : MonoBehaviour
    {
        static CharacterManager _instance;
        public static CharacterManager Instance => _instance;

        public PlayerStateMachine Player { get; private set; }
        public List<EnemyStateMachine> Enemy { get; private set; } = new();
        
        

        public bool IsReady { get; private set; }

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
        }

        IEnumerator Start()
        {
            yield return new WaitUntil(() => Player != null);
            IsReady = true;
        }
        
        public void Register<T>(T register)
        {
            if (typeof(T) == typeof(PlayerStateMachine))
            {
                Player = register as PlayerStateMachine;
            }

            if (typeof(T) == typeof(EnemyStateMachine))
            {
                if (Enemy.Contains(register as EnemyStateMachine))
                    return;

                Enemy.Add(register as EnemyStateMachine);
            }
            
        }

        public void Unregister<T>(T unregister)
        {
            if (typeof(T) == typeof(PlayerStateMachine))
            {
                Player = null;
            }

            if (typeof(T) == typeof(EnemyStateMachine))
            {
                Enemy.Remove(unregister as EnemyStateMachine);
            }
        }


        public void SetPlayerPositionAndRotationForCinematic(Transform actorTransform)
        {
            var _transform = Player.transform;
            _transform.position = actorTransform.position;
            _transform.rotation = actorTransform.rotation;
        }
    }
}