using System;

namespace DFun.UnityDataTypes
{
    public class ContextMenuData
    {
        public bool WasShown { get; set; } = false;

        public object NonSerializedValue { get; set; }
        public Action<object> OnValueChange { get; set; } = o => { };
    }
}