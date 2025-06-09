using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral.Defenses
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] Button upgradeButton;
        [SerializeField] TowerObject towerObject;
        
        TowerNode towerNode;
        TowerData towerData;
        
        public void Initialize(TowerData data, TowerNode node)
        {
            towerData = data;
            towerNode = node;
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }

        void OnUpgradeButtonClicked()
        {
            towerNode.Upgrade(towerData);
        }
    }
}