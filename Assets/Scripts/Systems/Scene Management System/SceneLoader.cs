using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// The SceneLoader class is responsible for managing scene transitions when a player enters a specific area.
// It uses a SphereCollider to detect player triggers and interacts with the EtheralSceneManager to load scenes.

// Key Features:
// - **Scene Data**: Utilizes a SceneData object to store the name and key of the scene to be loaded.
// - **Condition Validation**: Automatically determines if a scene can be loaded based on the presence of a required key.
// - **Trigger Detection**: Detects when a player enters the collider and ensures conditions are met before triggering the scene load.
// - **Scene Loading**: Initiates the scene transition through the EtheralSceneManager with a slight delay.
// - **Validation**: Ensures the SphereCollider is set as a trigger during development.
// - **Debugging and Visualization**: Includes debug logs for trigger events and visualizes the collider in the editor using gizmos.

namespace Etheral
{
    [RequireComponent(typeof(SphereCollider))]
    public class SceneLoader : Trigger
    {
        [InlineButton("CreateSceneData", "New Scene Data")]
        [SerializeField] SceneData sceneToLoad;
        [SerializeField] bool isSave;
        [SerializeField] Transform positionToSavePlayer;

        void Start()
        {
            //isConditionSatisfied will be true if no key  is needed, and false if a key is needed
            isConditionSatisfied = string.IsNullOrWhiteSpace(sceneToLoad.SceneKey);
        }

        void OnValidate()
        {
            GetComponent<SphereCollider>().isTrigger = true;

            if (sceneToLoad != null && name != "Scene Loader: " + " " + sceneToLoad.SceneName)
                name = "Scene Loader: " + " " + sceneToLoad.SceneName;
        }

        void OnTriggerEnter(Collider other)
        {
            if (isTriggered) return;
            if (!other.CompareTag("Player")) return;
            if (!isConditionSatisfied) return;
            isTriggered = true;
            SendKeyAndTriggerEvents();


            // Save the player's position and rotation if the scene is saved and used to position the player on load when returning
            GameManager.Instance?.GameData.LevelData
                .FirstOrDefault(t => t.LevelName == SceneManager.GetActiveScene().name)?.PlayerPositionData
                .SetLastPlayerPositionForLevel(positionToSavePlayer.position, positionToSavePlayer.rotation);

            LoadScene();
        }


        void LoadScene()
        {
            EtheralSceneManager.Instance.ChangeScene(sceneToLoad.SceneName, .15f, isSave);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }

#if UNITY_EDITOR

        public void CreateSceneData()
        {
            var data = AssetCreator.NewSceneData();

            if (data != null)
                sceneToLoad = data;
        }

#endif
    }
}