using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector2IntDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Vector2Int DefaultValue = Vector2Int.zero;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private Int32DataTypeSerializer _intSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();

            Vector2Int vector2Int = (Vector2Int)value;
            return _sb
                .Append(_intSerializer.Serialize(vector2Int.x)).Append(Splitter)
                .Append(_intSerializer.Serialize(vector2Int.y))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] split = stringValue.Split(Splitter);
            if (split.Length != 2) return DefaultValue;

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();

            return new Vector2Int(
                _intSerializer.DeserializeAndCast(split[0]),
                _intSerializer.DeserializeAndCast(split[1])
            );
        }

        public Vector2Int DeserializeAndCast(string stringValue)
        {
            return (Vector2Int)Deserialize(stringValue);
        }
    }
}