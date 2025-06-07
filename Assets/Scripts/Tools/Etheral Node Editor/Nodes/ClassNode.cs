using System.Collections.Generic;
using UnityEngine;
using XNode;


[NodeWidth(300)]
public class ClassNode : BaseNode
{
    public string className;
    public List<string> data;
    
    [TextArea(4, 4)]
    public string description;
    [Input] public float entry;
    [Input] public float callback;
    
    
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "b") return GetInputValue("a", entry);
        else return null;
    }

}