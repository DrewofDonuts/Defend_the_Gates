using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [Serializable]
    public class LevelDialogue
    {
        public string levelName;
        public List<NarratorDialogue> narratorDialogueObjects;
        
        public void AddDialogue(NarratorDialogue dialogue)
        {
            if (narratorDialogueObjects == null)
                narratorDialogueObjects = new List<NarratorDialogue>();
            
            narratorDialogueObjects.Add(dialogue);
        }
    }

    [Serializable]
    public class NarratorDialogueInfo : IMessage
    {
        [Header("Dialogue")]
        public string Title;
        public List<NarratorDialogueLine> dialogueLines = new();
        
        
        [FoldoutGroup("Keys", expanded: false)]
        [SerializeField] string keyToSend;
        [FoldoutGroup("Keys", expanded: false)]
        [SerializeField] string keyToReceive;

        
        public string KeyToSend => keyToSend;
        public string KeyToReceive => keyToReceive;
    }

    [Serializable]
    public class NarratorDialogueLine
    {
        [TextArea(3, 3)]
        public string sentence;
        public AudioClip audioClip;
    }
    
}