using UnityEngine;
using UnityEngine.Serialization;
using XNode;

[NodeTint(.0117f, .46f, .50f)]
[NodeWidth(300)]
public class PixelCrusherNode : BaseNode
{
    [Input] public float entry;
    [Output] public int exit;

    [TextArea(3, 10)]
    public string description;

    public string dialogueName;
    public string questName;
    public Sprite storySprite;

    
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "exit") return GetInputValue<float>("entry", entry);
        else return null;
    }
}