using System.Globalization;

namespace DFun.UnityDataTypes
{
    public class DoubleDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly double DefaultValue = new double();

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            double doubleValue = (double)value;
            return doubleValue.ToString(CultureInfo.InvariantCulture);
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string normalizedInput = stringValue.Replace(',', '.');
            if (double.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture,
                    out double parsedResult))
            {
                return parsedResult;
            }

            return DefaultValue;
        }
    }
}