using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class AdditiveSceneLoaderOnEnter : MonoBehaviour
    {
        public string[] scenesToLoad;
        public string[] scenesToUnload;

        public string loadSingleScene;
        
        public bool isAdditive = true;
        

         void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player entered trigger");
                
                if (isAdditive)
                    LoadScenes();
                else
                    LoadNextScene();
            }
        }

        void LoadNextScene()
        {
            // SceneManagerEtheral.Instance.LoadLevel(scenesToLoad[0]);
            
            SceneManager.LoadScene(loadSingleScene, LoadSceneMode.Single);
        }


        // You can also call this method from another script or event
        public void LoadScenes()
        {
            foreach (string sceneName in scenesToLoad)
            {
                if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    StartCoroutine(LoadScenesAsync(sceneName));
                }
                else
                {
                    Debug.LogWarning($"Scene {sceneName} is already loaded.");
                }
            }
        }

        IEnumerator LoadScenesAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void UnloadScenes()
        {
            foreach (string sceneName in scenesToUnload)
            {
                if (SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    SceneManager.UnloadSceneAsync(sceneName);
                }
                else
                {
                    Debug.LogWarning($"Scene {sceneName} is not loaded.");
                }
            }
        }
    }
}