using System;
using Etheral;
using Etheral.DefendTheGates;
using TMPro;
using UnityEngine;

namespace Ethereal.DefendTheGates
{
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
        }

        public void OnGameStateChanged(GameState newGameState)
        {
            if(GameStateManager.Instance.CurrentWave == 0) return;
            
            if (newGameState == GameState.BasePhase)
            {
                Debug.Log("GameState changed to BasePhase, updating resource UI.");
                resourceText.text = ResourceManager.Instance.LastWaveResources.ToString();
                UIFader.FadeTo(canvasGroup, 1f, fadeDuration, this); // Fade in
            }
            else if(canvasGroup.alpha > 0f)
            {
                Debug.Log("GameState changed from BasePhase, hiding resource UI.");
                UIFader.FadeTo(canvasGroup, 0f, fadeDuration, this); // Fade out
            }
        }

    }
}