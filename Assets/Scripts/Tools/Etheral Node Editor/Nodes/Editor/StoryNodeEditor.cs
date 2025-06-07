using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Etheral
{
    [CustomNodeEditor(typeof(StoryNode))]
    public class StoryNodeEditor : NodeEditor
    {
        StoryNode node;
        Color originalColor = GUI.backgroundColor;

        bool showConf;

        public override void OnBodyGUI()
        {
            if (node == null)
            {
                node = target as StoryNode;
            }

            serializedObject.Update();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(1));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("entry"), GUILayout.MinWidth(0));
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.Label("Description");
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
                node.title = EditorGUILayout.TextField("Title", node.title);
                EditorGUIUtility.labelWidth = prev;
                node.name = node.title;
                
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("storySprite"));

            }
            
            
            

            GUI.backgroundColor = originalColor;
        }
        
        void SpritePreview()
        {
            if (node.storySprite != null)
                node.storySprite =
                    (Sprite)EditorGUILayout.ObjectField("", node.storySprite, typeof(Sprite), true);
        }
    }
}