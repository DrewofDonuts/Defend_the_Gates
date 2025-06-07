using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


namespace Etheral
{
    [CreateAssetMenu(fileName = "Scene Data", menuName = "Etheral/Scene/Scene Data")]
    [InlineEditor]
    public class SceneData : ScriptableObject
    {
        [Header("Scene Settings")]
        [Tooltip("Must match the scene name that should be loaded")]
        [SerializeField] string sceneName;
        [Tooltip("Key is used to satisfy the condition in order to load the scene")]
        [SerializeField] string sceneKey;


        [Header("Scene Description")]
        [TextArea]
        [SerializeField] string sceneDetails;

        public string SceneName => sceneName;
        public string SceneKey => sceneKey;


#if UNITY_EDITOR

        void OnValidate()
        {
            if (string.IsNullOrEmpty(sceneName)) return;

            string path = AssetDatabase.GetAssetPath(this);
            string currentName = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name != sceneName)
            {
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.RenameAsset(path, sceneName);
                    AssetDatabase.SaveAssets();
                };
            }
        }

#endif
    }
}