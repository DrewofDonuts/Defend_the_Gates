using System;

namespace DFun.Invoker
{
    public class ArgumentInfo
    {
        public readonly Type DataType;
        public readonly string ArgName;

        public ArgumentInfo(Type dataType, string argName)
        {
            DataType = dataType;
            ArgName = argName;
        }
    }
}