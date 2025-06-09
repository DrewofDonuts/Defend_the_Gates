using System.Collections;
using UnityEngine;

namespace Etheral
{
    public abstract class NPCBaseState : AIBaseState
    {
        protected CompanionStateMachine stateMachine;


        public NPCBaseState(CompanionStateMachine companionStateMachine) : base(companionStateMachine)
        {
            stateMachine = companionStateMachine;
            animationHandler = stateMachine.GetAIComponents().GetAnimationHandler();
        }
    }
}