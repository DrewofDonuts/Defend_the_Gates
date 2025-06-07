using TMPro;
using UnityEngine;

namespace Etheral
{
    public class PlayerQuestHandler : MonoBehaviour
    {
        [Header("Audio References")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip questAddedClip;
        [SerializeField] AudioClip questCompletedClip;
        
        [Header("TMP References")]
        [SerializeField] TextMeshProUGUI questTitle;
        [SerializeField] TextMeshProUGUI questText;

        void Start()
        {
            EventBusGameController.OnQuestAdded += UpdateQuestUI;
            EventBusGameController.OnQuestCompleted += CompleteQuest;
        }

        void OnDisable()
        {
            EventBusGameController.OnQuestAdded -= UpdateQuestUI;
            EventBusGameController.OnQuestCompleted -= CompleteQuest;
        }

        void UpdateQuestUI(QuestObject obj)
        {
            Debug.Log($"Quest Added: {obj.QuestTitle}");
            questTitle.text = obj.QuestTitle;
            questText.text = obj.QuestObjective;
        }

        void CompleteQuest(QuestObject obj)
        {
            Debug.Log($"Quest Completed: {obj.QuestTitle}");
            questTitle.text = "Quest Completed";
            questText.text = "You have completed the quest.";
        }
    }
}