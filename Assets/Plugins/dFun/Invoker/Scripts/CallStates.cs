using System;

namespace DFun.Invoker
{
    [Flags]
    public enum CallStates
    {
        EditorMode = 1 << 0,
        PlayMode = 1 << 1
    }
}