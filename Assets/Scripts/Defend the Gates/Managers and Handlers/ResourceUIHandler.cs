using System;
using System.Collections;
using Etheral;
using Etheral.DefendTheGates;
using TMPro;
using UnityEngine;

namespace Ethereal.DefendTheGates
{
    //Updates the EndWave UI with the resources earned during the last wave.
    //Hooks with ResourceManager to get the last wave resources and updates the UI accordingly.

    public class ResourceUIHandler : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float fadeDuration = 1f;
        [SerializeField] TextMeshProUGUI resourceText;

        void Start()
        {
            canvasGroup.alpha = 0f; // Start with canvas hidden
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            ResourceManager.Instance.OnResourcesChanged += OnDisplayResources;
        }

        void OnDisable() => ResourceManager.Instance.OnResourcesChanged -= OnDisplayResources;
 

        public void OnDisplayResources(int resourcesGained)
        {
            if (GameStateManager.Instance.CurrentWave == 0) return;

            resourceText.text = resourcesGained.ToString();
            UIFader.FadeTo(canvasGroup, 1f, fadeDuration, this); // Fade in

            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(3f);
            UIFader.FadeTo(canvasGroup, 0f, fadeDuration, this); // Fade out
        }
    }
}