namespace DFun.Invoker
{
    public static class CallStatesUtils
    {
        public static bool IsCallStateSelected(this CallStates allowCallStates, CallStates compareTo)
        {
            return (allowCallStates & compareTo) != 0;
        }
    }
}