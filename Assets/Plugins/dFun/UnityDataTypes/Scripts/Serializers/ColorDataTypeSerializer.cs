using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class ColorDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Color DefaultValue = Color.white;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private SingleDataTypeSerializer _floatSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            Color color = (Color)value;
            return _sb
                .Append(_floatSerializer.Serialize(color.r)).Append(Splitter)
                .Append(_floatSerializer.Serialize(color.g)).Append(Splitter)
                .Append(_floatSerializer.Serialize(color.b)).Append(Splitter)
                .Append(_floatSerializer.Serialize(color.a))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;
            string[] split = stringValue.Split(Splitter);
            if (split.Length != 4) return DefaultValue;

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            return new Color(
                _floatSerializer.DeserializeAndCast(split[0]),
                _floatSerializer.DeserializeAndCast(split[1]),
                _floatSerializer.DeserializeAndCast(split[2]),
                _floatSerializer.DeserializeAndCast(split[3])
            );
        }

        public Color DeserializeAndCast(string stringValue)
        {
            return (Color)Deserialize(stringValue);
        }
    }
}