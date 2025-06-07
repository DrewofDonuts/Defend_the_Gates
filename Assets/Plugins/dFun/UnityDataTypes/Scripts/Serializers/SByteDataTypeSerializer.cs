namespace DFun.UnityDataTypes
{
    public class SByteDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly sbyte DefaultValue = new sbyte();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            sbyte sbyteValue = (sbyte)value;
            return sbyteValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (sbyte.TryParse(stringValue, out sbyte parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}