using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class QuestManager : MonoBehaviour, IBind<QuestSaveData>, INamed
    {
        static QuestManager instance;
        public Dictionary<string, bool> questDictionary;

        public static QuestManager Instance => instance;

        bool dictionaryUpdated;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            EventBusGameController.OnQuestAdded += UpdateQuestUI;
        }

        void OnDestroy()
        {
            EventBusGameController.OnQuestAdded -= UpdateQuestUI;
        }

        void UpdateQuestUI(QuestObject obj)
        {
            Debug.Log($"Quest Added: {obj.QuestTitle}");
        }

        public void AddQuest(QuestObject questObject)
        {
            if (!questDictionary.ContainsKey(questObject.QuestID))
            {
                questDictionary.Add(questObject.QuestID, false);
                EventBusGameController.AddQuest(this, questObject);
                EtheralMessageSystem.SendKey(this, questObject.KeyWhenActive);
                Debug.Log($"Quest {questObject.QuestTitle} added.");
            }
            else
            {
                Debug.LogWarning($"Quest {questObject.QuestTitle} already exists in the dictionary.");
            }
        }

        public void CompleteQuest(QuestObject questObject)
        {
            if (questDictionary.ContainsKey(questObject.QuestID))
            {
                questDictionary[questObject.QuestID] = true;
                Debug.Log($"Quest {questObject.QuestTitle} completed.");

                EtheralMessageSystem.SendKey(this, questObject.KeyToSend);
                EventBusGameController.CompleteQuest(this, questObject);
            }
            else
            {
                Debug.LogWarning($"Quest {questObject.QuestTitle} not found in the dictionary.");
            }
        }

        public void Bind(QuestSaveData _data)
        {
            if (questDictionary == null)
                questDictionary = new Dictionary<string, bool>();

            if (_data == null)
                _data = new QuestSaveData();

            questDictionary = _data.questDictionary;
            dictionaryUpdated = true;
        }

        public string Name { get; set; }
    }
}