using System;

namespace DFun.UnityDataTypes
{
    public class EnumDataTypeWrapper : DataType
    {
        public EnumDataTypeWrapper(Type enumType) : base(enumType, enumType.Name)
        {
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}