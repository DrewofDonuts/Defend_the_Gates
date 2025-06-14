using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class FadeCanvas : MonoBehaviour
    {
        [SerializeField] bool fadeOnStart = true;
        [SerializeField] FadeType fadeType;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float fadeDuration = 1f;

        void Start()
        {
            if (fadeOnStart)
            {
                switch (fadeType)
                {
                    case FadeType.FadeAlphaToZero:
                        canvasGroup.alpha = 1f; // Start with canvas visible
                        FaadeToZero();
                        break;
                    case FadeType.FadeAlphaToOne:
                        canvasGroup.alpha = 0f; // Start with canvas hidden
                        FadeToOne();
                        break;
                }
            }
        }
        
        public void FadeToOne()
        {
            StartCoroutine(Fade(canvasGroup.alpha, 1f));
        }

        public void FaadeToZero()
        {
            StartCoroutine(Fade(1, 0f));
        }

        IEnumerator Fade(float startAlpha, float endAlpha)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                canvasGroup.alpha = alpha;
                yield return null;
            }

            canvasGroup.alpha = endAlpha; // Ensure final value is set
        }
    }

    public enum FadeType
    {
        FadeAlphaToZero,
        FadeAlphaToOne
    }
}