using UnityEngine;

namespace Etheral
{
    public abstract class StateSelector : MonoBehaviour
    {
        public abstract void EnterDialogueStateWithAnimation(string animationName);
        public abstract void EnterDialogueState();
        public abstract void EnterIdleState();
        public abstract void EnterDeadState();
        public abstract void EnterEventState(string s);
    }
}