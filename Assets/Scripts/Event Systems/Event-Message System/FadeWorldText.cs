using System.Collections;
using Interfaces;
using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    public class FadeWorldText : MonoBehaviour, IGetTriggered
    {
        [Header("Settings")]
        [TextArea(3, 10)]
        [SerializeField] string inputText;
        [SerializeField] bool shouldFadeOut;
        [SerializeField] float fadeInDuration = 2f;
        [SerializeField] float fadeOutDuration = 1.5f;
        [SerializeField] bool doesNotRequireCondition;

        [Header("References")]
        [SerializeField] WorldUIText worldUI;


        public ITrigger Trigger { get; set; }


        bool isPlayerNear;

        void Start()
        {
            if (!inputText.IsNullOrWhitespace())
                worldUI.tmpText.text = inputText;

            worldUI.gameObject.SetActive(false);

            Trigger = GetComponent<ITrigger>();

            if (Trigger != null)
            {
                Trigger.OnTrigger += PlayerEntersArea;
                Trigger.OnReset += PlayerExitsArea;
            }
        }

        void OnDestroy()
        {
            if (Trigger != null)
            {
                Trigger.OnTrigger -= PlayerEntersArea;
                Trigger.OnReset -= PlayerExitsArea;
            }
        }


        public void PlayerEntersArea()
        {
            if (worldUI.tmpText == null) return;

            StopAllCoroutines();
            StartCoroutine(FadeInUIText());
        }


        public void PlayerExitsArea()
        {
            if (worldUI.tmpText == null) return;
            if (!shouldFadeOut) return;
            StopAllCoroutines();
            StartCoroutine(FadeOutUIText());
        }

        IEnumerator FadeInUIText()
        {
            worldUI.canvasGroup.alpha = 0f;
            worldUI.gameObject.SetActive(true);
            float elapsedTime = 0f;

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                worldUI.canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                yield return null;
            }

            worldUI.canvasGroup.alpha = 1f;
        }

        IEnumerator FadeOutUIText()
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                worldUI.canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                yield return null;
            }

            worldUI.canvasGroup.alpha = 0f;
            worldUI.gameObject.SetActive(false);
        }
    }
}