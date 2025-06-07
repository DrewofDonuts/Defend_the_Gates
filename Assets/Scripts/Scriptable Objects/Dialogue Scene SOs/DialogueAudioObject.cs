using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


namespace Etheral
{
    [CreateAssetMenu(fileName = "New Dialogue Audio", menuName = "Etheral/Audio/Dialogue Audio")]
    [InlineEditor]
    public class DialogueAudioObject : ScriptableObject
    {
        [PreviewField(50, ObjectFieldAlignment.Left)]
        [HideLabel] public Sprite icon;

        public AudioMixerGroup audioMixerGroup;

        public string name;
        public int id = -1;
        public AudioClip audioClip;

        public List<DialogueAudioData> dialogues;

        void OnValidate()
        {
            SetID();
        }


        public void SetID()
        {
            HashSet<int> id = new HashSet<int>();

            foreach (var dialogue in dialogues)
            {
                if (id.Contains(dialogue.id))
                {
#if UNITY_EDITOR
                    EditorUtility.DisplayDialog("Warning", $"IDs must be unique. {dialogue.name} has duplicate ID",
                        "OK");
#endif
                }
            }
        }


#if UNITY_EDITOR
        [Button("Create New Dialogue Audio")]
        public void CreateNewDialogueData()
        {
            if (dialogues.Any(x => x.id == id))
            {
                EditorUtility.DisplayDialog("Warning", "ID " + id + " is already in use", "OK");
                return;
            }

            if (id == -1)
            {
                EditorUtility.DisplayDialog("Warning", "ID cannot be -1", "OK");
                return;
            }

            DialogueAudioData audioData = new DialogueAudioData();
            audioData.name = name;
            audioData.id = id;
            audioData.audioClip = audioClip;
            dialogues.Add(audioData);

            ClearNewDialogueData();
        }

        void ClearNewDialogueData()
        {
            name = "";
            id = -1;
            audioClip = null;
        }
#endif
    }

    [Serializable]
    public class DialogueAudioData
    {
        public string name;

        [ReadOnly]
        public int id = -1;

        public AudioClip audioClip;
    }
}


//
// public void PlayAudioClip(int index)
// {
//     AudioSource audioSource = new AudioSource();
//     audioSource.reverbZoneMix = 1;
//     audioSource.outputAudioMixerGroup = audioMixerGroup;
//
//     audioSource.clip = dialogues[index].audioClips;
//     audioSource.Play();
//
//     // AudioSource.PlayClipAtPoint(dialogueData[index].audioClips, Vector3.zero);
// }