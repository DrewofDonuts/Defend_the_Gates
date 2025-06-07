using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class LayerMaskDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly LayerMask DefaultValue = int.MaxValue;

        private Int32DataTypeSerializer _intSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();

            return _intSerializer.Serialize((int)(LayerMask)value);
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();

            return (LayerMask)(int)_intSerializer.Deserialize(stringValue);
        }
    }
}