using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector4DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Vector4 DefaultValue = Vector4.zero;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private SingleDataTypeSerializer _floatSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            Vector4 vector4 = (Vector4)value;
            return _sb
                .Append(_floatSerializer.Serialize(vector4.x)).Append(Splitter)
                .Append(_floatSerializer.Serialize(vector4.y)).Append(Splitter)
                .Append(_floatSerializer.Serialize(vector4.z)).Append(Splitter)
                .Append(_floatSerializer.Serialize(vector4.w))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;
            string[] split = stringValue.Split(Splitter);
            if (split.Length != 4) return DefaultValue;

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            return new Vector4(
                _floatSerializer.DeserializeAndCast(split[0]),
                _floatSerializer.DeserializeAndCast(split[1]),
                _floatSerializer.DeserializeAndCast(split[2]),
                _floatSerializer.DeserializeAndCast(split[3])
            );
        }

        public Vector4 DeserializeAndCast(string stringValue)
        {
            return (Vector4)Deserialize(stringValue);
        }
    }
}