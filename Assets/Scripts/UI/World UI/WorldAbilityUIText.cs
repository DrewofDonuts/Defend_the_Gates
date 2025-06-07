using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class WorldAbilityUIText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleTextField;
        [SerializeField] TextMeshProUGUI descriptionTextField;
        [SerializeField] Image abilityIcon;


        public void SetAbilityData(AbilityUnlockData abilityData)
        {
            titleTextField.text = abilityData.AbilityTitle;
            descriptionTextField.text = abilityData.AbilityDescription;
            abilityIcon.sprite = abilityData.AbilitySprite;
        }
    }
}