using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class SimpleNPC : MonoBehaviour
    {
        [SerializeField] AnimationHandler animationHandler;
        [SerializeField] DialogueAudioObject dialogueAudioObject;
        [SerializeField] AudioSource dialogueSource;
        [SerializeField] CharacterID characterID;

        void Start()
        {
            EventBusPlayerController.SubscribeToCharacterStateChange(HandleCharacterStateChange);
        }

        void HandleCharacterStateChange(object sender, CharacterStateEventArgsSUNSET e)
        {
            if (!characterID)
                return;
            if (e.CharacterKey == characterID.CharacterKey)
            {
                Debug.Log($"Character {characterID} changed state to {e.StateType}");
                if (e.StateType == StateType.Idle)
                    animationHandler.CrossFadeInFixedTime("Idle", 0.2f);
                else if (e.StateType == StateType.Move)
                    animationHandler.CrossFadeInFixedTime("Locomotion", 0.2f);

                // Respond to the event, e.g., update UI or trigger animations
            }
        }

        public void StartDialogueAudio(int id)
        {
            if (dialogueAudioObject == null)
                return;
            dialogueSource.clip = dialogueAudioObject.dialogues.FirstOrDefault(x => x.id == id)!.audioClip;

            if (dialogueSource.clip != null)
                dialogueSource.Play();
        }

        public void StartAnimationDialogue(string animationName)
        {
            var randomNumber = Random.Range(1, 5);
            animationHandler.CrossFadeInFixedTime(animationName + randomNumber, 0.2f);
        }

#if UNITY_EDITOR

        public void CreateCharacterKey()
        {
            var characterKey = AssetCreator.NewCharacterKey();
            UnityEditor.AssetDatabase.SaveAssets();
        }

#endif
    }
}