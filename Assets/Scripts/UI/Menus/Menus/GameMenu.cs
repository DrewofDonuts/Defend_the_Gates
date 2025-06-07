using UnityEngine;

namespace Etheral
{
    public class GameMenu : MonoBehaviour
    {
        public bool isPause { get; private set; }
        [field: SerializeField] public GameObject PauseMenu { get; private set; }

        GameObject menuInstance;

        static GameMenu _instance;

        public static GameMenu Instance => _instance;

         void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }


        public void OnPause()
        {
            
            Debug.Log("Pause");
            if (!isPause)
            {
                Time.timeScale = 0f;
                // PauseMenu.SetActive(true);
                InstantiateMenu();
                isPause = true;
                AudioListener.pause = true;
                
                Debug.LogError($"Is Pause: {isPause}");
            }
            else
            {
                Time.timeScale = 1f;
                // PauseMenu.SetActive(false);
                Destroy(menuInstance);
                isPause = false;
                AudioListener.pause = false;
                Debug.LogError($"Is Pause from Else: {isPause}");
            }
        }


        public void OnQuitPressed()
        {
            Application.Quit();
        }


        public void InstantiateMenu()
        {
            if (menuInstance == null)
                menuInstance = Instantiate(PauseMenu);
            else
                Destroy(menuInstance);
        }

        //
        //
        // void Update()
        // {
        //     if (isPause)
        //         Time.timeScale = 0f;
        //     else
        //         Time.timeScale = 1f;
        // }
    }
}