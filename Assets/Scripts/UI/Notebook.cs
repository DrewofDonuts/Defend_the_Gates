using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
#if UNITY_EDITOR
    public class Notebook : MonoBehaviour
    {
        // [SerializeField] List<Note> notes = new List<Note>();
        [InlineButton("NewNote", "New")]
        [SerializeField] NoteScriptableObject noteObject;
        
        
        public void NewNote()
        {
            var asset = ScriptableObject.CreateInstance<NoteScriptableObject>();
            UnityEditor.AssetDatabase.CreateAsset(asset,
                 "Assets/Etheral/Scriptable Object Assets/Notebooks/NewNote.asset");
            UnityEditor.AssetDatabase.SaveAssets();
            noteObject = asset;
        }
        
    }
#endif
}