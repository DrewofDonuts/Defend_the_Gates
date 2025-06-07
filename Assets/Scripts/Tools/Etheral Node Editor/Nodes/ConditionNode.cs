using System.Collections;
using System.Collections.Generic;
using Etheral;
using UnityEngine;
using XNode;

[NodeWidth(300)]
public class ConditionNode : Node
{
    // Return the correct value of an output port when requested
    [Input] public float entry;
    [Output] public float pass;
    [Output] public float fail;

    public Condition condition;
    
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == pass.ToString()) return GetInputValue<float>("a", entry);
        else return null;
    }
    
    
}