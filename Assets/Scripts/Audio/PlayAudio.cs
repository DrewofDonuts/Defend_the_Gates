using System;
using UnityEngine;


namespace Etheral
{
    public class PlayAudio : MonoBehaviour
    {
        public AudioClip audioClip;
        public AudioSource audioSource;


        void OnEnable()
        {
            Debug.Log("PlayAudio enabled");
        }

        public void SetClipAndSource(AudioClip clip)
        {
            audioClip = clip;
        }

        [ContextMenu("Play Audio Clip")]
        public void PlayAudioClip(bool destroyWhenDone = false)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            
            Debug.Log("Clip length: " + audioClip.length);
            
            if(destroyWhenDone)
            {
                Destroy(gameObject, audioClip.length);
            }
        }
    }
}