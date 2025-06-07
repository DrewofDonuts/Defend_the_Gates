using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Etheral.CharacterActions
{
    [CreateAssetMenu(menuName = "Etheral/Character Actions/Character Action Object")]
    [InlineEditor]
    public class CharacterActionObject : ScriptableObject
    {
        [SerializeField] CharacterAction characterAction;
        public CharacterAction CharacterAction => characterAction;


#if UNITY_EDITOR

        void OnValidate()
        {
            RenameAsset();
        }

        void RenameAsset()
        {
            if (string.IsNullOrEmpty(characterAction.Name)) return;

            string path = AssetDatabase.GetAssetPath(this);
            string currentName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name != characterAction.Name)
            {
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.RenameAsset(path, characterAction.Name);
                    AssetDatabase.SaveAssets();
                };
            }
        }
#endif
    }
}