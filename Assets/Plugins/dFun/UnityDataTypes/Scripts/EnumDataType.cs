using System;

namespace DFun.UnityDataTypes
{
    public class EnumDataType : DataType
    {
        public EnumDataType(Type type) : base(type, string.Empty)
        {
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}