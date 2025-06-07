using System.Collections;
using UnityEngine;

namespace Etheral
{
    public abstract class NPCBaseState : AIBaseState
    {
        protected NPCStateMachine stateMachine;


        public NPCBaseState(NPCStateMachine npcStateMachine) : base(npcStateMachine)
        {
            stateMachine = npcStateMachine;
            animationHandler = stateMachine.GetAIComponents().GetAnimationHandler();
        }
    }
}