using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class AudioFXManager : MonoBehaviour
    {
        //instance of this class
        public static AudioFXManager Instance { get; private set; }

        [field: Header("SOURCES")]
        // [field: SerializeField] public AudioSource MusicSource { get; private set; }
        [field: SerializeField] public AudioSource AmbientAudioSource { get; private set; }

        [field: Header("CLIPS")]
        [field: SerializeField] public AudioClip[] AmbientClips { get; private set; }

        [field: Header(("SCENE DEFAULTS"))]
        [field: SerializeField] public SurfaceType SceneSurfaceDefault { get; private set; }

        [Header("AUDIO SETTINGS")]
        [SerializeField] public bool playMusic;

        [Header("Impact Settings")]
        [SerializeField] int MaxSimultaneousHits = 3; // Limit the number of hits
        [SerializeField] int currentHits = 0; // Track cu rent number of hits
        [SerializeField] float cooldownTime = 0.1f; // Minimum time between hits
        [SerializeField] float lastHitTime = 0f; // Time of the last hit played

        [Header("Audio Type Settings")]
        [SerializeField] AudioTypeSettings[] audioTypeSettings;


        Dictionary<AudioType, int> currentHitsPerType = new();
        Dictionary<AudioType, float> lastHitTimePerType = new();


        public Vector2 PitchRange => new(1f, 1f);
        public Vector2 PanRange => new(1f, 1f);

        [SerializeField] float _totalTime = 2f;


        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            if (Instance == null)
            {
                Instance = this;

                //Only works on root
                // DontDestroyOnLoad(gameObject);
            }

            foreach (var setting in audioTypeSettings)
            {
                currentHitsPerType[setting.type] = 0;
                lastHitTimePerType[setting.type] = 0f;
            }
            
            if (AmbientClips.Length > 0 && AmbientAudioSource != null) PlayAmbientSounds();
        }



        void PlayAmbientSounds()
        {
            AudioProcessor.PlayRandomOneShots(AmbientAudioSource, AmbientClips, PitchRange.x, PitchRange.y,
                PanRange.x, PanRange.y);
        }

        public bool CheckIfCanHitAndRegisterHitIfAvailable(AudioClip audioClip)
        {
            if (currentHits >= MaxSimultaneousHits) return false;
            if (Time.time - lastHitTime < cooldownTime) return false;

            float delay = audioClip.length;

            currentHits++;
            lastHitTime = Time.time;
            StartCoroutine(ResetHitCountAfterAudio(delay));

            return true;
        }

        IEnumerator ResetHitCountAfterAudio(float delay)
        {
            yield return new WaitForSeconds(delay);
            currentHits--;
        }

        
        public bool CheckIfAudioTypeLimitHasBeenMet(AudioClip audioClip, AudioType audioType)
        {
            if (audioType == AudioType.none) return true;
            
            var settings = audioTypeSettings.FirstOrDefault(x => x.type == audioType);
            if (settings == null) return false;

            if (currentHitsPerType[audioType] >= settings.maxSimultaneousHits) return false;
            if (Time.time - lastHitTimePerType[audioType] < settings.cooldownTime) return false;

            float delay = audioClip.length;

            currentHitsPerType[audioType]++;
            lastHitTimePerType[audioType] = Time.time;
            StartCoroutine(ResetHitCountAfterAudio(delay, audioType));

            return true;
        }

        IEnumerator ResetHitCountAfterAudio(float delay, AudioType audioType)
        {
            yield return new WaitForSeconds(delay);
            currentHitsPerType[audioType]--;
        }
    }

    [System.Serializable]
    public class AudioTypeSettings
    {
        public AudioType type;
        public int maxSimultaneousHits = 3;
        public float cooldownTime = 0.1f;
    }
}