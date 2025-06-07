using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class QuaternionDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        public static readonly Quaternion DefaultValue = Quaternion.identity;

        private const char Splitter = ';';

        private StringBuilder _sb;
        private SingleDataTypeSerializer _floatSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            Quaternion q = (Quaternion)value;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            return _sb
                .Append(_floatSerializer.Serialize(q.x)).Append(Splitter)
                .Append(_floatSerializer.Serialize(q.y)).Append(Splitter)
                .Append(_floatSerializer.Serialize(q.z)).Append(Splitter)
                .Append(_floatSerializer.Serialize(q.w))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] split = stringValue.Split(Splitter);
            if (split.Length != 4) return DefaultValue;

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            return new Quaternion(
                _floatSerializer.DeserializeAndCast(split[0]),
                _floatSerializer.DeserializeAndCast(split[1]),
                _floatSerializer.DeserializeAndCast(split[2]),
                _floatSerializer.DeserializeAndCast(split[3])
            );
        }
    }
}