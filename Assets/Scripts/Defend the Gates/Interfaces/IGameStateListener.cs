namespace Etheral.DefendTheGates
{
    //only use for LISTENERS that need to be updated during runtime. Do not use for classes that simply check state.
    public interface IGameStateListener
    {
        public GameState CurrentGameState { get;}
        void OnGameStateChanged(GameState newGameState);
        void RegisterListener();
        void UnregisterListener();
    }
}