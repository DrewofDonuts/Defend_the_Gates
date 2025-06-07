namespace DFun.UnityDataTypes
{
    public class UInt64DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly ulong DefaultValue = new ulong();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            ulong ulongValue = (ulong)value;
            return ulongValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (ulong.TryParse(stringValue, out ulong parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}