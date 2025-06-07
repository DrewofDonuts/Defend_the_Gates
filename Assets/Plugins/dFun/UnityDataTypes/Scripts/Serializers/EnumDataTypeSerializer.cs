using System;
using System.Text;

namespace DFun.UnityDataTypes
{
    public class EnumDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => throw new Exception(
            "Can't provide enum default value. Use method GetDefaultSerializedValue() instead"
        );

        public const object None = null;
        private const char Splitter = '|';

        private StringBuilder _sb;

        public string GetDefaultSerializedValue(Type enumType)
        {
            return Serialize(
                Enum.GetValues(enumType).GetValue(0)
            );
        }

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            int intValue = (int)value;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            return _sb.Append(value.GetType().AssemblyQualifiedName)
                .Append(Splitter)
                .Append(intValue.ToString())
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return None;

            string[] split = stringValue.Split(Splitter);
            if (split.Length != 2) return None;

            string enumAssemblyQualifiedName = split[0];
            if (!TypeUtils.TryParseClassType(enumAssemblyQualifiedName, out Type enumType))
            {
                return None;
            }

            string enumIntValueStr = split[1];

            if (TryParseEnum(enumType, enumIntValueStr, out var result))
            {
                return result;
            }

            return None;
        }

        private static bool TryParseEnum(Type enumType, string enumIntValueStr, out object result)
        {
#if UNITY_2021_2_OR_NEWER
            if (Enum.TryParse(enumType, enumIntValueStr, out object parsedResult))
            {
                result = parsedResult;
                return true;
            }

            result = default;
            return false;
#else
            try
            {
                result = Enum.Parse(enumType, enumIntValueStr);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = default;
                return false;
            }
#endif
        }
    }
}