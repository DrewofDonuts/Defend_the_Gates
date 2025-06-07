using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class CharacterActionConverterUI : MonoBehaviour
    {
        public PlayerAttributes playerAttributes;

        [Button("Convert Actions")]
        public void ConvertActions()
        {
            List<CharacterAction> characterActions = new List<CharacterAction>();

            // Use reflection to get all fields of PlayerAttributes
            FieldInfo[] fields =
                typeof(PlayerAttributes).GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                // Check if the field type is CharacterAction
                if (field.FieldType == typeof(CharacterAction))
                {
                    // Get the value of the field and add it to the list
                    CharacterAction action = field.GetValue(playerAttributes) as CharacterAction;
                    if (action != null)
                    {
                        characterActions.Add(action);
                    }
                }
            }

            foreach (var characterAction in characterActions)
            {
                var newCharacterAction = CharacterActionConverter.DeepCopy(characterAction);
                AssetCreator.CreateNewCharacterActionObject(newCharacterAction);
            }

            // Debug or process the list of CharacterActions
            Debug.Log($"Found {characterActions.Count} CharacterAction fields.");
        }
    }
}