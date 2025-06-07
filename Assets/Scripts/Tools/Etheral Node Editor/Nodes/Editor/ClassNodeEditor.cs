using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;


namespace Etheral
{
    [CustomNodeEditor(typeof(ClassNode))]
    public class ClassNodeEditor : NodeEditor
    {
        ClassNode node;
        Color originalColor = GUI.backgroundColor;
        bool showConf;
        string dataOutput = "";
        float prevWidth = EditorGUIUtility.labelWidth;
        GUIStyle wordWrapStyle = new GUIStyle(EditorStyles.label);
        public Color selectedColor = new Color(.12f, .7f, .68f);


        public override void OnBodyGUI()
        {
            GUI.backgroundColor = selectedColor;

            if (node == null)
                node = target as ClassNode;


            serializedObject.Update();

            wordWrapStyle.wordWrap = true;

            DescriptionAndPort();

            if (ConfigFoldout()) return;


            foreach (var p in node.DynamicOutputs)
            {
                NodeEditorGUILayout.PortField(p);
            }

            GUI.backgroundColor = originalColor;
        }

        void DescriptionAndPort()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Description");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(10));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("entry"), GUILayout.MinWidth(0));
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.MinHeight(100));
            GUIContent emptyLabel = new GUIContent("");
            EditorGUIUtility.labelWidth = 1;
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("description"), emptyLabel);
            EditorGUIUtility.labelWidth = prevWidth;
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            
            //TODO: Use this to create custom locations for nodes
            // Rect rect = GUILayoutUtility.GetRect(100, 20);
            // NodeEditorGUILayout.PortField(new Vector2(100, 250), target.GetInputPort("callback"));
        }

        bool ConfigFoldout()
        {
            showConf = EditorGUILayout.BeginFoldoutHeaderGroup(showConf, "Class Settings");

            if (showConf)
            {
                EditorGUIUtility.labelWidth = 100;
                node.className = EditorGUILayout.TextField("Class Name", node.className, GUILayout.MinWidth(200));
                EditorGUIUtility.labelWidth = prevWidth;

                node.name = node.className;

                selectedColor = EditorGUILayout.ColorField("Color", selectedColor);

                if (DynamicPortList()) return true;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            return false;
        }


        bool DynamicPortList()
        {
            EditorGUILayout.PrefixLabel("Output");
            dataOutput = EditorGUILayout.TextField(dataOutput);


            if (GUILayout.Button("Add Data Type"))
            {
                bool noOutput = (dataOutput.Length == 0);
                bool matchesExistingOutput = false;

                foreach (var p in node.DynamicOutputs)
                {
                    if (p.fieldName == dataOutput)
                    {
                        matchesExistingOutput = true;
                        break;
                    }
                }

                if (matchesExistingOutput)
                {
                    EditorUtility.DisplayDialog("Error", "Output already exists", "OK");
                    return true;
                }

                if (noOutput)
                {
                    EditorUtility.DisplayDialog("Error", "No output name", "OK");
                    return true;
                }

                node.AddDynamicOutput(typeof(int), Node.ConnectionType.Multiple, Node.TypeConstraint.None, dataOutput);

                node.data.Add(dataOutput);
            }

            return false;
        }
    }
}