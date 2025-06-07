using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class LoadGameButton : MonoBehaviour
    {
        [SerializeField] Button LoadButton;
        [SerializeField] Button DeleteButton;
        [SerializeField] Button ConfirmationButton;
        [SerializeField] Button CancelButton;

        string gameName;

        void Start()
        {
            LoadButton.onClick.AddListener(LoadGame);

            CancelButton.gameObject.SetActive(false);
            ConfirmationButton.gameObject.SetActive(false);

            ConfirmationButton.onClick.AddListener(DeleteGame);
            DeleteButton.onClick.AddListener(SelectsDelete);
            CancelButton.onClick.AddListener(SelectsCancel);
        }

        void SelectsDelete()
        {
            ConfirmationButton.gameObject.SetActive(true);
            CancelButton.gameObject.SetActive(true);
            CancelButton.Select();
            DeleteButton.gameObject.SetActive(false);
        }

        void SelectsCancel()
        {
            ConfirmationButton.gameObject.SetActive(false);
            CancelButton.gameObject.SetActive(false);
            LoadButton.Select();
            DeleteButton.gameObject.SetActive(true);
        }

        void LoadGame()
        {
            GameManager.Instance.LoadGame(gameName);
        }

        void DeleteGame()
        {
            GameManager.Instance.DeleteGame(gameName);

            Destroy(gameObject);
        }

        public void SetGameName(string _gameName)
        {
            Debug.Log($"Setting game name to {_gameName}");
            gameName = _gameName;
            GetComponentInChildren<TMP_Text>().SetText(gameName);
        }
    }
}