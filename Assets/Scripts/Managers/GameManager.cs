using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PixelCrushers;
using Sirenix.Utilities;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class GameManager : MonoBehaviour, IInitialize
    {
        static GameManager _instance;
        public static GameManager Instance => _instance;

        public List<string> AllGameNames = new();
        public event Action OnSave;

        [SerializeField] GameData gameData;
        public GameData GameData => gameData;

        //Will only restore the player's position if the game is loading. Prevents the player from being teleported to the last saved position
        //when the player is not loading a game - like when player is HandlingSceneLoaded
        public bool IsLoading;
        public bool isGameStarted;
        public Action<string> OnSceneLoading;

        bool isNewGame;


        public void Initialize()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            SceneManager.sceneLoaded += HandleSceneLoaded;

            WriteAllGameNamesToFile();
        }


        void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        void WriteAllGameNamesToFile()
        {
            string path = Path.Combine(Application.persistentDataPath, "AllGameNames.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                AllGameNames = JsonConvert.DeserializeObject<List<string>>(json);
            }
            else
            {
                Debug.LogError("File not found: " + path);
            }
        }

        public void NewGame(string sceneName)
        {
            gameData = new GameData();

            if (string.IsNullOrWhiteSpace(gameData.GameName))
                gameData.GameName = "Game " + AllGameNames.Count;

            // Reset the dialogue database
            SaveSystem.ResetGameState();

            // SceneManager.LoadScene(sceneIndex);
            EtheralSceneManager.Instance.ChangeScene(sceneName);

            isNewGame = true;
        }

        public void SaveGame()
        {
            OnSave?.Invoke();

            if (string.IsNullOrWhiteSpace(gameData.GameName))
                gameData.GameName = "Game " + (AllGameNames.Count + 1);

            SaveDialogueQuestData();

            string json = JsonConvert.SerializeObject(gameData, new UnityJsonConverter());
            string path = Path.Combine(Application.persistentDataPath, gameData.GameName + ".json");
            File.WriteAllText(path, json);


            Debug.Log("Saving Game: " + path + " " + json);

            if (!AllGameNames.Contains(gameData.GameName))
                AllGameNames.Add(gameData.GameName);

            isNewGame = false;

            SavesAllGameNames();
        }

        void SaveDialogueQuestData()
        {
            gameData.dialogueQuestData = SaveSystem.Serialize(SaveSystem.RecordSavedGameData());
        }

        void LoadDialogueSystemDatabase()
        {
            if (gameData.dialogueQuestData != null)
                SaveSystem.ApplySavedGameData(SaveSystem.Deserialize<SavedGameData>(gameData.dialogueQuestData));
        }

        void SavesAllGameNames()
        {
            string json = JsonConvert.SerializeObject(AllGameNames);
            string path = Path.Combine(Application.persistentDataPath, "AllGameNames.json");
            File.WriteAllText(path, json);
        }

        public void LoadGame(string gameName)
        {
            IsLoading = true;
            string path = Path.Combine(Application.persistentDataPath, gameName + ".json");

            Debug.Log("Loading Game: " + path);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                gameData = JsonConvert.DeserializeObject<GameData>(json,
                    new JsonSerializerSettings { Converters = new List<JsonConverter> { new UnityJsonConverter() } });

                if (String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
                    gameData.CurrentLevelName = "Act_1.1";

                // Restore dialogue system state from the saved data
                LoadDialogueSystemDatabase();

                SceneManager.LoadScene(gameData.CurrentLevelName);
            }
            else
            {
                Debug.LogError("File not found: " + path);
            }
        }


        public void DeleteGame(string gameName)
        {
            string path = Path.Combine(Application.persistentDataPath, gameName + ".json");
            if (File.Exists(path))
            {
                File.Delete(path);
                AllGameNames.Remove(gameName);
                SavesAllGameNames();
            }
            else
            {
                Debug.LogError("File not found: " + path);
            }
        }

        public void ReloadGame() => LoadGame(gameData.GameName);


        void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            Debug.Log($"Scene Loaded: {arg0.name}");


            if (arg0.name == "Main Menu Scene")
            {
                Debug.Log("Main Menu Scene Loaded");
            }
            else
            {
                Debug.Log($"Handling Scene Loaded: {arg0.name}");

                
                OnSceneLoading?.Invoke(arg0.name);

                //save the current level name to the gameData
                gameData.CurrentLevelName = arg0.name;

                //check if Level Data level name is same as arg0, which is what we are loading
                var levelData = gameData.LevelData.FirstOrDefault(t => t.LevelName == arg0.name);

                //if it's null, pass arg0 name to levelData
                if (levelData == null)
                {
                    levelData = new LevelData() { LevelName = arg0.name };
                    gameData.LevelData.Add(levelData);
                }


                //Disabled to used collection instead 04/11/2025
                // BindSingle<RunBinder, PlayerAbilityAndResourceData>(gameData.playerAbilityAndResourceData);
                // BindSingle<HealController, HealData>(gameData.healData);
                BindSingle<QuestManager, QuestSaveData>(gameData.questSaveData);

                // BindAllDataTypes(levelData);
                // BindInventorySlots();
                ReInstantiateEntities();

                FindAndBindPlayer();
                IsLoading = false;

                //Disabled to avoid multiple saves until Save design is decided 04/11/2025
                // if (isNewGame)
                // {
                //     Debug.Log("Saving game");
                //     SaveGame();
                // }
            }
        }




        void FindAndBindPlayer()
        {
            if (EventBusPlayerController.PlayerStateMachine == null)
                return;
            var playerStateMachine = EventBusPlayerController.PlayerStateMachine;
            var player = playerStateMachine.GetComponent<PlayerHealth>();
            var healController = playerStateMachine.PlayerComponents.GetHealController();
            var statsBinder = playerStateMachine.PlayerComponents.GetStatsController().StatsBinder;

            HandlePlayersJoined(player, healController);
            HandlePlayerStats(statsBinder);

            var levelData = gameData.LevelData.FirstOrDefault(t => t.LevelName == SceneManager.GetActiveScene().name);

            if (levelData.PlayerPositionData.isSavedPosition)
            {
                player.RestorePosition(levelData.PlayerPositionData);
            }

            //Previous way to restore player position 04/12/2025
            //IsLoading only pertinent if we are saving on every scene change
            // if (IsLoading)
            // {
            //     player.RestorePosition();
            //     IsLoading = false;
            // }
        }

        void BindInventorySlots()
        {
            if (Inventory.Instance != null)
                Inventory.Instance.Bind(gameData.slotData);
        }

        void BindAllDataTypes(LevelData levelData)
        {
            BindCollection<EnemyStateMachine, EnemyData>(levelData.EnemyData);
            BindCollection<OldDoor, DoorData>(levelData.DoorData);

            // Bind<PlayerInventory, PlayerData>(gameData.playerDatas);
            // BindEntities<EntitySaver, EntityData>(levelData.EntityData);
        }

        // T is a MonoBehaviour that implements the IBind<D> interface, and D is a type that implements the INamed interface 
        void BindCollection<T, D>(List<D> datas) where T : MonoBehaviour, IBind<D> where D : INamed, new()
        {
            // Find all MonoBehaviours of type T in the scene.
            var monoBehaviorGameObjects = FindObjectsByType<T>(FindObjectsSortMode.None);

            // Iterate over each instance.
            foreach (var monoBehaviourGameObject in monoBehaviorGameObjects)
            {
                // Try to find a data object that matches the name of the instance against the name in the respective GameData
                var data = datas.FirstOrDefault(t => t.Name == monoBehaviourGameObject.name);

                // If no matching data object is found, create a new one and add it to the list.
                if (data == null)
                {
                    data = new D() { Name = monoBehaviourGameObject.name };
                    datas.Add(data);
                }

                // Bind the data object to the instance.
                monoBehaviourGameObject.Bind(data);
            }
        }

        public void BindSingle<T, D>(D data) where T : MonoBehaviour, IBind<D> where D : INamed, new()
        {
            // Find a single instance of type T in the scene.
            var monoBehaviourGameObject = FindAnyObjectByType<T>();

            if (monoBehaviourGameObject != null)
            {
                // Bind the data object to the instance.
                monoBehaviourGameObject.Bind(data);
            }
        }


        //TODO: Remove once new system with multiple players is implemented 04/11/2025
        // public void HandlePlayerJoined(PlayerHealth _player)
        // {
        //     var data = gameData.playerData;
        //     if (data == null)
        //         gameData.playerData = data;
        //
        //     var player = _player;
        //     player.Bind(gameData);
        //     gameData.isSavedData = true;
        // }

        public void HandlePlayersJoined(PlayerHealth _player, HealController _healController)
        {
            var data = gameData.playerDataList.FirstOrDefault(t => t.Name == _player.Name);
            var levelData = gameData.LevelData.FirstOrDefault(t => t.LevelName == SceneManager.GetActiveScene().name);

            if (data == null)
            {
                data = new PlayerData() { Name = _player.Name };
                if (gameData.playerDataList != null)
                    gameData.playerDataList.Add(data);
            }

            var player = _player;
            player.Bind(gameData);

            var healController = _healController;
            healController.Bind(data, gameData.isSavedData);


            // player.BindPositionData(levelData?.PlayerPositionData);

            gameData.isSavedData = true;
        }

        public void HandlePlayerStats(StatsBinder _statsBinder)
        {
            var data = gameData.playerAbilityAndResourceDataList.FirstOrDefault(t => t.Name == _statsBinder.Name);

            if (data == null)
            {
                data = new PlayerAbilityAndResourceData { Name = _statsBinder.Name };

                if (gameData.playerAbilityAndResourceDataList != null)
                    gameData.playerAbilityAndResourceDataList.Add(data);
            }

            var player = _statsBinder;

            player.Bind(data);
        }


        public void RegisterEntity(EntitySaver entity)
        {
            var levelData = gameData.LevelData.FirstOrDefault(t => t.LevelName == SceneManager.GetActiveScene().name);

            if (levelData != null)
                Debug.Log($"Level Data: {levelData.LevelName}");

            if (levelData != null)
            {
                if (entity.Key.IsNullOrWhitespace())
                    entity.SetKey();
                var data = levelData.EntityData.FirstOrDefault(entityData => entityData.Key == entity.Key);

                if (data == null)
                {
                    Debug.Log($"This shouldn't enter if data is not null");
                    string nameSubstring;
                    if (entity.name.Contains('('))
                        nameSubstring = entity.name.Substring(0, entity.name.IndexOf('('));
                    else
                        nameSubstring = entity.name;

                    data = new EntityData()
                    {
                        Name = nameSubstring, Key = entity.Key
                    };

                    if (entity.isPositionSave)
                        data.position = entity.transform.position;
                    if (entity.isRotationSave)
                        data.rotation = entity.transform.rotation;

                    levelData.EntityData.Add(data);
                }

                entity.Bind(data);
            }
        }

        public void ReInstantiateEntities()
        {
            var levelData = gameData.LevelData.FirstOrDefault(t => t.LevelName == SceneManager.GetActiveScene().name);
            if (levelData != null)
            {
                var onlyIsSpawned = levelData.EntityData.Where(t => t.isSpawned).ToList();

                foreach (var entityData in onlyIsSpawned)
                {
                    var entity = Instantiate(Resources.Load<GameObject>("Prefabs/" + entityData.Name))
                        .GetComponent<EntitySaver>();

                    EntityData entityData1 = new EntityData();

                    levelData.EntityData.Add(entityData1);

                    entity.transform.position = entityData.position;
                    entity.transform.rotation = entityData.rotation;

                    entity.Bind(entityData1);
                    levelData.EntityData.Remove(entityData);
                }

                levelData.EntityData.RemoveAll(t => t.Name.IsNullOrWhitespace());
            }
        }
    }
}