using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class InjuredAudio : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] breathing;
        public AudioClip footDrag;
        public AudioClip[] footSteps;
        
        


        void Start()
        {
            audioSource.loop = false;
            audioSource.clip = breathing[0];
            audioSource.Play();
            
            StartCoroutine(IntermittentBreathing());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }


        IEnumerator IntermittentBreathing()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                audioSource.clip = breathing[Random.Range(0, breathing.Length)];
                audioSource.Play();
            }
        } 
    }
}