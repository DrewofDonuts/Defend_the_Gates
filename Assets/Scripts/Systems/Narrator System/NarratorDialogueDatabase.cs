using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    //Used in Narrator Dialogue Triggers
    [CreateAssetMenu(fileName = "New Dialogue Database", menuName = "Etheral/Narrator/Dialogue Database")]
    [InlineEditor]
    public class NarratorDialogueDatabase : ScriptableObject
    {
        [SerializeField] LevelDialogue[] levelDialogues;
        [SerializeField] List<NarratorDialogue> unAddedDialogues = new();


#if UNITY_EDITOR
        [Button("Create Level Dialogue", ButtonSizes.Medium), GUIColor(.25f, .50f, .25f)]
        void CreateNarratorDialogue(string _level, string dialogueTitle)
        {
            var newDialogue = AssetCreator.NewNarratorDialogue();
            newDialogue.Dialogue.Title = dialogueTitle;

            //Add the new dialogue to the level dialogues

            var level = levelDialogues.FirstOrDefault(x => x.levelName == _level);
            level?.AddDialogue(newDialogue);
        }

        [Button("Create Text FIle")]
        void CreateTextOfAllDialogueLines()
        {
            string text = "";
            foreach (var levelDialogue in levelDialogues)
            {
                text += $"Level: {levelDialogue.levelName}\n";
                foreach (var dialogue in levelDialogue.narratorDialogueObjects)
                {
                    text += $"Title: {dialogue.Dialogue.Title}\n";
                    foreach (var line in dialogue.Dialogue.dialogueLines)
                    {
                        text += $"{line.sentence}\n";
                    }
                }
            }

            string path = "Assets/NarratorDialogueLines.txt";
            System.IO.File.WriteAllText(path, text);
        }

        [Button("Get Narrator Dialogues not in DB from Project")]
        void GetNarratorDialoguesFromDirectory()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:NarratorDialogue", new[] { "Assets/Etheral/Scriptable Object Assets//Quest and Dialogue SOs/Narrator Dialogue/" });
            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var dialogue = UnityEditor.AssetDatabase.LoadAssetAtPath<NarratorDialogue>(path);

                if (dialogue != null && !unAddedDialogues.Contains(dialogue) && !IsDialogueInLevelDialogues(dialogue))
                {
                    unAddedDialogues.Add(dialogue);
                }
            }
        }

        bool IsDialogueInLevelDialogues(NarratorDialogue dialogue)
        {
            foreach (var levelDialogue in levelDialogues)
            {
                if (levelDialogue.narratorDialogueObjects.Contains(dialogue))
                {
                    return true;
                }
            }
            return false;
        }

#endif
    }
}