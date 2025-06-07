using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


namespace Etheral
{
    public class EtheralSceneManager : MonoBehaviour
    {
        [SerializeField] GameObject loadPanel;
        [SerializeField] Image progressBar;
        [SerializeField] Image fadeImage;
        [SerializeField] Image loadingImage;
        [SerializeField] TMP_Text loadingText;
        [SerializeField] RunSceneData[] sceneData;

        static EtheralSceneManager _instance;
        public static EtheralSceneManager Instance => _instance;

        float target;


        // initialize references
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

            loadPanel.SetActive(false);
            fadeImage.enabled = false;

            loadingImage.enabled = false;
            loadingText.enabled = false;
        }


        public RunSceneData GetSceneData(string sceneName)
        {
            foreach (var scene in sceneData)
            {
                if (scene.SceneName == sceneName)
                {
                    return scene;
                }
            }

            return null;
        }


        public void RestartScene()
        {
            ChangeScene(SceneManager.GetActiveScene().name);
            Debug.Log("Restarting Scene from Player StateMachine");
        }

        public void LoadHubScene(float timeBeforeStarting, bool saveGame)
        {
            ChangeScene("Rogue_Hub", timeBeforeStarting, saveGame);
        }
        
        public void ChangeScene(string sceneName, float timeBeforeStarting = 0, bool saveGame = false)
        {
            // LoadScene(sceneName);

            Debug.Log("Changing Scene to " + sceneName);
            StartCoroutine(LoadingScene(sceneName, timeBeforeStarting, saveGame));
        }

        IEnumerator LoadingScene(string sceneName, float timeBeforeStarting, bool saveGame)
        {
            yield return new WaitForSeconds(timeBeforeStarting);

            // Fade to black
            float alpha = 0;
            fadeImage.enabled = true;

            while (alpha < 1)
            {
                alpha += Time.deltaTime * 2f;
                fadeImage.color = new Color(0, 0, 0, Mathf.Min(alpha, 1));
                yield return null;
            }

            loadPanel.SetActive(true);
            loadingImage.enabled = true;
            loadingText.enabled = true;

            // Load scene
            var asyncScene = SceneManager.LoadSceneAsync(sceneName);

            float totalProgress = 0f;

            // while (!asyncScene.isDone)
            // {
            //     //Disabling Loading Bar for now 01/09/25
            //     // totalProgress = asyncScene.progress;
            //     // progressBar.fillAmount = totalProgress;
            //     yield return null;
            // }

            yield return new WaitUntil(() => asyncScene.progress >= 0.99f);

            // Fade back out
            while (alpha > 0)
            {
                alpha -= Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, alpha);
                loadingImage.color = new Color(0, 0, 0, alpha);

                loadPanel.SetActive(false);
                loadingText.enabled = false;
                loadingImage.enabled = false;

                yield return null;
            }

            fadeImage.enabled = false;
            loadingImage.color = Color.white;

            if (saveGame)
                GameManager.Instance.SaveGame();

            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
        }
    }
}