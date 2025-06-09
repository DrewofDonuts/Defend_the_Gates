using Etheral;
using UnityEngine;

public class NPCStateBlocks 
{
    NPCBaseState npcBaseState;
    CompanionStateMachine stateMachine;
    
    
    public NPCStateBlocks(CompanionStateMachine _stateMachine, NPCBaseState npcBaseState)
    {
        stateMachine = _stateMachine;
        this.npcBaseState = npcBaseState;
    }

}
