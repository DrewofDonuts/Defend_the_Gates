using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Vector3DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        public static readonly Vector3 DefaultValue = Vector3.zero;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private SingleDataTypeSerializer _floatSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            Vector3 vector3 = (Vector3)value;
            return _sb
                .Append(_floatSerializer.Serialize(vector3.x)).Append(Splitter)
                .Append(_floatSerializer.Serialize(vector3.y)).Append(Splitter)
                .Append(_floatSerializer.Serialize(vector3.z))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;
            string[] split = stringValue.Split(Splitter);
            if (split.Length != 3) return DefaultValue;

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            return new Vector3(
                _floatSerializer.DeserializeAndCast(split[0]),
                _floatSerializer.DeserializeAndCast(split[1]),
                _floatSerializer.DeserializeAndCast(split[2])
            );
        }

        public Vector3 DeserializeAndCast(string stringValue)
        {
            return (Vector3)Deserialize(stringValue);
        }
    }
}