using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public class DialogueSystemEtheralTrigger : MonoBehaviour
{
    [SerializeField] string variableName;
    [SerializeField] bool variableValue;


    [Button("Get Variable from Dialogue System")]
    public void GetVariableFromDialogueSystem()
    {
        var s = DialogueLua.GetVariable("Test").asBool;
        Debug.Log(s);
    }

    [Button("Create Variable in  Dialogue System")]
    public void SetDialogueSystemVariable()
    {
        DialogueLua.SetVariable(variableName, variableValue);
    }
}