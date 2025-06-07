using System.Collections.Generic;
using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

#if UNITY_EDITOR

[NodeWidth(310)]
[NodeTint(100, 100, 100)]
public class EventNode : BaseNode
{
    // [Input] public float entry;
    // [Output] public float exit;

    // [Output(dynamicPortList = true)] public string[] myArray;

    [InlineButton("NewCharacterKey", "New")]
    public CharacterKey characterKey;
    [InlineButton("CreateEventKey", "New")]
    public EventKey eventKey;
    
    public Sprite eventSprite;
    public bool dialogueOptions = false;
    public List<Outcomes> outcomeList;

    [Input] public float entry;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "b") return GetInputValue<float>("a", entry);
        else return null;
    }
    
    public void NewCharacterKey()
    {
        characterKey = AssetCreator.NewCharacterKey();
    }
    public void CreateEventKey()
    {
        eventKey = AssetCreator.NewEventKey();
    }

}


[System.Serializable]
public class Outcomes
{
    // //choice of the option
    public string dialogue;

    //what port to go to
    public string outcome;

    public Outcomes(string dialogue, string outcome)
    {
        this.dialogue = dialogue;
        this.outcome = outcome;
    }
}

#endif
