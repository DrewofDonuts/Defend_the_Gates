using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Etheral
{
    /*
     * MusicDirector is a MonoBehaviour that manages the playback of music tracks in the game.
     * There should only ever be one music director with the same music object in the scene.
     */
    public class MusicDirector : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] List<MusicObject> musicObjects;
        [SerializeField] bool playOnStart;

        List<MusicTrack> musicTracksCache;

        [ReadOnly]
        public MusicObject currentMusicObject;

        float fadeDuration;

        void Start()
        {
            if (playOnStart)
                PlayTracks();

            foreach (var musicObject in musicObjects)
            {
                // Preload the music tracks
                PreloadMusic(musicObject);
            }
        }

        void OnEnable() => EtheralMessageSystem.OnMusicAction += PerformAction;
        void OnDisable() => EtheralMessageSystem.OnMusicAction -= PerformAction;

        public void PreloadMusic(MusicObject musicObject)
        {
            // Force the ScriptableObject to be deserialized
            _ = musicObject.SongName;

            // Force access to MusicTracks
            foreach (var track in musicObject.MusicTracks)
            {
                // Force reference resolution
                _ = track.trackName;
                _ = track.clip;
                _ = track.clip.loadState;

                // Optional: warm up the AudioClip if not streamed
                if (track.clip.loadType != AudioClipLoadType.Streaming && !track.clip.preloadAudioData)
                {
                    track.clip.LoadAudioData(); // async, non-blocking
                }
            }
        }


        void PerformAction(MusicActions action, string songName, string track)
        {
            switch (action)
            {
                case MusicActions.mute:
                    MuteTrack(track);
                    break;
                case MusicActions.unmute:
                    UnmuteTrack(track);
                    break;
                case MusicActions.play:
                    PlayTracks(songName);
                    break;
                case MusicActions.stop:
                    StopAllMusic();
                    break;
                default:
                    break;
            }
        }


        public void PlayTracks(string _songName = null)
        {
            Debug.Log($"Playing track {_songName}");
            if (_songName == null)
            {
                int index = Random.Range(0, musicObjects.Count);
                currentMusicObject = musicObjects[index];
            }
            else
            {
                foreach (var musicObject in musicObjects)
                {
                    if (musicObject.SongName == _songName)
                    {
                        currentMusicObject = musicObject;
                        break;
                    }
                }

                if (currentMusicObject == null)
                {
                    Debug.LogError($"No music object found with the name {_songName}");
                    return;
                }
            }

            MusicManager.Instance.PlayTrack(currentMusicObject.MusicData);
        }

        public void MuteTrack(string trackName)
        {
            MusicManager.Instance.MuteTrack(trackName);
        }

        public void UnmuteTrack(string trackName)
        {
            MusicManager.Instance.UnmuteTrack(trackName);
        }

        public void StopAllMusic()
        {
            MusicManager.Instance.StopAllMusic();
        }


#if UNITY_EDITOR

        [Button("PlayTracks")]
        void PlayTracksEditor()
        {
            PlayTracks();
        }


        [Button("MuteTrack")]
        void MuteTrackEditor(string trackName)
        {
            MuteTrack(trackName);
        }

        [Button("UnmuteTrack")]
        void UnmuteTrackEditor(string trackName)
        {
            UnmuteTrack(trackName);
        }


#endif
    }
}