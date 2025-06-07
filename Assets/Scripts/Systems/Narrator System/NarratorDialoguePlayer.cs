using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    //When complete, the NarratorDialogue will send a key
    //This will allow for other events and quests to be triggered

    public class NarratorDialoguePlayer : MonoBehaviour, IGetTriggered
    {
        [Header("Dialogue Info")]
        [InlineButton("CreateNewDialogue", "New")]
        [SerializeField] NarratorDialogue narratorDialogue;

        [Header("References")]
        [SerializeField] Canvas narratorCanvas;
        [SerializeField] TextMeshProUGUI narratorText;
        [SerializeField] AudioSource audioSource;

        [FormerlySerializedAs("timeBasedOnTextLength")]
        [Header("Settings")]
        [SerializeField] float timeBetweenSentences = 0.1f;

        public ITrigger Trigger { get; set; }

        [Header("Debug")]
        public string title;


        Queue<NarratorDialogueLine> dialogueLines;
        Queue<AudioClip> audioClips;

        [ReadOnly]
        public float timeBeforeNextSentence;


        void Start()
        {
            narratorCanvas.enabled = false;
            dialogueLines = new Queue<NarratorDialogueLine>();
            audioClips = new Queue<AudioClip>();
            Trigger = GetComponent<ITrigger>();

            if (Trigger != null)
            {
                Debug.Log("Trigger found");
                Trigger.OnTrigger += PlayNarration;
            }
        }

        void OnDestroy()
        {
            if (Trigger != null)
            {
                Trigger.OnTrigger -= PlayNarration;
            }
        }

        void OnValidate()
        {
            if (narratorDialogue != null && title != narratorDialogue.name)
            {
                title = narratorDialogue.Dialogue.Title;
                name = "Narrator - " + title;
            }
        }

        public void PlayNarration()
        {
            Debug.Log("PlayNarration called");
            var dialogue = narratorDialogue.Dialogue;

            audioClips.Clear();
            dialogueLines.Clear();

            foreach (var dialogueDialogueLine in dialogue.dialogueLines)
            {
                dialogueLines.Enqueue(dialogueDialogueLine);
            }

            DisplayNextSentence();
        }

        void DisplayNextSentence()
        {
            if (dialogueLines.Count == 0)
            {
                StartCoroutine(EndDialogue());
                return;
            }

            var line = dialogueLines.Dequeue();

            // timeBeforeNextSentence = Mathf.Max(line.sentence.Length / timeBetweenSentences, 1f);
            timeBeforeNextSentence = Mathf.Max(line.sentence.Length * timeBetweenSentences, 1f);

            //Display the text
            narratorCanvas.enabled = true;
            narratorText.text = line.sentence;

            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();
            }

            StartCoroutine(PlayNextSentence());
        }

        IEnumerator PlayNextSentence()
        {
            var timeTowait = audioSource.clip != null ? audioSource.clip.length : 0;

            yield return new WaitForSeconds(timeTowait + timeBeforeNextSentence);
            DisplayNextSentence();
        }


        IEnumerator EndDialogue()
        {
            yield return new WaitForSeconds(timeBeforeNextSentence);
            narratorCanvas.enabled = false;

            if (!string.IsNullOrWhiteSpace(narratorDialogue.Dialogue.KeyToSend))
            {
                yield return new WaitForSeconds(1f);
                EtheralMessageSystem.SendKey(this, narratorDialogue.Dialogue.KeyToSend);
            }
        }

#if UNITY_EDITOR

        void CreateNewDialogue()
        {
            narratorDialogue = AssetCreator.NewNarratorDialogue();
        }

#endif
    }
}