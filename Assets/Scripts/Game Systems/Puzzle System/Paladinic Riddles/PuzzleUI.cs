using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

namespace Etheral
{
    public class PuzzleUI : MonoBehaviour
    {
        [Header("Puzzle References")]
        [SerializeField] TextMeshProUGUI riddleText;
        [SerializeField] public GameObject puzzleUI;
        [SerializeField] GameObject solutionUI;
        [SerializeField] Color pressColor;

        [Header("References")]
        [SerializeField] InputUIObject inputObject;

        public Button buttonOne;
        public Button buttonTwo;
        public Button buttonThree;
        public Button buttonFour;
        public Button buttonFive;
        public Button buttonSix;
        public Button buttonSeven;
        public Button buttonEight;
        public Button lbButton;
        public Button rbButton;

        public string currentSolution;

        Color originalColor;

        //public for testing - later to be passed by PuzzleDetector
        [FormerlySerializedAs("currentPuzzle")] public PaladinicPuzzleRiddle currentPaladinicPuzzleRiddle;
        public bool IsActive { get; private set; }

        void Awake()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            // riddleText.text = currentPuzzle.RiddleText;

            buttonOne.Select();
            buttonOne.onClick.AddListener(() => AddToSolution(buttonOne, "sun"));
            buttonTwo.onClick.AddListener(() => AddToSolution(buttonTwo, "moon"));
            buttonThree.onClick.AddListener(() => AddToSolution(buttonThree, "dagger"));
            buttonFour.onClick.AddListener(() => AddToSolution(buttonFour, "shield"));
            buttonFive.onClick.AddListener(() => AddToSolution(buttonFive, "sword"));
            buttonSix.onClick.AddListener(() => AddToSolution(buttonSix, "eye"));
            buttonSeven.onClick.AddListener(() => AddToSolution(buttonSeven, "skull"));
            buttonEight.onClick.AddListener(() => AddToSolution(buttonEight, "chain"));

            inputObject.LBButtonEvent += LBButtonPressed;
            inputObject.RBButtonEvent += RBButtonPressed;

            inputObject.LBButtonCanceled += lbButton.onClick.Invoke;
            inputObject.RBButtonCanceled += rbButton.onClick.Invoke;

            lbButton.onClick.AddListener(StopPuzzle);
            rbButton.onClick.AddListener(SubmitSolution);

            puzzleUI.SetActive(false);
        }

        public void StartPuzzle(PaladinicPuzzleRiddle paladinicPuzzleRiddle)
        {
            gameObject.SetActive(true);
            IsActive = true;

            riddleText.text = Regex.Replace(paladinicPuzzleRiddle.RiddleText, @"\*\*(.*?)\*\*", "<b>$1</b>");

            // riddleText.text = puzzle.RiddleText;


            if (!paladinicPuzzleRiddle.CheckIfActive()) return;
            
            currentPaladinicPuzzleRiddle = paladinicPuzzleRiddle;
            currentPaladinicPuzzleRiddle.PuzzleIsActivated();
            puzzleUI.SetActive(true);
            currentSolution = "";

            EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.UIState);
        }

        void LBButtonPressed()
        {
            originalColor = lbButton.image.color;
            lbButton.image.color = pressColor;
        }

        void RBButtonPressed()
        {
            originalColor = rbButton.image.color;
            rbButton.image.color = pressColor;
        }

        void AddToSolution(Button button, string obj)
        {
            var selectedAnswer = Instantiate(button, solutionUI.transform);
            selectedAnswer.GetComponent<Button>().enabled = false;
            currentSolution += obj;
        }


        void StopPuzzle()
        {
            IsActive = false;
            lbButton.image.color = originalColor;
            currentPaladinicPuzzleRiddle = null;
            currentSolution = "";
            puzzleUI.SetActive(false);
            EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.MovementState);
        }

        void SubmitSolution()
        {
            if (currentPaladinicPuzzleRiddle.CheckSolution(currentSolution))
            {
                Debug.Log("Correct");

                //Play Audio
                //Have delay
                //Play Animation
                DestroyIncorrectResponses();
                StopPuzzle();
            }
            else
            {
                Debug.Log("Incorrect");
                currentSolution = "";

                DestroyIncorrectResponses();

                buttonOne.Select();
            }
        }

        void DestroyIncorrectResponses()
        {
            var selectedAnswers = solutionUI.GetComponentsInChildren<Transform>();

            foreach (var answer in selectedAnswers)
            {
                if (answer == solutionUI.transform)
                    continue;

                Destroy(answer.gameObject);
            }
        }

        void OnDisable()
        {
            buttonOne.onClick.RemoveAllListeners();
            buttonTwo.onClick.RemoveAllListeners();
            buttonThree.onClick.RemoveAllListeners();
            buttonFour.onClick.RemoveAllListeners();
            buttonFive.onClick.RemoveAllListeners();
            buttonSix.onClick.RemoveAllListeners();
            buttonSeven.onClick.RemoveAllListeners();
            buttonEight.onClick.RemoveAllListeners();
            lbButton.onClick.RemoveAllListeners();
            rbButton.onClick.RemoveAllListeners();

            inputObject.LBButtonEvent -= LBButtonPressed;
            inputObject.RBButtonEvent -= RBButtonPressed;

            inputObject.LBButtonCanceled -= lbButton.onClick.Invoke;
            inputObject.RBButtonCanceled -= rbButton.onClick.Invoke;

            inputObject.LBButtonEvent -= lbButton.onClick.Invoke;
            inputObject.RBButtonEvent -= rbButton.onClick.Invoke;
        }
    }
}