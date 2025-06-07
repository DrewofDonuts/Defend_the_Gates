namespace DFun.UnityDataTypes
{
    public class UInt32DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly uint DefaultValue = new uint();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            uint uintValue = (uint)value;
            return uintValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (uint.TryParse(stringValue, out uint parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}