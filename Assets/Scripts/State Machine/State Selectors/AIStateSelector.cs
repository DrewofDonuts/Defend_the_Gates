namespace Etheral
{
    public abstract class AIStateSelector : StateSelector
    {
        public abstract void ToggleHostile(bool value);
        public abstract void EnterPatrolState();

        public abstract void EnterChaseState();

        public abstract void EnterFleeState();
        
        public abstract void EnterCombatState();
        
        public abstract void EnterFollowState();


        public override void EnterDialogueState()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterEventState(string s)
        {
        }
    }
}