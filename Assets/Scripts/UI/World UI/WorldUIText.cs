using System;
using TMPro;
using UnityEngine;

namespace Etheral
{
    public class WorldUIText : MonoBehaviour
    {
        [SerializeField] bool disableOnStart;


        void Start()
        {
            if (disableOnStart)
                DisableUI();
        }


        public TextMeshProUGUI tmpText;
        public CanvasGroup canvasGroup;


        public void DisableUI()
        {
            gameObject.SetActive(false);
        }
    }
}