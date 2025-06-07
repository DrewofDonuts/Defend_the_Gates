using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class DialogueTrigger : BaseEventTrigger
    {
        [field: FoldoutGroup("Dialogue")]
        [field: SerializeField] public Dialogue Dialogue { get; private set; }


        public void TriggerDialogue()
        {
            if (IsTriggered) return;
            DialogueSceneManager _dialogueSceneManager = FindObjectOfType<DialogueSceneManager>();
            
            
            // _dialogueSceneManager.StartDialogue(Dialogue);
            
            _dialogueSceneManager.StartDialogueByKey(Dialogue.EventKey);
            
            IsTriggered = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                TriggerDialogue();
        }

#if UNITY_EDITOR

        [ShowIf("@Dialogue == null")]
        [Button("Create Dialogue", ButtonSizes.Medium), GUIColor(.25f, .50f, .25f)]
        public void CreateDialogue()
        {
            var asset = ScriptableObject.CreateInstance<Dialogue>();
            UnityEditor.AssetDatabase.CreateAsset(asset,
                "Assets/Etheral/Scriptable Object Assets/Dialogue/New Dialogue.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            Dialogue = asset;
        }

#endif
    }
}