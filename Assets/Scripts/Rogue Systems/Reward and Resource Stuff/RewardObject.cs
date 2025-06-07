using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Etheral
{
    public class RewardObject : MonoBehaviour
    {
        [SerializeField] Canvas rewardCanvas;
        [SerializeField] Transform rewardParent;
        [SerializeField] RewardItemUI rewardButtonPrefab;
        [SerializeField] RewardData[] overrideRewardsData;

        // [SerializeField] List<RewardData> rewardsData = new();

        // RunStatData playerRunStatData;
        PlayerStatsController playerStatsController;

        readonly int rewardCount = 3;
        readonly List<RewardItemUI> spawnedRewardPrefabs = new();

        bool hasSelectedFirstButton;
        HashSet<RewardData> selectedRewards = new();

        RunSceneData runSceneData;


        void Start()
        {
            rewardCanvas.gameObject.SetActive(false);

            if (overrideRewardsData.Length == 0)
                runSceneData = EtheralSceneManager.Instance.GetSceneData(SceneManager.GetActiveScene().name);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerStateMachine player))
            {
                playerStatsController = player.PlayerComponents.GetStatsController();
                rewardCanvas.gameObject.SetActive(true);
                InitializeRewardOptions();
                EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.UIState);
            }
        }

        void InitializeRewardOptions()
        {
            //Add rewards to the UI
            //Get player's RunStatData to pass to the ApplyReward method


            while (selectedRewards.Count < rewardCount && selectedRewards.Count < runSceneData.RewardsData.Length)
            {
                var reward = runSceneData.RewardsData[UnityEngine.Random.Range(0, runSceneData.RewardsData.Length)];
                selectedRewards.Add(reward);
            }

            foreach (var reward in selectedRewards)
            {
                // var rewardItem = ObjectPoolManager.Instance.GetObject(rewardButtonPrefab, rewardParent, 3);

                var rewardItem = Instantiate(rewardButtonPrefab, rewardParent);

                // rewardItem.transform.parent = rewardParent;
                rewardItem.Init(reward);
                rewardItem.OnRewardApplied += OnCompleteRewardSelection;
                spawnedRewardPrefabs.Add(rewardItem);

                CheckIfGamepadtoSelectButton(rewardItem);
            }
        }

        void CheckIfGamepadtoSelectButton(RewardItemUI rewardItem)
        {
            if (!hasSelectedFirstButton)
            {
                // rewardItem.SelectButton();
                // hasSelectedFirstButton = true;
                var inputDevice = InputManager.Instance.GetInputDevice();

                if (inputDevice is Gamepad)
                {
                    EventSystem.current.SetSelectedGameObject(rewardItem.gameObject);
                    hasSelectedFirstButton = true;
                }
            }
        }

        public void OnCompleteRewardSelection(RewardData rewardData)
        {
            StartCoroutine(WaitBeforeDisablingRewardUI(rewardData));
        }

        IEnumerator WaitBeforeDisablingRewardUI(RewardData rewardData)
        {
            ApplyReward(rewardData);
            DisableOtherButtons(rewardData);
            yield return new WaitForSeconds(.5f);
            rewardCanvas.gameObject.SetActive(false);
            EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.MovementState);

            gameObject.SetActive(false);
        }

        void ApplyReward(RewardData rewardData)
        {
            var newRewardData = rewardData.GetStatReward();

            playerStatsController.ReceiveRewardData(newRewardData);
        }

        void DisableOtherButtons(RewardData selectedReward)
        {
            foreach (var rewardUI in spawnedRewardPrefabs)
            {
                if (rewardUI.GetRewardData() != selectedReward)
                {
                    rewardUI.DisableButton();
                }
            }
        }

        void OnDisable()
        {
            foreach (var rewardUI in spawnedRewardPrefabs)
            {
                rewardUI.OnRewardApplied -= OnCompleteRewardSelection;
                Destroy(rewardUI);
            }
        }
    }
}