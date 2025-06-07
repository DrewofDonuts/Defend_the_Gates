namespace DFun.UnityDataTypes
{
    public class UInt16DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly ushort DefaultValue = new ushort();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            ushort ushortValue = (ushort)value;
            return ushortValue.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (ushort.TryParse(stringValue, out ushort parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}