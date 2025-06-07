using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class SelectFirstButton : MonoBehaviour
    {
        
        [field: SerializeField] public Button FirstButton { get; private set; }
        [field: SerializeField] public Button TemplateButton { get; private set; }

        Button[] buttons;

        void OnEnable()
        {
            buttons = GetComponentsInChildren<Button>();
        }

        void Update()
        {
            if (FirstButton == null)
            {
                foreach (var button in buttons)
                {
                    if(button != TemplateButton)
                        FirstButton = button;
                    FirstButton.Select();
                }
            }
        }
    }
}