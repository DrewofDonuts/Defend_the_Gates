using System;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class CharacterID : MonoBehaviour
    {
        [InlineButton("CreateCharacterKey", "New")]
        [SerializeField] CharacterKey characterKey;

        public CharacterKey CharacterKey => characterKey;


#if UNITY_EDITOR

        public void CreateCharacterKey()
        {
            characterKey = AssetCreator.NewCharacterKey();
            UnityEditor.AssetDatabase.SaveAssets();
        }


        [Button("Set CharacterID Fields")]
        public void CheckForCharacterIDFields(GameObject obj = null)
        {
            // Check the current object's components for CharacterID fields
            
            if (obj == null)
                obj = gameObject;

            Component[] components = obj.GetComponents<Component>();


            foreach (Component component in components)
            {
                Type type = component.GetType();
                FieldInfo[] fields =
                    type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo field in fields)
                {
                    if (field.FieldType == typeof(CharacterID))
                    {
                        // Found a field of type CharacterID
                        Debug.Log($"Found CharacterID in {type.Name} on {obj.name}");

                        // Perform your desired action here

                        field.SetValue(component, this);
                    }
                }
            }

            // Recursively check each child object
            foreach (Transform child in obj.transform)
            {
                CheckForCharacterIDFields(child.gameObject);
            }
        }

#endif
    }
}