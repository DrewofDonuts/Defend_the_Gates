using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;

namespace Etheral
{
    public class TestCopyModelAndPlayTimeline : MonoBehaviour
    {
        [field: SerializeField] public GameObject Model { get; private set; }
        [field: SerializeField] public GameObject Duplicate { get; private set; }
        [field: SerializeField] public PlayableDirector PlayableDirector { get; private set; }


        [Button("Copy Model")]
        public void Copy()
        {
            Instantiate(Model, Duplicate.transform.position, Duplicate.transform.rotation, Duplicate.transform);
        }


        [Button("Play Timeline")]
        public void PlayTimeline()
        {
            PlayableDirector.Play();
        }
    }
}