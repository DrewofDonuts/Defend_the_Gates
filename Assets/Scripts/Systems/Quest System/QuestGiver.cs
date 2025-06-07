using System;
using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class QuestGiver : MonoBehaviour, IGetTriggered
    {
        [InlineButton("CreateNewQuest", "New Quest")]
        [SerializeField] QuestObject questObject;

        void Start()
        {
            Trigger = GetComponent<ITrigger>();
            
            if (Trigger != null)
                Trigger.OnTrigger += AddQuest;
            
            // MessageSystemEtheral.OnCondition += AddQuest;
        }


        void OnDestroy()
        {
            if (Trigger != null)
                Trigger.OnTrigger -= AddQuest;
            
            // MessageSystemEtheral.OnCondition -= AddQuest;
        }

        public ITrigger Trigger { get; set; }


        void AddQuest(string obj)
        {
            QuestManager.Instance.AddQuest(questObject);
        }


        public void AddQuest()
        {
            Debug.Log($"Adding quest: {questObject.QuestTitle}");
            QuestManager.Instance.AddQuest(questObject);
        }


#if UNITY_EDITOR

        void CreateNewQuest()
        {
            questObject = AssetCreator.CreateNewQuestObject();
        }


#endif
    }
}