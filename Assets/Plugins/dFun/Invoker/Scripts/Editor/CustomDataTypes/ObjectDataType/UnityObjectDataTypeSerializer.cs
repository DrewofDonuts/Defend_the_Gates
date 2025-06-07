using System;
using DFun.UnityDataTypes;
using UnityEditor;

namespace DFun.Invoker
{
    public class UnityObjectDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Object DefaultValue = default;

        public string Serialize(object value)
        {
            UnityEngine.Object unityObjectValue = (UnityEngine.Object)value;
            if (unityObjectValue == null) return string.Empty;

            return GlobalObjectIdCached.GetObjectId(unityObjectValue).ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            if (!GlobalObjectId.TryParse(stringValue, out GlobalObjectId globalObjectId))
            {
                return DefaultValue;
            }
            return GlobalObjectIdCached.GetObject(globalObjectId);
        }

        public UnityEngine.Object DeserializeAndCast(string stringValue)
        {
            return (UnityEngine.Object)Deserialize(stringValue);
        }
    }
}