namespace DFun.UnityDataTypes
{
    public class Int16DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly short DefaultValue = new short();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            short shortValue = (short)value;
            return shortValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (short.TryParse(stringValue, out short parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}