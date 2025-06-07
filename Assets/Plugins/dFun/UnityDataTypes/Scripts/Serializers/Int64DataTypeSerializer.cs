namespace DFun.UnityDataTypes
{
    public class Int64DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly long DefaultValue = new long();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            long longValue = (long)value;
            return longValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (long.TryParse(stringValue, out long parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}