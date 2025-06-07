using System;
using UnityEngine;
using UnityEngine.UI;


namespace Etheral
{
    [RequireComponent(typeof(Canvas))]
    public class Menu : MonoBehaviour
    {
        [field: SerializeField] public Button FirstButton { get; private set; }


        protected virtual void Start()
        {
            if (FirstButton != null)
                FirstButton.Select();
        }


        public void OnBackPressed()
        {
            if (MenuManager.Instance != null)
                MenuManager.Instance.CloseMenu();
        }

        public void OnQuitPressed()
        {
            Application.Quit();
        }
    }
}