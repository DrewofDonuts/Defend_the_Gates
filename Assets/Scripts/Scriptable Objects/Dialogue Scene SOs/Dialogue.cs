using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Etheral/Text Stuff/New Dialogue")]
    [InlineEditor]
    public class Dialogue : ScriptableObject
    {
        [field: InlineButton("CreateEventKey", "New")]
        [field: SerializeField] public EventKey EventKey { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

        [SerializeField] Actor actorOne;
        [SerializeField] Actor actorTwo;


        // [field: SerializeField] public DialogueLine[] DialogueLines { get; private set; }

        [field: SerializeField] public List<DialogueLine> dialogueLines = new();


#if UNITY_EDITOR

        [Button("Create Dialogue Lines", ButtonSizes.Medium), GUIColor(.25f, .50f, .25f)]
        public void CreateDialogue()
        {
            dialogueLines.Add(new DialogueLine());

            if (dialogueLines.Count % 2 == 0)
                dialogueLines[dialogueLines.Count - 1].SetupSpeaker(actorTwo.actorData);
            else
                dialogueLines[dialogueLines.Count - 1].SetupSpeaker(actorOne.actorData);
        }

        void CreateEventKey()
        {
            if (EventKey == null)
                EventKey = AssetCreator.NewEventKey();
        }
#endif
    }

    [Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public AudioClip audioClip;
        public string sentence;

        [PreviewField]
        public Sprite speakerSprite;

        public void SetupSpeaker(ActorData actorData)
        {
            speakerSprite = actorData.sprite;
            speakerName = actorData.characterKey.CharacterName;
        }
    }
}