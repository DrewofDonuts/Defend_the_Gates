using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace DFun.UnityDataTypes
{
    public static class DataTypeSerialization
    {
        private static readonly Dictionary<DataType, IDataTypeSerializer> DataTypeToSerializer =
            new Dictionary<DataType, IDataTypeSerializer>()
            {
                { SupportedDataTypes.Bool, new BoolDataTypeSerializer() },
                { SupportedDataTypes.Byte, new ByteDataTypeSerializer() },
                { SupportedDataTypes.SByte, new SByteDataTypeSerializer() },
                { SupportedDataTypes.Char, new CharDataTypeSerializer() },
                { SupportedDataTypes.Double, new DoubleDataTypeSerializer() },
                { SupportedDataTypes.Int16, new Int16DataTypeSerializer() },
                { SupportedDataTypes.Int32, new Int32DataTypeSerializer() },
                { SupportedDataTypes.Int64, new Int64DataTypeSerializer() },
                { SupportedDataTypes.Single, new SingleDataTypeSerializer() },
                { SupportedDataTypes.String, new StringDataTypeSerializer() },
                { SupportedDataTypes.UInt16, new UInt16DataTypeSerializer() },
                { SupportedDataTypes.UInt32, new UInt32DataTypeSerializer() },
                { SupportedDataTypes.UInt64, new UInt64DataTypeSerializer() },
                { SupportedDataTypes.Enum, new EnumDataTypeSerializer() },

                { SupportedDataTypes.Bounds, new BoundsDataTypeSerializer() },
                { SupportedDataTypes.BoundsInt, new BoundsIntDataTypeSerializer() },
                { SupportedDataTypes.Color, new ColorDataTypeSerializer() },
                { SupportedDataTypes.AnimationCurve, new AnimationCurveDataTypeSerializer() },
                { SupportedDataTypes.Gradient, new GradientDataTypeSerializer() },
                { SupportedDataTypes.Rect, new RectDataTypeSerializer() },
                { SupportedDataTypes.RectInt, new RectIntDataTypeSerializer() },
                { SupportedDataTypes.Vector2, new Vector2DataTypeSerializer() },
                { SupportedDataTypes.Vector2Int, new Vector2IntDataTypeSerializer() },
                { SupportedDataTypes.Vector3, new Vector3DataTypeSerializer() },
                { SupportedDataTypes.Vector3Int, new Vector3IntDataTypeSerializer() },
                { SupportedDataTypes.Vector4, new Vector4DataTypeSerializer() },
                { SupportedDataTypes.Quaternion, new QuaternionDataTypeSerializer() },
                { SupportedDataTypes.Matrix4X4, new Matrix4X4DataTypeSerializer() },
                { SupportedDataTypes.LayerMask, new LayerMaskDataTypeSerializer() },
            };

        public static IDataTypeSerializer GetDataTypeSerializer(Type type)
        {
            return GetDataTypeSerializer(SupportedDataTypes.GetDataType(type));
        }

        public static IDataTypeSerializer GetDataTypeSerializer(string dataTypeString)
        {
            return GetDataTypeSerializer(SupportedDataTypes.GetDataType(dataTypeString));
        }

        public static IDataTypeSerializer GetDataTypeSerializer(DataType dataType)
        {
            if (DataTypeToSerializer.TryGetValue(dataType, out IDataTypeSerializer serializer))
            {
                return serializer;
            }

            if (dataType is EnumDataTypeWrapper)
            {
                return DataTypeToSerializer[SupportedDataTypes.Enum];
            }

            return default;
        }

        /// <returns>true if the serializer was added successfully; otherwise, false.</returns>
        public static bool AddSerializer(DataType dataType, IDataTypeSerializer serializer)
        {
            Assert.IsNotNull(dataType, "Data type can't be null");
            Assert.IsNotNull(serializer, "Serializer type can't be null");
            if (DataTypeToSerializer.ContainsKey(dataType))
            {
                return false;
            }

            DataTypeToSerializer.Add(dataType, serializer);
            return true;
        }

        public static string GetDefaultSerializedValue(Type type)
        {
            return GetDataTypeSerializer(type).DefaultSerializedValue;
        }

        public static string GetDefaultSerializedValue(string dataTypeString)
        {
            return GetDataTypeSerializer(dataTypeString).DefaultSerializedValue;
        }
    }
}