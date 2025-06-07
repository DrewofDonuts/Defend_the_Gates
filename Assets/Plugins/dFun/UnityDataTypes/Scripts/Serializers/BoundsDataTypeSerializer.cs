using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class BoundsDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Bounds DefaultValue = new Bounds();

        private const char Splitter = '|';

        private StringBuilder _sb;
        private Vector3DataTypeSerializer _vector3Serializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_vector3Serializer == null) _vector3Serializer = new Vector3DataTypeSerializer();

            Bounds bounds = (Bounds)value;
            return _sb
                .Append(_vector3Serializer.Serialize(bounds.center)).Append(Splitter)
                .Append(_vector3Serializer.Serialize(bounds.size))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] parts = stringValue.Split(Splitter);
            if (parts.Length != 2) return DefaultValue;

            if (_vector3Serializer == null) _vector3Serializer = new Vector3DataTypeSerializer();

            return new Bounds(
                _vector3Serializer.DeserializeAndCast(parts[0]),
                _vector3Serializer.DeserializeAndCast(parts[1])
            );
        }
    }
}