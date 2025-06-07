namespace DFun.UnityDataTypes
{
    public class Int32DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly int DefaultValue = new int();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            int intValue = (int)value;
            return intValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (int.TryParse(stringValue, out int parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }

        public int DeserializeAndCast(string stringValue)
        {
            return (int)Deserialize(stringValue);
        }
    }
}