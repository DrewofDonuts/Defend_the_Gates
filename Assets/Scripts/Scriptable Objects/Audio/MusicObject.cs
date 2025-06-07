using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Audio/MusicObject", fileName = "MusicData")]
    [InlineEditor]
    public class MusicObject : ScriptableObject
    {
        [SerializeField] MusicData musicData;
        
        public MusicData MusicData => musicData;
        public float FadeDuration => musicData.FadeDuration;
        public List<MusicTrack> MusicTracks => musicData.MusicTracks;
        public string SongName => musicData.SongName;


#if UNITY_EDITOR
        void OnValidate()
        {
            RenameAssetToSongName();
        }


        void RenameAssetToSongName()
        {
            if (string.IsNullOrEmpty(SongName)) return;
            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            string newPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), SongName + ".asset");
            UnityEditor.AssetDatabase.RenameAsset(path, SongName);
        }
#endif
    }


    [Serializable]
    public class MusicData
    {
        [field: Header("Settings")]
        [SerializeField] string songName;
        [SerializeField] float fadeDuration = 1f;
        
        [field: Header("References")]
        [SerializeField] List<MusicTrack> musicTracks;

        public float FadeDuration => fadeDuration;
        public List<MusicTrack> MusicTracks => musicTracks;
        public string SongName => songName;
        
        public MusicData() {}

        //Constructor for deep copy
        public MusicData(MusicData other)
        {
            songName = other.songName;
            fadeDuration = other.fadeDuration;
            musicTracks = new List<MusicTrack>();
            foreach (var track in other.musicTracks)
                musicTracks.Add(new MusicTrack(track));
        }

        public void SetSongName(string name) => songName = name;
        public void SetFadeDuration(float duration) => fadeDuration = duration;
        
    }


    [Serializable]
    public class MusicTrack
    {
        public string trackName;
        public TrackType trackType;
        public AudioClip clip;

        // public bool playOnStart;
        public bool isMuted = true;
        public bool loop = true;

        // [HideInInspector]
        public AudioSource source;
        
        
        
        public MusicTrack() {}

        // Constructor for deep copy
        public MusicTrack(MusicTrack other)
        {
            trackName = other.trackName;
            trackType = other.trackType;
            clip = other.clip;
            isMuted = other.isMuted;
            loop = other.loop;
            source = null; // We always assign source later
        }
    }
}