using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Etheral
{
    [CustomNodeEditor(typeof(ConditionNode))]
    public class ConditionalNodeEditor : NodeEditor
    {
        ConditionNode node;
        bool showSettings = true;


        public override void OnBodyGUI()
        {
            if (node == null)
                node = target as ConditionNode;
            
            serializedObject.Update();

            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("entry"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("pass"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("fail"), GUILayout.MinWidth(0));


            FoldoutMenu();

            //show the EventKey in the editor
            if (node.condition != null && node.condition.EventKey != null)
                EditorGUILayout.LabelField("Event: " + node.condition.EventKey.Value);
            
    
                for (int i = 0; i < node.condition.ConditionMetElements.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    node.condition.ConditionMetElements[i].description =
                        EditorGUILayout.TextField(node.condition.ConditionMetElements[i].description);
                    if (GUILayout.Button("Remove"))
                    {
                        node.condition.ConditionMetElements.RemoveAt(i);
                        i--;
                    }

                    EditorGUILayout.EndHorizontal();
                }
        }

        void FoldoutMenu()
        {
            showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Settings");

            if (showSettings)
            {
                if (GUILayout.Button("Add Condition"))
                {
                    node.condition.ConditionMetElements.Add(new BoolElement());
                }

                foreach (var condition in node.condition.ConditionMetElements)
                {
                    if (condition == null) continue;
                    condition.description = EditorGUILayout.TextField(condition.description);
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}