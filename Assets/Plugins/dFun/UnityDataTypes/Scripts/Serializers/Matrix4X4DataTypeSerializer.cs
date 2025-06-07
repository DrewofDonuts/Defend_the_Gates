using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class Matrix4X4DataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Matrix4x4 DefaultValue = Matrix4x4.zero;

        private const char Splitter = '|';

        private StringBuilder _sb;
        private Vector4DataTypeSerializer _vector4Serializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_vector4Serializer == null) _vector4Serializer = new Vector4DataTypeSerializer();

            Matrix4x4 matrix4X4 = (Matrix4x4)value;


            return _sb
                .Append(_vector4Serializer.Serialize(matrix4X4.GetColumn(0))).Append(Splitter)
                .Append(_vector4Serializer.Serialize(matrix4X4.GetColumn(1))).Append(Splitter)
                .Append(_vector4Serializer.Serialize(matrix4X4.GetColumn(2))).Append(Splitter)
                .Append(_vector4Serializer.Serialize(matrix4X4.GetColumn(3)))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;
            string[] split = stringValue.Split(Splitter);
            if (split.Length != 4) return DefaultValue;

            if (_vector4Serializer == null) _vector4Serializer = new Vector4DataTypeSerializer();

            return new Matrix4x4(
                _vector4Serializer.DeserializeAndCast(split[0]),
                _vector4Serializer.DeserializeAndCast(split[1]),
                _vector4Serializer.DeserializeAndCast(split[2]),
                _vector4Serializer.DeserializeAndCast(split[3])
            );
        }
    }
}