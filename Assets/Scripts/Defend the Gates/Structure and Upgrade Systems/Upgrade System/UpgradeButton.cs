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
        
        INode node;
        IUpgradable upgradeData;
        
        public void Initialize(IUpgradable data, INode _node)
        {
            upgradeData = data;
            node = _node;
            nameText.text = upgradeData.Name;
            descriptionText.text = upgradeData.Description;
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        void OnUpgradeButtonClicked()
        {
            node.Upgrade(upgradeData);
        }
    }
}