using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


namespace Etheral
{
    [CreateAssetMenu(fileName = "New Event Key", menuName = "Etheral/Keys/Event Key")]
    public class EventKey : StringObject
    {
        [field: TextArea(3, 10)]
        [field: SerializeField] public string EventDescription { get; set; }

        [field: HorizontalGroup("Config", Width = 125)]
        [field: LabelWidth(100)]
        [field: SerializeField] public bool IsDialogue { get; set; }

        [field: HorizontalGroup("Config", Width = 125)]
        [field: LabelWidth(100)]
        [field: SerializeField] public bool IsCinematic { get; set; }


        [field: HorizontalGroup("Config")]
        [field: LabelWidth(125)]
        [field: SerializeField] public bool HasEventCondition { get; set; }

        // [field: VerticalGroup("Config/Right")]
        // [field: VerticalGroup("Config/Left")]

        [field: ShowIf("HasEventCondition")]
        [field: SerializeField] public EventKey PreviousEventKeyCondition { get; set; }
        public string KeyTitle { get; private set; }

#if UNITY_EDITOR
        void OnValidate()
        {
            KeyTitle = "";
            KeyTitle = NameConverter.ConvertToName(Value);
            
            if (PreviousEventKeyCondition != null)
            {
                if (PreviousEventKeyCondition == this || PreviousEventKeyCondition.Value == Value)
                {
                    EditorUtility.DisplayDialog("Error adding Event Key",
                        "You cannot add the same Event Key as a previous condition", "Ok");
                    PreviousEventKeyCondition = null;
                }
            }
        }
#endif
    }
}