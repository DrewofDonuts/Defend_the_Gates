#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace Etheral
{
    public class ObjectFaderEditorWindow : EditorWindow
    {
        public Material[] materials; // Changed from GameObject[] to Material[]
        public string directory;

        Color originalColor = Color.black;


        [MenuItem("Tools/Etheral/Object Fader Editor")]
        public static void ShowWindow()
        {
            GetWindow<ObjectFaderEditorWindow>("Object Fader Editor");
        }

        void OnGUI()
        {
            GUILayout.Label("Replace Materials", EditorStyles.boldLabel);

            originalColor = GUI.backgroundColor;

            //not sure what this does
            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            //get the property of the materials field
            SerializedProperty property = serializedObject.FindProperty("materials");
            EditorGUILayout.PropertyField(property, true);

            //required to make materials field work
            serializedObject.ApplyModifiedProperties();


            GUILayout.BeginHorizontal(GUILayout.Height(100));
            GUILayout.FlexibleSpace();

            var guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.wordWrap = true;
            guiStyle.normal.textColor = Color.white;


            GUILayout.Label("Instructions \n 1. Add materials to duplicate and rename. " +
                            "\n2. Select Directory to place new materials in " +
                            "\n3. Create Transparent Materials - manually change new materials to Transparent" +
                            "\n4. Replace Materials in scene that have or whose parent has ObjectFader",
                guiStyle);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(50));
            GUILayout.FlexibleSpace();

            GUI.backgroundColor = Color.green;


            if (GUILayout.Button("Select Target Directory", GUILayout.Height(25), GUILayout.Width(200)))
            {
                directory = EditorUtility.OpenFolderPanel("Select Target Directory", "Assets/Etheral/Art", "");
            }

            GUI.backgroundColor = originalColor;
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(GUILayout.Height(50));

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create Transparent Materials", GUILayout.Height(25), GUILayout.Width(200)))
            {
                foreach (var _material in materials)
                {
                    var newMaterial = new Material(_material);
                    newMaterial.name = _material.name + "_transparent";

                    AssetDatabase.CreateAsset(newMaterial,
                        RemoveFirstPart(directory) + "/" + newMaterial.name + ".mat");
                }

                materials = null;
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();


            //set Horizontal Height

            GUILayout.BeginHorizontal(GUILayout.Height(50));

            GUILayout.FlexibleSpace();

            // Replace Materials button
            if (GUILayout.Button("Replace Materials", GUILayout.Height(25), GUILayout.Width(200)))
            {
                if (directory == null)
                {
                    throw new Exception("No directory selected");
                }

                MaterialReplacer.ReplaceMaterials(RemoveFirstPart(directory));
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (directory != null)
                GUILayout.Label("Target Directory: " + RemoveFirstPart(directory));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        static string RemoveFirstPart(string input)
        {
            int index = input.IndexOf("assets", StringComparison.OrdinalIgnoreCase);

            if (index != -1)
            {
                // Add the length of indexto skip until Assets
                return input.Substring(index);
            }

            return input;
        }
    }
}
#endif