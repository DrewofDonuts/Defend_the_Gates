using Etheral;
using UnityEngine;

public class NPCStateBlocks 
{
    NPCBaseState npcBaseState;
    NPCStateMachine stateMachine;
    
    
    public NPCStateBlocks(NPCStateMachine _stateMachine, NPCBaseState npcBaseState)
    {
        stateMachine = _stateMachine;
        this.npcBaseState = npcBaseState;
    }

}
