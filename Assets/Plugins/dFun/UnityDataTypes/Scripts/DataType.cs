using System;

namespace DFun.UnityDataTypes
{
    public class DataType
    {
        public readonly Type Type;
        public readonly string Name;

        public DataType(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}";
        }
    }
}