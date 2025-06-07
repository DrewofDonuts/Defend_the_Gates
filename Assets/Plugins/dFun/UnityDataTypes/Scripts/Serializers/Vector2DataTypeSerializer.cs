using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector2DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Vector2 DefaultValue = Vector2.zero;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private SingleDataTypeSerializer _floatSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            Vector2 vector2 = (Vector2)value;
            return _sb
                .Append(_floatSerializer.Serialize(vector2.x)).Append(Splitter)
                .Append(_floatSerializer.Serialize(vector2.y))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;
            string[] split = stringValue.Split(Splitter);
            if (split.Length != 2) return DefaultValue;

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            return new Vector2(
                _floatSerializer.DeserializeAndCast(split[0]),
                _floatSerializer.DeserializeAndCast(split[1])
            );
        }
        
        public Vector2 DeserializeAndCast(string stringValue)
        {
            return (Vector2)Deserialize(stringValue);
        }
    }
}