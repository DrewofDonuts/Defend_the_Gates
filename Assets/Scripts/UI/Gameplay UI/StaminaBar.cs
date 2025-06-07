using UnityEngine;
using UnityEngine.UI;

namespace Etheral.UI
{
    public class StaminaBar : MonoBehaviour
    {
        [Tooltip("Stamina bar Slider")] [SerializeField] Slider _slider;

        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        public void SetMaxStamina(int maxStamina) //sliders max value will be based on the players max stamina
        {
            _slider.maxValue = maxStamina;
            _slider.value = maxStamina;
        }

        public void SetCurrentStamina(float currentStamina)
        {
            _slider.value = currentStamina;
        }
    }
}