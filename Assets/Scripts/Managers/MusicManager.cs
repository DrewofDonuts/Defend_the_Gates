using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace Etheral
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] AudioMixerGroup audioMixerGroup;
        static MusicManager instance;
        public static MusicManager Instance => instance;

        const int MaxAudioSources = 5;
        Queue<AudioSource> availableSources = new();
        List<MusicTrack> playingTracks = new();

        [ReadOnly]
        public MusicData currentMusicData;

        bool isTransitioningToNextSong;
        float fadeDuration = 1f;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < MaxAudioSources; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = audioMixerGroup;
                source.playOnAwake = false;
                source.loop = false;
                availableSources.Enqueue(source);
            }
        }

        public void PlayTrack(MusicData musicData)
        {
            if (currentMusicData != null && currentMusicData.SongName == musicData.SongName)
                return;

            currentMusicData = musicData;
            playingTracks.Clear();

            foreach (var track in currentMusicData.MusicTracks)
                CacheMusicTrack(track);

            foreach (var track in playingTracks)
            {
                track.source.clip = track.clip;
                track.source.loop = track.loop;
                track.source.volume = track.isMuted ? 0 : 1;
                track.source.Play();
            }
        }

        void CacheMusicTrack(MusicTrack track)
        {
            if (availableSources.Count == 0)
                return;

            AudioSource pooledSource = availableSources.Dequeue();
            pooledSource.clip = track.clip;
            pooledSource.loop = track.loop;
            pooledSource.volume = track.isMuted ? 0 : 1;
            pooledSource.mute = track.isMuted;

            track.source = pooledSource;
            playingTracks.Add(track);
        }

        public void MuteTrack(string trackName)
        {
            MusicTrack track = FindTrack(trackName);
            if (track != null && !track.isMuted)
            {
                track.isMuted = true;
                track.source.mute = true; // Ensure the source is muted
                StopAllCoroutines();
                StartCoroutine(FadeTrackVolume(track, 0));
            }
            else
            {
                Debug.LogWarning($"Track {trackName} not found or already muted.");
            }
        }

        public void UnmuteTrack(string trackName)
        {
            MusicTrack track = FindTrack(trackName);
            if (track != null && track.isMuted)
            {
                track.isMuted = false;
                track.source.mute = false; // Ensure the source is not muted
                StopAllCoroutines();
                StartCoroutine(FadeTrackVolume(track, 1));
            }
            else
            {
                Debug.LogWarning($"Track {trackName} not found or already unmuted.");
            }
        }

        MusicTrack FindTrack(string trackName)
        {
            return playingTracks.Find(t => t.trackName == trackName);
        }

        IEnumerator FadeTrackVolume(MusicTrack track, float targetVolume, bool removeAfter = false)
        {
            AudioSource source = track.source;
            float startVolume = source.volume;
            float time = 0;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, targetVolume, time / fadeDuration);
                yield return null;
            }

            source.volume = targetVolume;

            if (removeAfter)
            {
                source.Stop();
                playingTracks.Remove(track);
                availableSources.Enqueue(source);
            }

            if (playingTracks.Count == 0)
                isTransitioningToNextSong = false;
        }

        public void PlayNextSong(MusicData nextSong)
        {
            isTransitioningToNextSong = true;
            StopAllCoroutines();
            StartCoroutine(MoveToNextSongAfterFadingOutCurrent(nextSong));
        }

        IEnumerator MoveToNextSongAfterFadingOutCurrent(MusicData nextSong)
        {
            foreach (var track in playingTracks)
                StartCoroutine(FadeTrackVolume(track, 0, true));

            while (isTransitioningToNextSong)
                yield return null;

            PlayTrack(nextSong);
        }

        public void StopAllMusic()
        {
            StopAllCoroutines();
            foreach (var track in playingTracks)
                StartCoroutine(FadeTrackVolume(track, 0, true));
        }
    }
}
