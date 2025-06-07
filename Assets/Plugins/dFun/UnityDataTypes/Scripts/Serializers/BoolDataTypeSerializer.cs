using System;

namespace DFun.UnityDataTypes
{
    public class BoolDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly bool DefaultValue = new bool();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;
            bool boolValue = (bool)value;

            return boolValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (Boolean.TryParse(stringValue, out bool parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}