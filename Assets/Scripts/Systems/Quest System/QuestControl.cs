using System;
using PixelCrushers.QuestMachine.Wrappers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class QuestControl : MonoBehaviour
    {
        [FormerlySerializedAs("questData")]
        [Header("Quest Control")]
        [SerializeField] QuestObject questObject;
        
        public void CompleteQuest()
        {
            QuestManager.Instance.CompleteQuest(questObject);
        }
        
    }
}


//Quest is Ready to be Added - needs a key to be activated
//Quest Is added 
//Certain objects and behaviours are activated when that quest is activated - needs a beginning key
//Quest is completed
//Certain objects and behaviours are activated  when that quest is completed - needs a completion key