using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    [InlineEditor]
    public class LeverPuzzleHandler : MonoBehaviour
    {
        [SerializeField] Resolver resolver;
        [SerializeField] List<LeverTrigger> leverTriggers;
        [SerializeField] float timeBeforeResolveOnSuccess = 0.5f;
        [SerializeField] int attemptsBeforeReset = 4;
        [SerializeField] UnityEvent onPuzzleComplete;

        [Header("Debug")]
        [TextArea(5, 10)]
        public string puzzleNotes;

        int currentAttempts;


        void Start()
        {
            foreach (var lever in leverTriggers)
            {
                lever.OnChanged += HandleLeverPulled;
            }
        }


        void OnDisable()
        {
            foreach (var lever in leverTriggers)
            {
                lever.OnChanged -= HandleLeverPulled;
            }
        }

        void HandleLeverPulled(LeverTrigger obj)
        {
            bool areAllDown = leverTriggers.All(x => x.IsActivated);

            currentAttempts++;

            if (areAllDown)
                StartCoroutine(ResolveAfterTime());
            // else if (currentAttempts >= attemptsBeforeReset)
            // {
            //     currentAttempts = 0;
            //     StartCoroutine(RestAllLevers());
            // }
        }
        
        IEnumerator RestAllLevers()
        {
            yield return new WaitForSeconds(1f);
            foreach (var lever in leverTriggers)
            {
                lever.Deactivated();
            }
        }

        IEnumerator ResolveAfterTime()
        {
            yield return new WaitForSeconds(timeBeforeResolveOnSuccess);
            resolver.Resolve();
            onPuzzleComplete?.Invoke();
        }
    }
}