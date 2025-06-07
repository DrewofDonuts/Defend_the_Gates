using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Etheral
{
    [CustomNodeEditor(typeof(PixelCrusherNode))]
    public class PixelCrusherNodeEditor : NodeEditor
    {
        PixelCrusherNode node;
        bool showConf;

        public override void OnBodyGUI()
        {
            if (node == null)
            {
                node = target as PixelCrusherNode;
            }

            serializedObject.Update();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(1));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("entry"), GUILayout.MinWidth(0));
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.Label("Description/Dialogue");
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginVertical(GUILayout.Width(1));
            SpritePreview();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(1));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("exit"), GUILayout.MinWidth(0));
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();
            GUIContent emptyLabel = new GUIContent("");
            EditorGUIUtility.labelWidth = 1;
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("description"), emptyLabel);

            showConf = EditorGUILayout.Foldout(showConf, "Configuration");
            if (showConf)
            {
                var prev = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 100;
                node.dialogueName = EditorGUILayout.TextField("Dialogue Name", node.dialogueName);
                node.questName = EditorGUILayout.TextField("Quest Name", node.questName);
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("storySprite"));
                EditorGUIUtility.labelWidth = prev;
                node.name = $"Dialogue: {node.dialogueName}" + "     " + $"Quest: {node.questName}";
                
            }
        }
        
        void SpritePreview()
        {
            if (node.storySprite != null)
                node.storySprite =
                    (Sprite)EditorGUILayout.ObjectField("", node.storySprite, typeof(Sprite), true);
        }
    }
}