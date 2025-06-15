using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral.DefendTheGates
{
    public class GameStateController : MonoBehaviour
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
            if (GameStateManager.Instance.CurrentGameState != GameState.BasePhase) return;
            holdProgress = UIFillUtility.UpdateRadialFill(radialFill, isHolding, holdProgress, holdTime, resetSpeed,
                OnStartWavePhase);

            // if (isHolding)
            // {
            //     holdProgress += Time.deltaTime / holdTime;
            //
            //     if (holdProgress >= 1f)
            //     {
            //         OnStartWavePhase();
            //         holdProgress = 0f;
            //         isHolding = false;
            //     }
            // }
            // else if (holdProgress > 0f)
            // {
            //     holdProgress -= Time.deltaTime * resetSpeed;
            // }
            //
            // radialFill.fillAmount = Mathf.Clamp01(holdProgress);
        }

        void OnStartWavePhase()
        {
            GameStateManager.Instance.SetGameState(GameState.WavePhase);
            GameStateManager.Instance.IncrementWave();
            isHolding = false; // Reset holding state
        }

        public void EndWavePhase()
        {
            StartCoroutine(EndWavePhaseCoroutine());
        }

        IEnumerator EndWavePhaseCoroutine()
        {
            yield return new WaitForSeconds(4f); // Wait for 1 second before transitioning
            GameStateManager.Instance.SetGameState(GameState.BasePhase);
        }
    }
}