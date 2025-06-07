using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class RectDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Rect DefaultValue = new Rect();

        private const char Splitter = '|';

        private StringBuilder _sb;
        private Vector2DataTypeSerializer _vector2Serializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_vector2Serializer == null) _vector2Serializer = new Vector2DataTypeSerializer();

            Rect rect = (Rect)value;
            return _sb
                .Append(_vector2Serializer.Serialize(rect.position)).Append(Splitter)
                .Append(_vector2Serializer.Serialize(rect.size))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] parts = stringValue.Split(Splitter);
            if (parts.Length != 2) return DefaultValue;

            if (_vector2Serializer == null) _vector2Serializer = new Vector2DataTypeSerializer();

            return new Rect(
                _vector2Serializer.DeserializeAndCast(parts[0]),
                _vector2Serializer.DeserializeAndCast(parts[1])
            );
        }
    }
}