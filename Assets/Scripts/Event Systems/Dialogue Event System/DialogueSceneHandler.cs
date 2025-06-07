using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Etheral
{
    //This class holds the dialogues for the scene
    
    public class DialogueSceneHandler : MonoBehaviour
    {
        [field: SerializeField] public List<Dialogue> Dialogues { get; private set; } = new();
        public UnityEvent testDialogueEvent;

        public Dictionary<EventKey, bool> eventKeys = new();

        IEnumerator Start()
        {
            DialogueEventKeyToDictionary();
            yield return new WaitUntil(() => DialogueSceneManager.Instance);

            DialogueSceneManager.Instance.Register(this);
        }

        [Button("Invoke Dialogue Event", ButtonSizes.Medium), GUIColor(.25f, .50f, .25f)]

        public void InvokeDialogueEvent()
        {
            testDialogueEvent.Invoke();
        }

        //used by Dialogue System and other Events 
        public void PlayDialogue(EventKey eventKey)
        {
            Dialogue dialogue = GetDialogue(eventKey);
            StartCoroutine(TimeBeforeStarting(dialogue));
        }
        
        public void PlayDialogue(Dialogue dialogue)
        {
            StartCoroutine(TimeBeforeStarting(dialogue));
        }

        IEnumerator TimeBeforeStarting(Dialogue dialogue)
        {
            float timeBeforeStarting = 1.5f;
            yield return new WaitForSeconds(timeBeforeStarting);
            DialogueSceneManager.Instance.StartDialogueWithoutKey(dialogue);
        }

        void DialogueEventKeyToDictionary()
        {
            if (Dialogues == null) return;

            foreach (var dialogue in Dialogues)
            {
                if (dialogue.EventKey == null) continue;
                eventKeys.TryAdd(dialogue.EventKey, false);
            }
        }

        void OnDestroy()
        {
            DialogueSceneManager.Instance.UnRegister(this);
        }

        public Dialogue GetDialogue(EventKey eventKey)
        {
            foreach (var dialogue in Dialogues)
            {
                if (dialogue.EventKey == eventKey && eventKeys[eventKey] == false)
                {
                    return dialogue;
                }
            }

            return default;
        }


#if UNITY_EDITOR

        // [ShowIf("@Dialogue == null")]
        [Button("Create Dialogue", ButtonSizes.Medium), GUIColor(.25f, .50f, .25f)]
        public void CreateDialogue()
        {
            var asset = ScriptableObject.CreateInstance<Dialogue>();
            UnityEditor.AssetDatabase.CreateAsset(asset,
                "Assets/Etheral/Scriptable Object Assets/Dialogue/New Dialogue.asset");
            UnityEditor.AssetDatabase.SaveAssets();

            Dialogues.Add(asset);
        }

#endif
    }
}