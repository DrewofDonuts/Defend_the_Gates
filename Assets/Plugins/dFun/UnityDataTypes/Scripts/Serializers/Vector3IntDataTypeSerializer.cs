using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector3IntDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Vector3Int DefaultValue = Vector3Int.zero;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private Int32DataTypeSerializer _intSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();

            Vector3Int vector3Int = (Vector3Int)value;
            return _sb
                .Append(_intSerializer.Serialize(vector3Int.x)).Append(Splitter)
                .Append(_intSerializer.Serialize(vector3Int.y)).Append(Splitter)
                .Append(_intSerializer.Serialize(vector3Int.z))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] split = stringValue.Split(Splitter);
            if (split.Length != 3) return DefaultValue;

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();

            return new Vector3Int(
                _intSerializer.DeserializeAndCast(split[0]),
                _intSerializer.DeserializeAndCast(split[1]),
                _intSerializer.DeserializeAndCast(split[2])
            );
        }

        public Vector3Int DeserializeAndCast(string stringValue)
        {
            return (Vector3Int)Deserialize(stringValue);
        }
    }
}