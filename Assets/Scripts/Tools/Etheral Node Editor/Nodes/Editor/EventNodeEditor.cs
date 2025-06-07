using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;


namespace Etheral
{
    [CustomNodeEditor(typeof(EventNode))]
    public class EventNodeEditor : NodeEditor
    {
        EventNode node;

        bool showInput = true;
        bool showFields = false;

        string newDialogueOption = "";
        string newDialogueOptionOutput = "";

        float prev = EditorGUIUtility.labelWidth;

        GUIStyle wordWrapStyle = new GUIStyle(EditorStyles.label);


        public override void OnBodyGUI()
        {
            if (node == null)
            {
                node = target as EventNode;
            }

            wordWrapStyle.wordWrap = true;

            serializedObject.Update();
            
            if (showInput)
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("entry"), GUILayout.MinWidth(0));

            ShowDescriptionAndSprite();

            NodeConfiguration();


            // IfEntryButton();

            // EditorGUIUtility.labelWidth = 150;
            // node.dialogueOptions = EditorGUILayout.Toggle("Show Dialogue options?", node.dialogueOptions);
            // EditorGUIUtility.labelWidth = prev;


            foreach (var p in node.DynamicOutputs)
            {
                NodeEditorGUILayout.PortField(p);
            }
        }

        void ShowDescriptionAndSprite()
        {
            EditorGUILayout.BeginHorizontal();


            EditorGUILayout.BeginVertical(GUILayout.Width(280));

            if (node.eventKey != null)
            {
                //make labelfield bold
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 15;
                EditorGUIUtility.labelWidth = 200;
                EditorGUILayout.LabelField("Description", style);
                EditorGUILayout.TextArea(node.eventKey.EventDescription, wordWrapStyle);
                EditorGUIUtility.labelWidth = prev;
            }
            
            // NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.Width(15));
            SpritePreview();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        void IfEntryButton()
        {
            Color prev = GUI.backgroundColor;
            GUI.backgroundColor = Color.gray;

            if (GUILayout.Button("If Entry"))
            {
                showInput = !showInput;
            }

            GUI.backgroundColor = prev;
        }

        void NodeConfiguration()
        {
            // showFields = EditorGUILayout.BeginFoldoutHeaderGroup(showFields, "Node Settings");

            if (GUILayout.Button("Node Configuration"))
                showFields = !showFields;

            if (showFields)
            {
                if (node.eventKey != null)
                    node.name = node.eventKey.KeyTitle;
                else
                    node.name = "New Event";

                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("eventSprite"));
                EditorGUIUtility.labelWidth = 100;
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("characterKey"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("eventKey"));
                EditorGUIUtility.labelWidth = prev;

                IfEntryButton();

                if (DynamicPortList()) return;
            }

            // EditorGUILayout.EndFoldoutHeaderGroup();
        }

        bool DynamicPortList()
        {
            // EditorGUILayout.PrefixLabel("Dialogue");
            // newDialogueOption = EditorGUILayout.TextField(newDialogueOption);

            EditorGUILayout.PrefixLabel("Output");
            newDialogueOptionOutput = EditorGUILayout.TextField(newDialogueOptionOutput);

            if (GUILayout.Button("Create New Option"))
            {
                // bool noDialogue = (newDialogueOption.Length == 0);
                bool noOutput = (newDialogueOptionOutput.Length == 0);
                bool matchesExistingOutput = false;

                foreach (var p in node.DynamicOutputs)
                {
                    if (p.fieldName == newDialogueOptionOutput)
                    {
                        matchesExistingOutput = true;
                        break;
                    }
                }

                if (matchesExistingOutput)
                {
                    EditorUtility.DisplayDialog("Error creating port", "Output already exists.", "Ok");
                    return true;
                }

                // if (noDialogue)
                // {
                //     EditorUtility.DisplayDialog("Error creating port", "No dialogue was entered.", "Ok");
                //     return true;
                // }

                if (noOutput)
                {
                    EditorUtility.DisplayDialog("Error creating port", "No output was entered.", "Ok");
                    return true;
                }

                node.AddDynamicOutput(typeof(int), Node.ConnectionType.Multiple, Node.TypeConstraint.None,
                    newDialogueOptionOutput);
                node.outcomeList.Add(new Outcomes(newDialogueOption, newDialogueOptionOutput));
            }

            return false;
        }

        void SpritePreview()
        {
            if (node.eventSprite != null)
                node.eventSprite =
                    (Sprite)EditorGUILayout.ObjectField("", node.eventSprite, typeof(Sprite), true);
        }
    }
}


/* -----Adjust Label Width ----- */
/* float prev = EditorGUIUtility.labelWidth;
EditorGUIUtility.labelWidth = 100;
EditorGUILayout.PropertyField(serializedObject.FindProperty("characterName"));
*/

/* -----Text Field ----- */
/* EditorGUILayout.TextField("Character Name", node.characterName); */