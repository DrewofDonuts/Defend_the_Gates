using UnityEngine;

namespace Etheral
{
    public static class AudioProcessor
    {
        public static void PlayRandomClips(AudioSource source, AudioClip[] clips, float pitchMin = 1f,
            float pitchMax = 1f, float volumeMin = 1f, float volumeMax = 1f, AudioType audioType = AudioType.none)
        {
            if (clips == null) return;
            if (clips.Length < 2)
            {
                PlaySingleClip(source, clips[0], pitchMin, pitchMax, volumeMin, volumeMax, audioType);
                return;
            }

            var newValue = RetrieveRandomVolumePitchIndex(source, clips, pitchMin, pitchMax, volumeMin, volumeMax);

            //if it's a repeat clip, get a new value
            while (source.clip == clips[newValue])
            {
                newValue = Random.Range(0, clips.Length);
            }

            source.clip = clips[newValue];

            CheckMangerBeforePlay(source, clips[newValue], audioType, true);
        }


        public static void PlayRandomOneShots(AudioSource source, AudioClip[] clips, float pitchMin = 1f,
            float pitchMax = 1f, float volumeMin = 1f, float volumeMax = 1f, AudioType audioType = AudioType.none)
        {
            if (clips == null) return;
            if (clips.Length < 2)
            {
                PlaySingleOneShot(source, clips[0], audioType, pitchMin, pitchMax);
                return;
            }

            var newValue = RetrieveRandomVolumePitchIndex(source, clips, pitchMin, pitchMax, volumeMin, volumeMax);


            //if it's a repeat clip, get a new value
            while (source.clip == clips[newValue])
            {
                newValue = Random.Range(0, clips.Length);
            }

            source.clip = clips[newValue];

            CheckMangerBeforePlay(source, clips[newValue], audioType, true);
        }

        public static void PlaySingleClip(AudioSource source, AudioClip clip, float pitchMin = 1f,
            float pitchMax = 1f, float volumeMin = 1f, float volumeMax = 1f, AudioType audioType = AudioType.none)
        {
            source.pitch = Random.Range(pitchMin, pitchMax);

            if (clip == null) return;
            source.clip = clip;
            source.Play();
        }

        public static void PlaySingleOneShot(AudioSource source, AudioClip clip, AudioType audioType, float pitchMin = 1f,
            float pitchMax = 1f, float volumeMin = 1f, float volumeMax = 1f)
        {
            source.pitch = Random.Range(pitchMin, pitchMax);

            if (clip == null) return;
            source.clip = clip;
            source.volume = Random.Range(volumeMin, volumeMax);

            CheckMangerBeforePlay(source, clip, audioType, false);
        }


        static void CheckMangerBeforePlay(AudioSource source, AudioClip clip, AudioType audioType,
            bool isOneShot)
        {
            if (AudioFXManager.Instance != null)
                if (!AudioFXManager.Instance.CheckIfAudioTypeLimitHasBeenMet(clip, audioType))
                {
                    return;
                }
            if (isOneShot)
                source.PlayOneShot(source.clip);
            else
                source.Play();
        }

        static int RetrieveRandomVolumePitchIndex(AudioSource source, AudioClip[] clips, float pitchMin, float pitchMax,
            float volumeMin, float volumeMax)
        {
            source.pitch = Random.Range(pitchMin, pitchMax);
            source.volume = Random.Range(volumeMin, volumeMax);
            int newValue = Random.Range(0, clips.Length);
            return newValue;
        }
    }
}