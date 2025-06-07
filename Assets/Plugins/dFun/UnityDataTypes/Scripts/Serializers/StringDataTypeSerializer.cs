namespace DFun.UnityDataTypes
{
    public class StringDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly string DefaultValue = string.Empty;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            string stringValue = (string)value;
            return stringValue;
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            return stringValue;
        }
    }
}