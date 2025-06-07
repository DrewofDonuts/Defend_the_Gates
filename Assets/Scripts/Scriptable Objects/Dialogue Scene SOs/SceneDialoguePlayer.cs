using Etheral;
using UnityEngine;


namespace Etheral
{
    
    // This class is responsible for playing the dialogue audio clips in the scene.
    //It is used by Dialogue System Scene Events
    
    public class SceneDialoguePlayer : MonoBehaviour
    {
        public DialogueAudioObject dialogueAudioObject;
        public AudioSource dialogueSource;

        public void PlayDialogue(int index)
        {
            if (index < dialogueAudioObject.dialogues.Count)
            {
                var dialogue = dialogueAudioObject.dialogues[index];
                dialogueSource.clip = dialogue.audioClip;
                dialogueSource.Play();
            }
        }
    }
}