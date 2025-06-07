using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "Narrator Dialogue", menuName = "Etheral/Narrator/Narrator Dialogue")]
    [InlineEditor]
    public class NarratorDialogue : ScriptableObject
    {
        [SerializeField]  NarratorDialogueInfo dialogue;
        public NarratorDialogueInfo Dialogue => dialogue;


        
        #if UNITY_EDITOR
        void OnValidate()
        {
            
            if(string.IsNullOrEmpty(dialogue.Title)) return;

            string path = AssetDatabase.GetAssetPath(this);
            string currentName = System.IO.Path.GetFileNameWithoutExtension(path);
            
            if(name != dialogue.Title )
            {
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.RenameAsset(path, dialogue.Title);
                    AssetDatabase.SaveAssets();
                };
            }
        }
        
        #endif
    }
}