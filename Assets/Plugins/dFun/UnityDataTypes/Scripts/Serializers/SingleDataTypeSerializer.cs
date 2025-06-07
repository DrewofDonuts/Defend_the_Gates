using System.Globalization;

namespace DFun.UnityDataTypes
{
    public class SingleDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly float DefaultValue = new float();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            float floatValue = (float)value;
            return floatValue.ToString(CultureInfo.InvariantCulture);
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string normalizedInput = stringValue.Replace(',', '.');
            if (float.TryParse(normalizedInput, NumberStyles.Float, CultureInfo.InvariantCulture,
                    out float parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }

        public float DeserializeAndCast(string stringValue)
        {
            return (float)Deserialize(stringValue);
        }
    }
}