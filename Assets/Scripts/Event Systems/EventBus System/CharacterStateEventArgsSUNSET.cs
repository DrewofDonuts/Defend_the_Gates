using System;

namespace Etheral
{
    //TO REMOVE 
    public class CharacterStateEventArgsSUNSET : EventArgs
    {
        public CharacterKey CharacterKey { get; private set; }
        public StateType StateType { get; private set; }
        
        public CharacterStateEventArgsSUNSET(CharacterKey characterKey, StateType stateType)
        {
            CharacterKey = characterKey;
            StateType = stateType;
        }
        
    }
}