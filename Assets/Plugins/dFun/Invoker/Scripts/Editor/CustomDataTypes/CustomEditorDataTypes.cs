using DFun.UnityDataTypes;

namespace DFun.Invoker
{
    public class CustomEditorDataTypes
    {
        /// List of all the custom types from this assembly
        public static DataType[] TypesList => new[]
        {
            MenuCommandDataType.Instance
        };
    }
}