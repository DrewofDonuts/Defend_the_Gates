using UnityEngine;

namespace Etheral
{
    public class CinematicController : MonoBehaviour
    {
        [SerializeField] InputObject inputObject;
        [SerializeField] SceneData nextScene;

        void Start()
        {
            inputObject.SouthButtonEvent += SkipCinematic;
        }

        void SkipCinematic()
        { 
            EtheralSceneManager.Instance.ChangeScene(nextScene.SceneName, .25f);
        }
    }
}