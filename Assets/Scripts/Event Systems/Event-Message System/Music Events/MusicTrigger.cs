using System.Collections.Generic;
using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class MusicTrigger : MonoBehaviour, IGetTriggered
    {
        [SerializeField] List<MusicActions> musicActions;
        [SerializeField] MusicObject musicObject;

        [ValueDropdown(nameof(GetTrackNames))]
        [SerializeField] List<string> muteTrackNames;
        [ValueDropdown(nameof(GetTrackNames))]
        [SerializeField] List<string> unmuteTrackNames;
        
            
        
        public ITrigger Trigger { get; set; }

        void Start()
        {
            Trigger = GetComponent<ITrigger>();

            if (Trigger != null)
                Trigger.OnTrigger += PerformAction;
        }

        void OnDisable() =>
            Trigger.OnTrigger -= PerformAction;


        void PerformAction()
        {
            foreach (var action in musicActions)
            {
                switch (action)
                {
                    case MusicActions.mute:
                        foreach (var trackName in muteTrackNames)
                        {
                            EtheralMessageSystem.SendMusicAction(MusicActions.mute, musicObject.SongName, trackName);
                        }

                        break;
                    case MusicActions.unmute:
                        foreach (var trackName in unmuteTrackNames)
                        {
                            EtheralMessageSystem.SendMusicAction(MusicActions.unmute, musicObject.SongName, trackName);
                        }

                        break;
                    case MusicActions.play:
                        EtheralMessageSystem.SendMusicAction(MusicActions.play, musicObject.SongName, "");
                        break;
                    case MusicActions.stop:
                        EtheralMessageSystem.SendMusicAction(MusicActions.stop, musicObject.SongName, "");
                        break;
                    default:
                        break;
                }
            }
        }


        IEnumerable<string> GetTrackNames()
        {
            if (musicObject == null || musicObject.MusicTracks == null)
                return new List<string>();

            return musicObject.MusicTracks.ConvertAll(track => track.trackName);
        }
    }

    public enum MusicActions
    {
        nothing = 0,
        mute = 1,
        unmute = 2,
        play = 3,
        stop = 4,
    }
}