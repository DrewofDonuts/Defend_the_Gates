using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;


[NodeTint(.01f, .50f, .38f)]
[NodeWidth(300)]
public class StoryNode : BaseNode
{
    [Input] public float entry;
    [Output] public int exit;

    [TextArea(3, 10)]
    public string description;

    public Sprite storySprite;
    public string title;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "exit") return GetInputValue<float>("entry", entry);
        else return null;
    }
}