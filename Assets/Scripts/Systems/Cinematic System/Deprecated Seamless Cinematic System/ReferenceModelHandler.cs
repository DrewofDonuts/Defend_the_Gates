using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class ReferenceModelHandler : MonoBehaviour
    {
        [field: SerializeField] public GameObject GameModel { get; private set; }
        [field: SerializeField] public VirtualActor VirtualActor { get; private set; }


        IEnumerator Start()
        {
            yield return new WaitUntil(() => CinematicManager.Instance != null);
            CinematicManager.Instance.Register(this);
            CinematicManager.Instance.IsPlayingCinematic += ToggleGameModel;
        }

        public void ToggleGameModel(bool showModel)
        {
            Debug.Log("Toggling game model");
            GameModel.SetActive(showModel);
        }

        public GameObject GetModel()
        {
            return GameModel;
        }
    }
}