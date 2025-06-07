using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class MainMenu : Menu
    {
        [field: SerializeField] public Button LoadButton { get; private set; }
        [field: SerializeField] public Button ControlsMapButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }
        [SerializeField] SceneData newGameSceneData;

        // [SerializeField] string newGameSceneName;

        protected override void Start()
        {
            if (FirstButton != null)
            {
                FirstButton.onClick.AddListener(OnPlayPressed);
            }

            if (LoadButton != null)
            {
                LoadButton.onClick.AddListener(OnLoadPressed);
            }

            if (SettingsButton != null)
            {
                SettingsButton.onClick.AddListener(OnSettingsPressed);
            }

            if (ControlsMapButton != null)
            {
                ControlsMapButton.onClick.AddListener(OnControlsMapPressed);
            }
        }

        public void OnPlayPressed()
        {
            GameManager.Instance.NewGame(newGameSceneData.SceneName);
        }

        public void OnLoadPressed()
        {
            //Find searches child objects - so you check from the parent on down. In this case, the "Menus" go and down
            Menu loadGameMenu = transform.parent.Find("LoadGameMenu(Clone)").GetComponent<Menu>();
            if (loadGameMenu != null && MenuManager.Instance != null)
            {
                MenuManager.Instance.OpenMenu(loadGameMenu);
            }
        }


        public void OnSettingsPressed()
        {
            //Find searches child objects - so you check from the parent on down. In this case, the "Menus" go and down
            Menu _settingsMenu = transform.parent.Find("SettingsMenu(Clone)").GetComponent<Menu>();
            if (_settingsMenu != null && MenuManager.Instance != null)
            {
                MenuManager.Instance.OpenMenu(_settingsMenu);
            }
        }

        public void OnControlsMapPressed()
        {
            //Find searches child objects - so you check form the parent on down. In this case, the "Menus" go and down
            Menu creditsScreen = transform.parent.Find("ControlMap(Clone)").GetComponent<Menu>();
            if (creditsScreen != null)
            {
                MenuManager.Instance.OpenMenu(creditsScreen);
            }
        }
    }
}