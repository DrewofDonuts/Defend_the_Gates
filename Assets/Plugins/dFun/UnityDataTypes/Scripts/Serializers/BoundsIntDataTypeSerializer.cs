using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class BoundsIntDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly BoundsInt DefaultValue = new BoundsInt();

        private const char Splitter = '|';

        private StringBuilder _sb;
        private Vector3IntDataTypeSerializer _vector3IntSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_vector3IntSerializer == null) _vector3IntSerializer = new Vector3IntDataTypeSerializer();

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            BoundsInt boundsInt = (BoundsInt)value;
            return _sb
                .Append(_vector3IntSerializer.Serialize(boundsInt.position)).Append(Splitter)
                .Append(_vector3IntSerializer.Serialize(boundsInt.size))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] parts = stringValue.Split(Splitter);
            if (parts.Length != 2) return DefaultValue;

            if (_vector3IntSerializer == null) _vector3IntSerializer = new Vector3IntDataTypeSerializer();

            return new BoundsInt(
                _vector3IntSerializer.DeserializeAndCast(parts[0]),
                _vector3IntSerializer.DeserializeAndCast(parts[1])
            );
        }
    }
}