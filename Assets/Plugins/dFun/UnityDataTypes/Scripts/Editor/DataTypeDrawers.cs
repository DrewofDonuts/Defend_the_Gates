using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace DFun.UnityDataTypes
{
    public static class DataTypeDrawers
    {
        private static readonly Dictionary<DataType, IDataTypeDrawer> DataTypeToDrawer =
            new Dictionary<DataType, IDataTypeDrawer>()
            {
                { SupportedDataTypes.Bool, new BoolDataTypeDrawer() },
                { SupportedDataTypes.Byte, new ByteDataTypeDrawer() },
                { SupportedDataTypes.SByte, new SByteDataTypeDrawer() },
                { SupportedDataTypes.Char, new CharDataTypeDrawer() },
                { SupportedDataTypes.Double, new DoubleDataTypeDrawer() },
                { SupportedDataTypes.Int16, new Int16DataTypeDrawer() },
                { SupportedDataTypes.Int32, new Int32DataTypeDrawer() },
                { SupportedDataTypes.Int64, new Int64DataTypeDrawer() },
                { SupportedDataTypes.Single, new SingleDataTypeDrawer() },
                { SupportedDataTypes.String, new StringDataTypeDrawer() },
                { SupportedDataTypes.UInt16, new UInt16DataTypeDrawer() },
                { SupportedDataTypes.UInt32, new UInt32DataTypeDrawer() },
                { SupportedDataTypes.UInt64, new UInt64DataTypeDrawer() },
                { SupportedDataTypes.Enum, new EnumDataTypeDrawer() },

                { SupportedDataTypes.Bounds, new BoundsDataTypeDrawer() },
                { SupportedDataTypes.BoundsInt, new BoundsIntDataTypeDrawer() },
                { SupportedDataTypes.Color, new ColorDataTypeDrawer() },
                { SupportedDataTypes.AnimationCurve, new AnimationCurveDataTypeDrawer() },
                { SupportedDataTypes.Gradient, new GradientDataTypeDrawer() },
                { SupportedDataTypes.Rect, new RectDataTypeDrawer() },
                { SupportedDataTypes.RectInt, new RectIntDataTypeDrawer() },
                { SupportedDataTypes.Vector2, new Vector2DataTypeDrawer() },
                { SupportedDataTypes.Vector2Int, new Vector2IntDataTypeDrawer() },
                { SupportedDataTypes.Vector3, new Vector3DataTypeDrawer() },
                { SupportedDataTypes.Vector3Int, new Vector3IntDataTypeDrawer() },
                { SupportedDataTypes.Vector4, new Vector4DataTypeDrawer() },
                { SupportedDataTypes.Quaternion, new QuaternionDataTypeDrawer() },
                { SupportedDataTypes.Matrix4X4, new Matrix4X4DataTypeDrawer() },
                { SupportedDataTypes.LayerMask, new LayerMaskDataTypeDrawer() },
            };

        public static IDataTypeDrawer GetDataTypeDrawer(Type type)
        {
            return GetDataTypeDrawer(SupportedDataTypes.GetDataType(type));
        }

        public static IDataTypeDrawer GetDataTypeDrawer(string dataTypeString)
        {
            return GetDataTypeDrawer(SupportedDataTypes.GetDataType(dataTypeString));
        }

        public static IDataTypeDrawer GetDataTypeDrawer(DataType dataType)
        {
            if (DataTypeToDrawer.TryGetValue(dataType, out IDataTypeDrawer drawer))
            {
                return drawer;
            }

            if (dataType is EnumDataTypeWrapper)
            {
                return DataTypeToDrawer[SupportedDataTypes.Enum];
            }

            return default;
        }

        /// <returns>true if the newDataType was added successfully; otherwise, false.</returns>
        public static bool AddDataTypeDrawer(DataType dataType, IDataTypeDrawer drawer)
        {
            Assert.IsNotNull(dataType, "Data type can't be null");
            Assert.IsNotNull(drawer, "Drawer type can't be null");
            if (DataTypeToDrawer.ContainsKey(dataType))
            {
                return false;
            }

            DataTypeToDrawer.Add(dataType, drawer);
            return true;
        }
    }
}