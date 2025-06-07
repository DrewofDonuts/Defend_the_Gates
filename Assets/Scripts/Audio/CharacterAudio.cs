using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class CharacterAudio : MonoBehaviour
    {
        [FormerlySerializedAs("attackEmoteTimer")]
        [Header("Audio Settings")]
        [SerializeField] float attackEmoteCooldownTime = 1.5f;

        [field: Header("Audio Library")]
        [field: SerializeField] public AudioLibrary AudioLibrary { get; private set; }

        [field: Header("Audio Sources")]
        [field: SerializeField] public AudioSource BlockSource { get; private set; }
        [SerializeField] AudioSource dialogueSource;
        [field: SerializeField] public AudioSource EmoteSource { get; private set; }
        [field: SerializeField] public AudioSource EquipmentSource { get; private set; }
        [field: SerializeField] public AudioSource ImpactSource { get; private set; }
        [field: SerializeField] public AudioSource LocomotionSource { get; private set; }
        [field: SerializeField] public AudioSource OneShotSource { get; private set; }
        [field: SerializeField] public AudioSource WeaponSource { get; private set; }
        [field: SerializeField] public AudioSource RangedDamageSource { get; private set; }
        [field: SerializeField] public AudioSource MagicSource { get; private set; }


        [SerializeField] DialogueAudioObject dialogueAudioObject;

        public Vector2 PitchRange { get; private set; } = new Vector2(1f, 1f);
        public SurfaceType CurrentSurface { get; internal set; }
        public AudioImpact ImpactType { get; internal set; }

        public AudioSelector AudioSelector => audioSelector;

        internal AudioClip[] currentRunFootsteps;
        internal AudioClip[] currentWalkFootsteps;
        internal AudioClip[] currentImpact;
        readonly AudioSelector audioSelector;

        float lastAttackTime;


        public CharacterAudio()
        {
            audioSelector = new AudioSelector(this);
        }

        IEnumerator Start()
        {
            // currentWalkFootsteps = AudioLibrary.GravelSurfaceWalking;
            // currentRunFootsteps = AudioLibrary.GravelSurfaceRunning;
            CheckIfAnyAudioSourcesAreNull();

            CurrentSurface = SurfaceType.Gravel;

            yield return new WaitUntil(() => AudioFXManager.Instance != null && DialogueSceneManager.Instance != null);
            CurrentSurface = AudioFXManager.Instance.SceneSurfaceDefault;

            if (AudioLibrary == null)
                Debug.LogError("Audio Library is null");
            else
                AudioSelector.DetectAndSetFootstep(CurrentSurface);


            DialogueSceneManager.Instance.SetAudioSource(EmoteSource);
        }

        public void PlayDialogueClip(int id)
        {
            dialogueSource.clip = dialogueAudioObject.dialogues.FirstOrDefault(x => x.id == id).audioClip;

            // dialogueSource.clip = dialogueAudioObject.dialogueData[index].audioClips;
            dialogueSource.Play();
        }

        #region Play Audio Methods
        public void PlayStep(int runWalk)
        {
            //if 0 - walk if 1 -  run

            if (runWalk == 0)
                AudioProcessor.PlayRandomOneShots(LocomotionSource, currentWalkFootsteps, PitchRange.x, PitchRange.y);
            else if (runWalk == 1)
                AudioProcessor.PlayRandomOneShots(LocomotionSource, currentRunFootsteps, PitchRange.x, PitchRange.y);
            else
                AudioProcessor.PlayRandomOneShots(LocomotionSource, currentWalkFootsteps, PitchRange.x, PitchRange.y);
        }

        public void PlayEmote(AudioClip audioClip, AudioType audioType = AudioType.none)
        {
            if (audioType == AudioType.attackEmote)
            {
                if (Time.time - lastAttackTime < attackEmoteCooldownTime)
                    return;
            }

            lastAttackTime = Time.time;
            AudioProcessor.PlaySingleOneShot(EmoteSource, audioClip, audioType, PitchRange.x, PitchRange.y);
        }

        public void PlayRandomEmote(AudioClip[] audioClips, AudioType audioType = AudioType.none)
        {
            if (audioClips.Length <= 0) return;

            if (audioType == AudioType.attackEmote)
            {
                if (Time.time - lastAttackTime < attackEmoteCooldownTime)
                    return;
            }

            lastAttackTime = Time.time;

            AudioProcessor.PlayRandomClips(EmoteSource, audioClips, PitchRange.x, PitchRange.y, 1, 1, audioType);
        }

        public void PlayOneShot(AudioSource source, AudioClip audioClip, AudioType audioType = AudioType.none, float minPitch = 1f, float maxPitch = 1f,
            float minVolume = 1f, float MaxVolume = 1f)
        {
            AudioProcessor.PlaySingleOneShot(source, audioClip, audioType, minPitch, maxPitch, minVolume, MaxVolume);
        }

        public void PlayRandomOneShot(AudioSource source, AudioClip[] audioClips, AudioType audioType,
            float minPitch = 1f, float maxPitch = 1f,
            float minVolume = 1f, float MaxVolume = 1f)
        {
            AudioProcessor.PlayRandomClips(source, audioClips, PitchRange.x, PitchRange.y, minVolume, MaxVolume,
                audioType);
        }

        public void PlayClip(AudioClip audioClip)
        {
            AudioProcessor.PlaySingleClip(RangedDamageSource, audioClip);
        }

        public void PlayCurrentImpact()
        {
            var audioIndex = Random.Range(0, currentImpact.Length);
            AudioType audioType = AudioType.impact;

            AudioProcessor.PlaySingleOneShot(ImpactSource, currentImpact[audioIndex], audioType, .85f, 1.15f, .5f, 1f);
        }
        #endregion

        void CheckIfAnyAudioSourcesAreNull()
        {
            if (LocomotionSource == null)
                Debug.LogError("LocomotionSource is null");
            if (EmoteSource == null)
                Debug.LogError("EmoteSource is null");
            if (OneShotSource == null)
                Debug.LogError("OneShotSource is null");
            if (WeaponSource == null)
                Debug.LogError("WeaponSource is null");
            if (BlockSource == null)
                Debug.LogError("BlockSource is null");
            if (ImpactSource == null)
                Debug.LogError("ImpactSource is null");
            if (EquipmentSource == null)
                Debug.LogError("EquipmentSource is null");
        }

#if UNITY_EDITOR

        [Button("Player Impact Audio")]
        public void PlayPlayerImpactAudio()
        {
            currentImpact = AudioLibrary.BladeDamage;
            PlayCurrentImpact();
        }

        [Button("Load Audio Sources")]
        public void LoadAudioSources()
        {
            BlockSource = GameObject.Find("Block Source").GetComponentInChildren<AudioSource>();
            dialogueSource = GameObject.Find("Dialogue Source").GetComponentInChildren<AudioSource>();
            EmoteSource = GameObject.Find("Emote Source").GetComponentInChildren<AudioSource>();
            EquipmentSource = GameObject.Find("Equipment Source").GetComponentInChildren<AudioSource>();
            ImpactSource = GameObject.Find("Impact Source").GetComponentInChildren<AudioSource>();
            LocomotionSource = GameObject.Find("Locomotion Source").GetComponentInChildren<AudioSource>();
            OneShotSource = GameObject.Find("OneShot Source").GetComponentInChildren<AudioSource>();
            WeaponSource = GameObject.Find("Weapon Source").GetComponentInChildren<AudioSource>();
            RangedDamageSource = GameObject.Find("Ranged Weapon Source").GetComponentInChildren<AudioSource>();
            MagicSource = GameObject.Find("Magic Source").GetComponentInChildren<AudioSource>();
            
        }

#endif
    }
}