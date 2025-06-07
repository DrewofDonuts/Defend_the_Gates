
using UnityEngine;

public interface IBlock 
{
    public bool IsBlocking { get; }
    Transform transform { get; }
    public void TriggerBlockDefense();
}
