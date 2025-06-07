using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class PuzzleHandler : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] PuzzleUI puzzleUI;

        [FormerlySerializedAs("currentPuzzle")] public PaladinicPuzzleRiddle currentPaladinicPuzzleRiddle;
        public bool hasPuzzle;

        void OnEnable() =>
            inputObject.SouthButtonEvent += SouthButtonPressed;

        void OnDisable() =>
            inputObject.SouthButtonEvent -= SouthButtonPressed;


        void SouthButtonPressed()
        {
            if (hasPuzzle && !puzzleUI.IsActive)
                StartCoroutine(DelayStartPuzzle());
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PaladinicPuzzleRiddle>(out var puzzle))
            {
                currentPaladinicPuzzleRiddle = puzzle;
                hasPuzzle = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PaladinicPuzzleRiddle>(out var puzzle))
            {
                currentPaladinicPuzzleRiddle = null;
                hasPuzzle = false;
            }
        }

        IEnumerator DelayStartPuzzle()
        {
            Debug.Log(" Attempt Puzzle");
            yield return new WaitForSeconds(0.25f);
            puzzleUI.StartPuzzle(currentPaladinicPuzzleRiddle);
            
            currentPaladinicPuzzleRiddle = null;
            hasPuzzle = false;
        }
    }
}