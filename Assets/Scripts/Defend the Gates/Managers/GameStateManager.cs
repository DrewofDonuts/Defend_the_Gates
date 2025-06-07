using System;
using UnityEngine;

namespace Etheral
{
    public class GameStateManager : MonoBehaviour, IInitialize
    {
        public GameState CurrentGameState { get; private set; } = GameState.None;

        static GameStateManager instance;
        public static GameStateManager Instance => instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Initialize()
        {
            GameManager.Instance.OnSceneLoading += OnGameLoading;
        }

        void OnDisable()
        {
            GameManager.Instance.OnSceneLoading -= OnGameLoading;
        }

        void OnGameLoading(GameState gameState)
        {
            CurrentGameState = gameState;
        }
    }


    public enum GameState
    {
        None = 0,
        MainMenu = 1,
        StartGame = 2,
        WaveInProgress = 3,
        BasePhase = 4,
        EndGame = 5,
    }
}