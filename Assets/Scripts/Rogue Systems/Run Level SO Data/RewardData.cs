using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    [CreateAssetMenu(fileName = "New Reward Data", menuName = "Etheral/Rewards/Reward")]
    public class RewardData : ScriptableObject
    {
      [SerializeField] protected PlayerStatsData playerStatDataData;
        [SerializeField] Sprite rewardIcon;
        [Tooltip("Title of the reward")]
        [SerializeField] string rewardTitle;
        [Tooltip("Details of the reward")]
        [SerializeField] string rewardDetails;
        [Tooltip("Stat that receives the bonus")]
        [SerializeField] string affectedStat;
        [Tooltip("The actual bonus amount")]
        [SerializeField] string rewardStat;

        public string GetRewardTitle() => rewardTitle;
        public string GetRewardDetails() => rewardDetails;
        public string GetAffectedStat() => affectedStat;
        public string GetRewardStat() => rewardStat;
        public Sprite GetRewardIcon() => rewardIcon;


        public PlayerStatsData GetStatReward() => playerStatDataData;
    }
}