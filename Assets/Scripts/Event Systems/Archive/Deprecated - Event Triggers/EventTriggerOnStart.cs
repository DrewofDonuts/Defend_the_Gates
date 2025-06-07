using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class EventTriggerOnStart : BaseEventTrigger
    {
        IEnumerator Start()
        {
            yield return new WaitUntil(() => EventManager.Instance && CinematicManager.Instance.IsReady && CharacterManager.Instance.IsReady);
            StartTriggerCondition();
        }
    }
}