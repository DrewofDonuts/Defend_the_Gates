using Etheral;
using Sirenix.OdinInspector;
using UnityEngine;

public class EventTestTrigger : BaseEventTrigger
{
    [Button("Test Event Trigger", ButtonSizes.Large)]
    public void TestEventTrigger()
    {
        StartTriggerCondition();
    }
}