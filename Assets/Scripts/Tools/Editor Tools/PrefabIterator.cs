using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
#if UNITY_EDITOR
    public class PrefabIterator : EditorWindow
    {
        public GameObject parentPrefab;
        public string baseName;
        public GameObject[] prefabVariants;

        [MenuItem("Tools/Etheral/Prefab Iterator")]
        public static void ShowWindow()
        {
            GetWindow<PrefabIterator>("Prefab Iterator");
        }

        void OnGUI()
        {
            var guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.wordWrap = true;
            guiStyle.normal.textColor = Color.white;

            GUILayout.Label("Instructions \n 1. Add Parent Prefab. " +
                            "\n2. Input base name to name the files " +
                            "\n3. Add objects to child to Parent Prefab" +
                            "\n4. Create Prefab Variants which will instantiate in scene" +
                            "\n5. Drag to project and save as prefab-variants ",
                guiStyle);


            //not sure what this does
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty property = serializedObject.FindProperty("parentPrefab");
            EditorGUILayout.PropertyField(property, true);

            SerializedProperty property2 = serializedObject.FindProperty("baseName");
            EditorGUILayout.PropertyField(property2, true);

            SerializedProperty property3 = serializedObject.FindProperty("prefabVariants");
            EditorGUILayout.PropertyField(property3, true);

            // if (GUILayout.Button("Select Target Directory", GUILayout.Height(25), GUILayout.Width(200)))
            // {
            //     directory = EditorUtility.OpenFolderPanel("Select Target Directory", "Assets/Etheral", "");
            // }

            serializedObject.ApplyModifiedProperties();


            if (GUILayout.Button("Create Prefab Variants"))
            {
                foreach (var prefabVariant in prefabVariants)
                {
                    CreatePrefabVariant(prefabVariant, baseName, parentPrefab);
                }
            }
        }

        static void CreatePrefabVariant(GameObject prefab, string baseName, GameObject parentPrefab)
        {
            GameObject basePrefab = parentPrefab;

            if (basePrefab == null)
            {
                Debug.LogError("Please select a base prefab to create a variant.");
                return;
            }

            // Create a variant of the base prefab
            GameObject prefabVariant = PrefabUtility.InstantiatePrefab(basePrefab) as GameObject;

            if (prefabVariant != null)
            {
                // Save the changes to the prefab variant
                PrefabUtility.ApplyPrefabInstance(prefabVariant, InteractionMode.AutomatedAction);
                Debug.Log("Prefab variant created successfully");

                parentPrefab.gameObject.name = baseName + "-" + prefab.name;

                PrefabUtility.InstantiatePrefab(prefab, prefabVariant.transform);
            }
            else
            {
                Debug.LogError("Failed to create prefab variant");
            }
        }
    }


#endif
}