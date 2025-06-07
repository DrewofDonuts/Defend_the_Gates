using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Etheral
{
    //This class manages the dialogue scene
    
    public class DialogueSceneManager : MonoBehaviour
    {
        static DialogueSceneManager _instance;
        public static DialogueSceneManager Instance => _instance;

        [field: SerializeField] public TextMeshProUGUI DialogueText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI SpeakerNameText { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        [SerializeField] Image image;
        [field: SerializeField] public float TimeBetweenSentences { get; private set; } = 11f;

        public List<Speaker> speakers = new();

        [FormerlySerializedAs("dialogueHandler")]
        public DialogueSceneHandler _dialogueSceneHandler;


        Queue<DialogueLine> dialogueLines;
        Queue<string> sentences;
        Queue<AudioClip> audioClips;

        string speaker;
        Dialogue currentDialogue;


        float timeBeforeNextSentence;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        void Start()
        {
            dialogueLines = new Queue<DialogueLine>();

            //Old
            sentences = new Queue<string>();
            audioClips = new Queue<AudioClip>();
            
            DialogueText.text = "";
            SpeakerNameText.text = "";
            image.enabled = false;
        }

        public void Register<T>(T register)
        {
            if (typeof(T) == typeof(DialogueSceneHandler))
            {
                _dialogueSceneHandler = register as DialogueSceneHandler;
            }

            if (typeof(T) == typeof(Speaker))
            {
                speakers.Add(register as Speaker);
            }
        }

        public void UnRegister<T>(T unregister)
        {
            if (typeof(T) == typeof(DialogueSceneHandler))
            {
                _dialogueSceneHandler = null;
            }

            if (typeof(T) == typeof(Speaker))
            {
                speakers.Remove(unregister as Speaker);
            }
        }


        public void InitializeDialogueAsset(EventKey eventKey)
        {
            //clear dialogue to ensure no repeats
            currentDialogue = null;

            //check DialogueHandler for dialogue against Key
            currentDialogue = _dialogueSceneHandler.GetDialogue(eventKey);

            //If Dialogue is found, start dialogue
            if (currentDialogue != null)
                StartDialogueWithoutKey(currentDialogue);
        }

        public void StartDialogueWithoutKey(Dialogue dialogue)
        {
            sentences.Clear();
            audioClips.Clear();

            currentDialogue = dialogue;

            dialogueLines.Clear();

            foreach (var dialogueLine in currentDialogue.dialogueLines)
            {
                dialogueLines.Enqueue(dialogueLine);
            }

            DisplayNextSentence();
        }


        public void StartDialogueByKey(EventKey dialogueEventKey)
        {
            if (!ConditionManager.Instance.CheckCondition(dialogueEventKey)) return;

            sentences.Clear();
            audioClips.Clear();

            currentDialogue = _dialogueSceneHandler.GetDialogue(dialogueEventKey);


            dialogueLines.Clear();

            foreach (var dialogueLine in currentDialogue.dialogueLines)
            {
                dialogueLines.Enqueue(dialogueLine);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (dialogueLines.Count == 0)
            {
                EndDialogue();
                return;
            }

            var line = dialogueLines.Dequeue();

            timeBeforeNextSentence = Mathf.Max(line.sentence.Length / TimeBetweenSentences, 1f);
            
            // timeBeforeNextSentence = Mathf.Max(line.sentence.Length * .50f, 1f);

            //Display the text
            SpeakerNameText.text = line.speakerName;
            DialogueText.text = line.sentence;

            //Display speaker image
            HandleSprite(line);


            //Play the audio
            if (line.audioClip != null)
            {
                if (AudioSource == null)
                    AudioSource = CharacterManager.Instance.Player.GetComponentInChildren<CharacterAudio>().EmoteSource;


                AudioSource.clip = line.audioClip;
                AudioSource.Play();
            }


            StartCoroutine(PlayNextSentence());
        }

        void HandleSprite(DialogueLine line)
        {
            if (image.enabled == false)
                image.enabled = true;

            image.sprite = line.speakerSprite;
        }


        IEnumerator PlayNextSentence()
        {
            var timeTowait = AudioSource.clip != null ? AudioSource.clip.length : 0;

            yield return new WaitForSeconds(timeTowait + timeBeforeNextSentence);
            DisplayNextSentence();
        }

        void EndDialogue()
        {
            SpeakerNameText.text = "";
            DialogueText.text = "";
            image.sprite = null;
            image.enabled = false;

            Debug.Log("End of conversation");
        }

        public void SetAudioSource(AudioSource audioSource)
        {
            AudioSource = audioSource;
        }
    }
}