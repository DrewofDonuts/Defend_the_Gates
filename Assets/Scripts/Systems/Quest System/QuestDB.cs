using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Quest DB", menuName = "Etheral/Quest/Quest DB")]
    public class QuestDB : ScriptableObject
    {
        [SerializeField] LevelQuest[] levelQuests;


        void OnValidate()
        {
            CheckForDuplicateQuestIDs();
        }

        void CheckForDuplicateQuestIDs()
        {
            foreach (var levelQuest in levelQuests)
            {
                HashSet<string> questIDs = new HashSet<string>();
                foreach (var quest in levelQuest.quests)
                {
                    if (!questIDs.Add(quest.QuestID))
                    {
                        Debug.LogError($"Duplicate quest ID found: {quest.QuestID} in level {levelQuest.levelName}");
                    }
                }
            }
        }
    }

    [Serializable]
    public class LevelQuest
    {
        public string levelName;
        public QuestObject[] quests;
    }
}