using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New String Object", menuName = "Etheral/Data Objects/String Object")]
    [InlineEditor]
    public class StringObject : ScriptableObject
    {
        [field: SerializeField] public string Value { get; private set; }

        public void SetValue(string value)
        {
            Value = value;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            Value = fileName;
        }
#endif
    }
}