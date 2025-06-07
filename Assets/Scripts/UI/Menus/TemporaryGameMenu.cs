using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Etheral
{
    public class TemporaryGameMenu : MonoBehaviour
    {
        [SerializeField] public Button firstButton;
        [SerializeField] public Button restartButton;
        [SerializeField] public Button quitButton;
        [SerializeField] public Button saveButton;
        [SerializeField] public Button loadButton;


        // Start is called before the first frame update
        void Start()
        {
            firstButton.Select();

            restartButton.onClick.AddListener(OnRestart);
            quitButton.onClick.AddListener(OnQuit);
            saveButton.onClick.AddListener(OnSave);
            loadButton.onClick.AddListener(OnLoad);

            // EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }

        void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnRestart);
            quitButton.onClick.RemoveListener(OnQuit);
            saveButton.onClick.RemoveListener(OnSave);
            loadButton.onClick.RemoveListener(OnLoad);
        }

        void OnLoad()
        {
            GameMenu.Instance.OnPause();
            GameManager.Instance.LoadGame(GameManager.Instance.GameData.GameName);
        }

        void OnSave() => GameManager.Instance.SaveGame();
        void OnQuit() => GameMenu.Instance.OnQuitPressed();

        void OnRestart()
        {
            GameMenu.Instance.OnPause();
            
            
            EtheralSceneManager.Instance.RestartScene();
        }
    }
}