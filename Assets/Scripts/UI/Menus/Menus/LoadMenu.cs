using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class LoadMenu : Menu
    {
        protected override void Start()
        {
            if (FirstButton != null)
            {
                FirstButton.onClick.AddListener(OnBackPressed);
            }
        }

    }
}
