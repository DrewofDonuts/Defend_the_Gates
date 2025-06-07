using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Quest Data", menuName = "Etheral/Quest/Quest Data")]
    [InlineEditor]
    public class QuestObject : MessageObject
    {
        [SerializeField] string questTitle;
        [SerializeField] string questID;
        [TextArea]
        [SerializeField] string questObjective;

        [Header("Conditions")]
        [SerializeField] string keyNeededToStartQuest;

        string keyWhenQuestIsActive => questID + ".active";
        string keyWhenQuestIsCompleted => questID + ".completed";


        public string QuestTitle => questTitle;
        public string QuestID => questID;
        public string QuestObjective => questObjective;
        public override string KeyToReceive => keyNeededToStartQuest;
        public string KeyWhenActive => keyWhenQuestIsActive;
        public override string KeyToSend => keyWhenQuestIsCompleted;


#if UNITY_EDITOR

        void OnValidate()
        {
            SetIDBasedOnName();
        }

        void SetIDBasedOnName()
        {
            if (string.IsNullOrWhiteSpace(keyNeededToStartQuest))
            {
                keyNeededToStartQuest = questID + ".start";
            }

            string assetPath = AssetDatabase.GetAssetPath(this);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            questID = fileName;
        }
#endif
    }
}