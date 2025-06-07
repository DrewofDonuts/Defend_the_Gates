using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Etheral
{
    //solution key
    // up, down, left, right, north, south, east, west

    public class PaladinicPuzzleRiddle : MessengerClass
    {
        [Header("Puzzle Settings")]
        [Tooltip("Use to inform player about the puzzle")]
        [SerializeField] string puzzleInfo;
        [SerializeField] string riddleText;
        [SerializeField] string solution;

        [Header("References")]
        [SerializeField] TextMeshProUGUI puzzleInfoUIText;
        [SerializeField] CanvasGroup canvasGroup;

        [Header("Puzzle State")]
        [FormerlySerializedAs("isReadyToBeSolved")]

        [Tooltip("Is active while the puzzle is being solved")]
        [SerializeField] bool isActive;

        [SerializeField] UnityEvent onPuzzleSolved;
        [SerializeField] UnityEvent onPuzzleFailed;

        public string RiddleText => riddleText;


        public string cleanedSolution;

        void Awake()
        {
            // cleanedSolution = solution.ToLower().Replace(" ", "");

            if (puzzleInfoUIText == null)
            {
                Debug.LogError("TMP Item is not assigned");
                return;
            }

            puzzleInfoUIText.text = puzzleInfo;
            puzzleInfoUIText.gameObject.SetActive(false);
        }

        void OnValidate()
        {
            solution = solution.ToLower().Replace(" ", "");
            cleanedSolution = solution.ToLower().Replace(" ", "");
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (puzzleInfoUIText == null)
                    return;

                if (!isConditionSatisfied)
                    StartCoroutine(FadeInUIText());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (puzzleInfoUIText == null)
                    return;


                if (!isConditionSatisfied)
                    StartCoroutine(FadeOutUIText());
            }
        }

        public void PuzzleIsActivated()
        {
            isActive = true;
        }

        public bool CheckSolution(string proposedSolution)
        {
            if (isActive)
            {
                if (proposedSolution == cleanedSolution)
                {
                    Debug.Log("Puzzle Solved");
                    onPuzzleSolved.Invoke();
                    if (keyToSend != null)
                        SendKey();

                    if (message != null)
                        SendMessage();
                }
                else
                {
                    Debug.Log("Puzzle Failed");
                    onPuzzleFailed.Invoke();
                }
            }

            return proposedSolution == cleanedSolution;
        }

        [ContextMenu("Check If Active")]
        public bool CheckIfActive()
        {
            if (!isConditionSatisfied)
            {
                canvasGroup.alpha = 1f;
                puzzleInfoUIText.gameObject.SetActive(true);


                StartCoroutine(FadeOutUIText());
            }

            return isConditionSatisfied;
        }

        IEnumerator FadeInUIText()
        {
            puzzleInfoUIText.gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            float duration = 2f; // Duration of the fade
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        IEnumerator FadeOutUIText()
        {
            float duration = 2f; // Duration of the fade
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            puzzleInfoUIText.gameObject.SetActive(false);
        }
    }
}