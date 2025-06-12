using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral.DefendTheGates
{
    public class WaveInitializer : MonoBehaviour
    {
        [SerializeField] Canvas waveCanvas;
        [SerializeField] InputObject inputObject;
        [SerializeField] Image radialFill;
        [SerializeField] float holdTime = 2f;
        [SerializeField] float resetSpeed = 2f;


        public float holdProgress;
        public bool isHolding;


        void Start()
        {
            inputObject.ModeSelectEvent += OnModeSelectPressed;
            inputObject.ModeSelectCancelEvent += OnModeSelectPressed;
            radialFill.fillAmount = 0f; // Initialize fill amount
        }

        void OnDisable()
        {
            inputObject.ModeSelectEvent -= OnModeSelectPressed;
            inputObject.ModeSelectCancelEvent -= OnModeSelectPressed;
        }


        void OnModeSelectPressed()
        {
            if (GameStateManager.Instance.CurrentGameState != GameState.BasePhase) return;


            isHolding = inputObject.IsWaveToggle;
        }


        void Update()
        {
            if (isHolding)
            {
                holdProgress += Time.deltaTime / holdTime;

                if (holdProgress >= 1f)
                {
                    OnComplete();
                    holdProgress = 0f;
                    isHolding = false;
                }
            }
            else if (holdProgress > 0f)
            {
                holdProgress -= Time.deltaTime * resetSpeed;
            }

            radialFill.fillAmount = Mathf.Clamp01(holdProgress);
        }


        void OnComplete()
        {
            GameStateManager.Instance.SetGameState(GameState.WavePhase);
        }
    }
}