using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral.DefendTheGates
{
    public class UpgradeButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Button upgradeButton;
        
        [Header("Text References")]
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI descriptionText;
        
        TowerNode towerNode;
        TowerData towerData;
        
        public void Initialize(TowerData data, TowerNode node)
        {
            towerData = data;
            towerNode = node;
            nameText.text = towerData.TowerName;
            descriptionText.text = towerData.Description;
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        void OnUpgradeButtonClicked()
        {
            towerNode.Upgrade(towerData);
        }
    }
}