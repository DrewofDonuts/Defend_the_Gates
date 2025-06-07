using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace Etheral
{
    public class ObjectiveBasedCinematics : MonoBehaviour
    {
        [Header("Playable References")]
        [SerializeField] PlayableDirector startLevelCinematic;
        [SerializeField] PlayableDirector endLevelCinematic;
        [SerializeField] List<EndLevelCinematics> possibleEndCinematics;

        [Header("Scene References")]
        [SerializeField] SceneObjectiveHandler sceneObjectiveHandler;

        List<PlayableDirector> achievedEndCinematics = new();
        int currentTimelineIndex;


        void Start()
        {
            achievedEndCinematics.Add(endLevelCinematic);
        }

        public void PlayStartLevelCinematic()
        {
            startLevelCinematic.Play();
        }

        public void CheckAchievedCinematics()
        {
            foreach (var endCinematic in possibleEndCinematics)
            {
                if (sceneObjectiveHandler.CheckIfObjectiveComplete(endCinematic.PlayableDirectorKey))
                {
                    achievedEndCinematics.Add(endCinematic.PlayableDirector);
                }
            }
        }

        public void PlayEndLevelCinematic()
        {
            if (currentTimelineIndex < achievedEndCinematics.Count)
            {
                achievedEndCinematics[currentTimelineIndex].stopped += OnTimelineStopped;
                achievedEndCinematics[currentTimelineIndex].Play();
            }
        }

        void OnTimelineStopped(PlayableDirector obj)
        {
            obj.stopped -= OnTimelineStopped;
            currentTimelineIndex++;
            if (currentTimelineIndex < achievedEndCinematics.Count)
            {
                PlayEndLevelCinematic();
            }
            else
            {
                Debug.Log("All cinematics played");

                // End level
            }
        }
    }


    [Serializable]
    public class EndLevelCinematics
    {
        public PlayableDirector PlayableDirector;
        public EventKey PlayableDirectorKey;
    }
}