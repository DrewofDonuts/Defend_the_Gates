using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
#if UNITY_EDITOR
    public class ChangeRenderingLayerMask : EditorWindow
    {
        public GameObject[] gameObjects;
        public RenderingLayerMask renderingLayerMask;
        [ReadOnly]
        public List<SkinnedMeshRenderer> previewSkinnedMeshRenderers;
        [ReadOnly]
        public List<MeshRenderer> previewMeshRenderers;


        [MenuItem("Tools/Etheral/Change Renderer Layer Mask")]
        public static void ShowWindow()
        {
            GetWindow<ChangeRenderingLayerMask>("Change Renderer Layer Mask");
        }

        void OnGUI()
        {
            GUILayout.Label("Change Rendering Layer Mask", EditorStyles.boldLabel);

            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty property = serializedObject.FindProperty("gameObjects");
            EditorGUILayout.PropertyField(property, true);

            SerializedProperty property2 = serializedObject.FindProperty("renderingLayerMask");
            EditorGUILayout.PropertyField(property2, true);

            SerializedProperty property3 = serializedObject.FindProperty("previewSkinnedMeshRenderers");
            EditorGUILayout.PropertyField(property3, true);
            SerializedProperty property4 = serializedObject.FindProperty("previewMeshRenderers");
            EditorGUILayout.PropertyField(property4, true);

            if (GUILayout.Button("Preview Layer Mask"))
                PreviewLayerMask();

            if (GUILayout.Button("Apply Layer Mask"))
                ApplyLayerMask();

            serializedObject.ApplyModifiedProperties();
        }

        void PreviewLayerMask()
        {
            previewSkinnedMeshRenderers = new List<SkinnedMeshRenderer>();
            previewMeshRenderers = new List<MeshRenderer>();

            foreach (var anObject in gameObjects)
            {
                SkinnedMeshRenderer[] skinnedMeshRenderers = anObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    Debug.Log(
                        $"Previewing SkinnedMeshRenderer: {skinnedMeshRenderer.name} with Layer Mask: {renderingLayerMask}");
                    previewSkinnedMeshRenderers.Add(skinnedMeshRenderer);
                }
            }

            foreach (var anObject in gameObjects)
            {
                MeshRenderer[] meshRenderers = anObject.GetComponentsInChildren<MeshRenderer>();
                foreach (var meshRenderer in meshRenderers)
                {
                    previewMeshRenderers.Add(meshRenderer);
                }
            }
        }

        void ApplyLayerMask()
        {
            foreach (var anObject in gameObjects)
            {
                SkinnedMeshRenderer[] skinnedMeshRenderers = anObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    Debug.Log(
                        $"Applying Layer Mask: {renderingLayerMask} to SkinnedMeshRenderer: {skinnedMeshRenderer.name}");
                    skinnedMeshRenderer.renderingLayerMask = renderingLayerMask;
                    EditorUtility.SetDirty(skinnedMeshRenderer);
                }
            }

            foreach (var anObject in gameObjects)
            {
                MeshRenderer[] meshRenderers = anObject.GetComponentsInChildren<MeshRenderer>();

                if (meshRenderers.Length > 0)
                {
                    foreach (var meshRenderer in meshRenderers)
                    {
                        meshRenderer.renderingLayerMask = renderingLayerMask;
                        EditorUtility.SetDirty(meshRenderer);
                    }
                }
            }

            AssetDatabase.SaveAssets();

            gameObjects = null;
            previewSkinnedMeshRenderers.Clear();
            previewMeshRenderers.Clear();
        }
    }
#endif
}