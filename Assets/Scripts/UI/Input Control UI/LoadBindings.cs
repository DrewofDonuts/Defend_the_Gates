using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class LoadBindings : MonoBehaviour
    {
        [SerializeField] List<InputIcons> inputIcons;
        [SerializeField] List<StringObject> actionNames;
        [SerializeField] InputBindingButton inputBindingButton;
        [SerializeField] Transform parent;


        void Start()
        {
            for (int i = 0; i < actionNames.Count; i++)
            {
                var button = Instantiate(inputBindingButton, parent);
                button.Initialize(actionNames[i].Value, inputIcons[i]);
            }
        }
    }
}