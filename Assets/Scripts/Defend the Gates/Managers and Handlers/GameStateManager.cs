using System.Collections.Generic;
using Etheral.DefendTheGates;
using UnityEngine;

namespace Etheral
{
    public class GameStateManager : MonoBehaviour, IInitialize
    {
        public GameState CurrentGameState { get; private set; } = GameState.None;
        readonly List<IGameStateListener> gameStateListeners = new();

        static GameStateManager instance;
        public static GameStateManager Instance => instance;


        public int CurrentWave { get; private set; } = 0;


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

        void OnGameLoading(string sceneName)
        {
            CurrentGameState = SetGameModeBasedOnSceneName(sceneName);

            SetGameState(CurrentGameState);
        }

        public void SetGameState(GameState newGameState)
        {
            if (CurrentGameState == newGameState) return;

            CurrentGameState = newGameState;

            foreach (var listener in gameStateListeners)
            {
                listener.OnGameStateChanged(newGameState);
            }
        }

        public void IncrementWave()
        {
            CurrentWave++;
        }

        GameState SetGameModeBasedOnSceneName(string arg0)
        {
            GameState loadingState;
            if (arg0.Contains("Menu"))
                loadingState = GameState.MainMenu;
            else if (arg0.Contains("Defend"))
                loadingState = GameState.BasePhase;
            else if (arg0.Contains("Attack"))
                loadingState = GameState.AttackPhase;
            else if (arg0.Contains("Explore"))
                loadingState = GameState.ExplorePhase;
            else
                loadingState = GameState.None;
            return loadingState;
        }


        public void RegisterListener(IGameStateListener listener)
        {
            if (!gameStateListeners.Contains(listener))
            {
                Debug.Log(
                    $"GameStateManager: Registering listener {listener.GetType().Name} for game state {CurrentGameState}");
                gameStateListeners.Add(listener);
                listener.OnGameStateChanged(CurrentGameState);
            }
        }

        public void UnregisterListener(IGameStateListener listener)
        {
            if (gameStateListeners.Contains(listener))
                gameStateListeners.Remove(listener);
        }
    }


    public enum GameState
    {
        None = 0,
        MainMenu = 1,
        StartGame = 2,
        WavePhase = 3,
        BasePhase = 4,
        AttackPhase = 5,
        ExplorePhase = 6,
        EndGame = 7,
    }
}