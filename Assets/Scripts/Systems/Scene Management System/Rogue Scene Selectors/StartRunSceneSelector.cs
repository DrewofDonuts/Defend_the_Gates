using Sirenix.Utilities;
using UnityEngine;

namespace Etheral
{
    public class StartRunSceneSelector : MonoBehaviour
    {
        // [SerializeField] string sceneToLoad;
        [SerializeField] RunSceneData sceneData;


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                LoadRandomScene();
        }


        [ContextMenu("Load Starting Level ")]
        public void LoadRandomScene()
        {
            if (sceneData.SceneName.IsNullOrWhitespace())
            {
                Debug.LogError("No scenes to load.");
                return;
            }

            EtheralSceneManager.Instance.ChangeScene(sceneData.SceneName, .15f);
        }
    }
}