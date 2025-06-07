using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class AdditiveSceneLoader : MonoBehaviour
    {
        [SerializeField] SceneData sceneData;
        [SerializeField] bool loadOnStart = true;

        void Start()
        {
            if (loadOnStart)
            {
                if (sceneData != null)
                {
                    SceneManager.LoadSceneAsync(sceneData.SceneName, LoadSceneMode.Additive);
                }
            }

            EventBusGameController.OnUnloadAdditiveScene += OnStartGame;
        }

        void OnDestroy()
        {
            EventBusGameController.OnUnloadAdditiveScene -= OnStartGame;
        }


        public void OnStartGame()
        {
            StartCoroutine(StartGameSequence());
        }

        IEnumerator StartGameSequence()
        {
            if (SceneManager.GetSceneByName(sceneData.SceneName).isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(sceneData.SceneName);
            }
        }
    }
}