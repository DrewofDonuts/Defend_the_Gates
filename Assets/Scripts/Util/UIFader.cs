using System.Collections;
using UnityEngine;

namespace Etheral
{
    public static class UIFader
    {
        public static void FadeTo(CanvasGroup canvasGroup, float targetAlpha, float duration, MonoBehaviour coroutineHost)
        {
            if (canvasGroup == null || coroutineHost == null)
                return;

            coroutineHost.StartCoroutine(FadeCoroutine(canvasGroup, targetAlpha, duration));
        }

         static IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float targetAlpha, float duration)
        {
            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha; // Ensure final value is set
        }
    }
}