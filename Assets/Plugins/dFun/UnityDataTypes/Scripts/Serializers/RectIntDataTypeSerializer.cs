using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class RectIntDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly RectInt DefaultValue = new RectInt();

        private const char Splitter = '|';

        private StringBuilder _sb;
        private Vector2IntDataTypeSerializer _vector2IntSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_vector2IntSerializer == null) _vector2IntSerializer = new Vector2IntDataTypeSerializer();

            RectInt rectInt = (RectInt)value;
            return _sb
                .Append(_vector2IntSerializer.Serialize(rectInt.position)).Append(Splitter)
                .Append(_vector2IntSerializer.Serialize(rectInt.size))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] parts = stringValue.Split(Splitter);
            if (parts.Length != 2) return DefaultValue;

            if (_vector2IntSerializer == null) _vector2IntSerializer = new Vector2IntDataTypeSerializer();

            return new RectInt(
                _vector2IntSerializer.DeserializeAndCast(parts[0]),
                _vector2IntSerializer.DeserializeAndCast(parts[1])
            );
        }
    }
}