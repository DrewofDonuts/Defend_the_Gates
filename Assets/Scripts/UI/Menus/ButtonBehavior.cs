using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Etheral
{
    public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, ISelectHandler
    {
        [SerializeField] AudioClip hoverSound;
        [SerializeField] AudioSource audioSource;

        // This method is called when the pointer enters the button area.
        public void OnPointerEnter(PointerEventData eventData)
        {
            audioSource.PlayOneShot(hoverSound);
        }


        //for gamepad
        public void OnSelect(BaseEventData eventData)
        {
            audioSource.PlayOneShot(hoverSound);

            // You can add additional logic here if needed when the button is selected.
            // For example, you might want to change the button's appearance or trigger an animation.
        }
        

#if UNITY_EDITOR
        [Button("GetAudioSource")]
        public void GetAudioSource()
        {
            audioSource = GetComponent<AudioSource>();
        }


#endif
    }
}