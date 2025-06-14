using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] List<MonoBehaviour> systems;

        List<IInitialize> initializers = new List<IInitialize>();

        static Bootstrapper _instance;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            InitializeSystems();
        }

        void InitializeSystems()
        {
            foreach (var system in systems)
            {
                system.gameObject.SetActive(true);
                if (system is IInitialize initializer)
                {
                    initializers.Add(initializer);
                    initializer.Initialize();
                }
            }
        }

        void OnValidate()
        {
            foreach (var sys in systems)
            {
                sys.gameObject.SetActive(false);
            }
        }
    }
}