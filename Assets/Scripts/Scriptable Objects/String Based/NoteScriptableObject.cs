using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Note", menuName = "Etheral/Data Objects/Note")]
    [InlineEditor]
    public class NoteScriptableObject : ScriptableObject
    {
        public List<Note> Notes = new();
        public List<ColorNote> ColorNotes = new();
    }
    
    [Serializable]
    public class Note
    {
        [LabelWidth(30)]
        [SerializeField] string title;

        [SerializeField] string date;
        [SerializeField] bool completed;

        [TextArea(5, 16)]
        [SerializeField] string body;
    }

    [Serializable]
    public class ColorNote
    {
        [SerializeField] string title;
        [SerializeField] string color;
        
        [ColorPalette("Button Color")]
        public Color ButtonColor;
    }
}