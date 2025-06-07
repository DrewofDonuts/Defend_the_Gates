using System;

namespace DFun.Invoker
{
    [Flags]
    public enum MethodsTypes
    {
        //access modifier types
        Public = 1 << 0,
        NonPublic = 1 << 1,

        //special types
        Static = 1 << 2,
        NonVoidReturn = 1 << 3,
        Properties = 1 << 4
    }
}