using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class Speaker : MonoBehaviour
    {
        [field: SerializeField] public CharacterKey CharacterKey { get; private set; }

        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }

        [field: SerializeField] public bool IsSpeaking { get; private set; }
        public string SpeakerName => CharacterKey.CharacterName;

        public void StartSpeaking()
        {
            IsSpeaking = true;
        }

        public void StopSpeaking()
        {
            IsSpeaking = false;
        }

        public void PlayAnimation(string animationName)
        {
            Animator.Play(animationName);
        }

        public void PlayAudioClip()
        {
            AudioSource.PlayOneShot(AudioClip);
        }

        public void StopAudioClip()
        {
            AudioSource.Stop();
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            AudioSource.clip = audioClip;
        }

#if UNITY_EDITOR



#endif
    }
}