using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class CinematicManager : MonoBehaviour
    {
        public static CinematicManager Instance { get; private set; }
        [field: SerializeField] public List<PlayableDirectorHandler> PlayableDirectorHandlers { get; private set; } =
            new();
        [field: SerializeField] public ReferenceModelHandler ReferenceModelHandler { get; private set; }
        [field: SerializeField] public VirtualActor VirtualActor { get; private set; }
        [field: SerializeField] public FadeOutAndIn FadeOutAndIn { get; set; }
        public bool BlockAllControlsAndInput { get; set; }
        public event Action<bool> IsPlayingCinematic;
        Transform virtualActorTransform;

        public bool IsReady { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);


            if (FadeOutAndIn != null)
                FadeOutAndIn.gameObject.SetActive(false);
        }

        IEnumerator Start()
        {
            yield return new WaitUntil(() => ReferenceModelHandler != null);
            IsReady = true;
        }

        public void Register<T>(T register)
        {
            if (typeof(T) == typeof(PlayableDirectorHandler))
            {
                PlayableDirectorHandlers.Add(register as PlayableDirectorHandler);
            }

            if (typeof(T) == typeof(VirtualActor))
            {
                VirtualActor = register as VirtualActor;
            }

            if (typeof(T) == typeof(ReferenceModelHandler))
            {
                ReferenceModelHandler = register as ReferenceModelHandler;

                if (ReferenceModelHandler != null && ReferenceModelHandler.VirtualActor != null)
                    VirtualActor = ReferenceModelHandler.VirtualActor;
            }
        }

        public void Unregister<T>(T unregister)
        {
            if (typeof(T) == typeof(PlayableDirectorHandler))
            {
                PlayableDirectorHandlers.Remove(unregister as PlayableDirectorHandler);
            }

            if (typeof(T) == typeof(ReferenceModelHandler))
                ReferenceModelHandler = null;
        }

        public void TriggerCinematicEvent(EventKey eventKey)
        {
            if (!PlayableDirectorHandlers.Any(handler => handler.PlayableDirectorKey == eventKey))
            {
                Debug.Log("No Playable Director Handler found for this event key.");
                return;
            }

            InitializeVirtualActor();
            PlayTimeline(eventKey);
        }

        void InitializeVirtualActor()
        {
            //Get reference model to pass to Virtual Actor
            //Instantiate reference model
            var referenceModel = ReferenceModelHandler.GetModel();
            VirtualActor.SetVirtualModel(referenceModel);
            VirtualActor.InstantiateVirtualModel();
        }

        public void PlayTimeline(EventKey eventKey)
        {
            foreach (var playableDirectorHandler in PlayableDirectorHandlers)
            {
                if (playableDirectorHandler.PlayableDirectorKey == eventKey)
                {
                    //block all controls and input
                    BlockAllControlsAndInput = true;

                    virtualActorTransform = playableDirectorHandler.SetPlayerLocation.transform;
                    SetPlayerPosition();


                    //Play Playable Timeline Asset
                    playableDirectorHandler.PlayableDirector.Play();
                    ReferenceModelHandler.ToggleGameModel(false);

                    // Start coroutine to FadeOutAndIn 2 seconds before the end
                    StartCoroutine(FadeOutAndInBeforeEnd(playableDirectorHandler.PlayableDirector.duration));
                    FadeIn();

                    playableDirectorHandler.PlayableDirector.stopped += delegate
                    {
                        EndTimelineActions(playableDirectorHandler);
                    };
                }
            }
        }

        void EndTimelineActions(PlayableDirectorHandler playableDirectorHandler)
        {
            //Invoke End Timeline Event
            //unblock all controls and input
            //Destroy reference model

            // CharacterManager.Instance.SetPlayerPositionAndRotation(playableDirectorHandler.SetPlayerLocation.transform);

            ReferenceModelHandler.ToggleGameModel(true);
            BlockAllControlsAndInput = false;

            VirtualActor.DestroyVirtualModel();
            if (!playableDirectorHandler.IsRepeatable)
                Unregister(playableDirectorHandler);
        }

        IEnumerator FadeOutAndInBeforeEnd(double duration)
        {
            // Wait for the duration of the timeline minus two seconds
            yield return new WaitForSeconds((float)duration - FadeOutAndIn.FadeTime);

            // Start FadeOutAndIn
            FadeOutAndInRoutine();
        }

        public void GetTransform()
        {
            virtualActorTransform = VirtualActor.GetVirtualActorModelTransform();
        }

        [Button("Set Player Position")]
        public void SetPlayerPosition()
        {
            CharacterManager.Instance.SetPlayerPositionAndRotationForCinematic(virtualActorTransform);
        }

        public void FadeIn()
        {
            if (FadeOutAndIn.gameObject.activeSelf == false)
                FadeOutAndIn.gameObject.SetActive(true);
            FadeOutAndIn.FadeInImageRoutine();
        }

        public void FadeOut()
        {
            if (FadeOutAndIn.gameObject.activeSelf == false)
                FadeOutAndIn.gameObject.SetActive(true);
            FadeOutAndIn.FadeOutImageRoutine();
        }

        public void FadeOutAndInRoutine()
        {
            if (FadeOutAndIn.gameObject.activeSelf == false)
                FadeOutAndIn.gameObject.SetActive(true);
            FadeOutAndIn.FadeOutAndInImageRoutine();
        }
    }
}