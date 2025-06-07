namespace DFun.UnityDataTypes
{
    public class CharDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly char DefaultValue = new char();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            char charValue = (char)value;
            return charValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (char.TryParse(stringValue, out char parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}