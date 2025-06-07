using System.Collections;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class RunSceneSelector : MessengerClass
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] Canvas canvas;
        [SerializeField] CanvasGroup canvasGroup;

        [Header("Scene Settings")]
        [SerializeField] AudioClip ready;

        RunSceneData runSceneData;
        public UnityEvent onTrigger;
        
        void Start()
        {
            runSceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);
            keyToReceive = runSceneData.KeyToReceive;
            
            canvas.gameObject.SetActive(false);
        }


        protected override void HandleReceivingKey()
        {
            base.HandleReceivingKey();
            OnEnemiesDead();
        }

        void OnTriggerEnter(Collider other)
        {
            if (isTriggered) return;
            if (!other.CompareTag("Player")) return;
            if (!isConditionSatisfied) return;
            isTriggered = true;
            
            Debug.Log($"Triggered {other.name}");
            
            LoadRandomScene();
        }

        [ContextMenu("Load Next Scenes")]
        public void LoadRandomScene()
        {
            if (runSceneData.SceneList.Length == 0)
            {
                Debug.LogError("No scenes to load.");
                return;
            }

            int randomIndex = Random.Range(0, runSceneData.SceneList.Length);

            EtheralSceneManager.Instance.ChangeScene(runSceneData.SceneList[randomIndex].SceneName);

            // EtheralSceneManager.Instance.ChangeScene(possibleNextScenes[randomIndex]);
        }

        void OnEnemiesDead()
        {
            StartCoroutine(FadeCanvasGroupWhenLevelCompletes(canvasGroup, 2f));
        }


        IEnumerator FadeCanvasGroupWhenLevelCompletes(CanvasGroup _canvasGroup, float duration)
        {
            canvas.gameObject.SetActive(true);
            PlayReadySound();
            float startAlpha = _canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(1.5f);
            onTrigger?.Invoke();

            float endAlpha = 0f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                _canvasGroup.alpha = newAlpha;
                yield return null;
            }

            isConditionSatisfied = true;
            _canvasGroup.alpha = endAlpha;
        }

        void PlayReadySound()
        {
            audioSource.PlayOneShot(ready);
        }
    }
}