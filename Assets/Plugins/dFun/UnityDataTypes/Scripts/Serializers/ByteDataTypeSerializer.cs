using System;

namespace DFun.UnityDataTypes
{
    public class ByteDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly byte DefaultValue = new byte();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            byte byteValue = (byte)value;
            return byteValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (byte.TryParse(stringValue, out byte parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}