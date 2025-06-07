using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class RunProgressUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI atonementText;
        [SerializeField] TextMeshProUGUI maxAtonementText;
        [FormerlySerializedAs("runProgressController")] [SerializeField] PlayerStatsController playerStatsController;

        int currentAtonement;
        
        void Start()
        {
            EventBusPlayerController.OnGetXP += ReceiveXP;
            currentAtonement = playerStatsController.GetAtonement();
            atonementText.text = currentAtonement.ToString();
            maxAtonementText.text = playerStatsController.GetMaxAtonement().ToString();
        }

        void ReceiveXP(int xp)
        {
            playerStatsController.UpdateXPInRunBinder(xp);
            currentAtonement = playerStatsController.GetAtonement();
            atonementText.text = currentAtonement.ToString();
        }

        void OnDisable()
        {
            EventBusPlayerController.OnGetXP -= ReceiveXP;
        }
    }
}