using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class RewardItemUI : MonoBehaviour, IAmPoolObject<RewardItemUI>
    {
        [field: Header("Object Pooling")]
        [field: SerializeField] public RewardItemUI Prefab { get; set; }
        [field: SerializeField] public string PoolKey { get; set; }

        [Header("References")]
        [SerializeField] Button applyRewardButton;
        [SerializeField] TextMeshProUGUI rewardTitle;
        [SerializeField] TextMeshProUGUI rewardDetails;
        [SerializeField] TextMeshProUGUI affectedStat;
        [SerializeField] TextMeshProUGUI rewardStat;
        [SerializeField] Image rewardIcon;

        RewardData rewardData;

        public event Action<RewardData> OnRewardApplied;

        void Start()
        {
            applyRewardButton.onClick.AddListener(SendReward);
        }

        public void SelectButton()
        {
            applyRewardButton.Select();
            
        }

        public void Init(RewardData _rewardData)
        {
            rewardData = _rewardData;
            InitRewardData();
        }

        void InitRewardData()
        {
            rewardTitle.text = rewardData.GetRewardTitle();
            rewardStat.text = rewardData.GetRewardStat();
            affectedStat.text = rewardData.GetAffectedStat();
            rewardDetails.text = rewardData.GetRewardDetails();
            rewardIcon.sprite = rewardData.GetRewardIcon();
        }

        void SendReward()
        {
            OnRewardApplied?.Invoke(rewardData);
        }

        public void DisableButton()
        {
            applyRewardButton.interactable = false;
        }

        public RewardData GetRewardData()
        {
            return rewardData;
        }

        // void OnDisable()
        // {
        //     Release();
        // }

        public void Release()
        {
            ObjectPoolManager.Instance.ReleaseObject(this);
        }
    }
}