using System;
using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class CanvasGroupFaderOnTrigger : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] bool shouldFadeOut;
        [SerializeField] float fadeInDuration = 2f;
        [SerializeField] float fadeOutDuration = 1.5f;


        void Start()
        {
            canvasGroup.alpha = 0f;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (canvasGroup.alpha >= .90f) return;
                StartCoroutine(FadeInUIText());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!shouldFadeOut) return;

            if (other.CompareTag("Player"))
            {
                StartCoroutine(FadeOutUIText());
            }
        }


        IEnumerator FadeInUIText()
        {
            canvasGroup.alpha = 0f;
            float elapsedTime = 0f;

            while (elapsedTime < fadeInDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        IEnumerator FadeOutUIText()
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }
    }
}