using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Etheral
{
    public class PlayableDirectorHandler : MonoBehaviour
    {
        [field: SerializeField] public PlayableDirector PlayableDirector { get; private set; }
        [field: SerializeField] public EventKey PlayableDirectorKey { get; private set; }
        [field: SerializeField] public SetPlayerLocation SetPlayerLocation { get; private set; }

        [field: SerializeField] public bool IsRepeatable { get; private set; }
        [field: SerializeField] public bool IsTriggered { get; private set; }


        IEnumerator Start()
        {
            yield return new WaitUntil(() => CinematicManager.Instance != null);
            Register();
        }

        public void Register()
        {
            CinematicManager.Instance.Register(this);
        }

        public void PlayTimeLine()
        {
            if (IsTriggered && !IsRepeatable) return;

            StartCoroutine(PlayTimelineWhenReady());
        }
        
        

        IEnumerator PlayTimelineWhenReady()
        {
            yield return new WaitUntil(() =>
                CinematicManager.Instance.VirtualActor != null &&
                CinematicManager.Instance.ReferenceModelHandler != null);


            PlayableDirector.Play();
            IsTriggered = true;
        }


#if UNITY_EDITOR
        [Button("Load Components")]
        public void LoadComponents()
        {
            PlayableDirector = GetComponent<PlayableDirector>();
        }


#endif
    }
}