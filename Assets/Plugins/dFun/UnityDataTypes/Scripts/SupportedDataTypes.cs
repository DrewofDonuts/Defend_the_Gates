using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace DFun.UnityDataTypes
{
    public static class SupportedDataTypes
    {
        public static readonly DataType Unknown = new DataType(typeof(void), "unknown");

        public static readonly DataType Bool = new DataType(typeof(Boolean), "bool");

        public static readonly DataType Byte = new DataType(typeof(Byte), "byte");
        public static readonly DataType SByte = new DataType(typeof(SByte), "sbyte");

        public static readonly DataType Char = new DataType(typeof(Char), "char");

        public static readonly DataType Double = new DataType(typeof(Double), "double");

        public static readonly DataType Int16 = new DataType(typeof(Int16), "short");
        public static readonly DataType Int32 = new DataType(typeof(Int32), "int");
        public static readonly DataType Int64 = new DataType(typeof(Int64), "long");

        public static readonly DataType Single = new DataType(typeof(Single), "float");

        public static readonly DataType String = new DataType(typeof(String), "string");

        public static readonly DataType UInt16 = new DataType(typeof(UInt16), "ushort");
        public static readonly DataType UInt32 = new DataType(typeof(UInt32), "uint");
        public static readonly DataType UInt64 = new DataType(typeof(UInt64), "ulong");
        public static readonly DataType Enum = new EnumDataType(typeof(Enum));

        public static readonly DataType Bounds = new DataType(typeof(Bounds), "Bounds");
        public static readonly DataType BoundsInt = new DataType(typeof(BoundsInt), "BoundsInt");
        public static readonly DataType Color = new DataType(typeof(Color), "Color");
        public static readonly DataType AnimationCurve = new DataType(typeof(AnimationCurve), "AnimationCurve");
        public static readonly DataType Gradient = new DataType(typeof(Gradient), "Gradient");
        public static readonly DataType Rect = new DataType(typeof(Rect), "Rect");
        public static readonly DataType RectInt = new DataType(typeof(RectInt), "RectInt");
        public static readonly DataType Vector2 = new DataType(typeof(Vector2), "Vector2");
        public static readonly DataType Vector2Int = new DataType(typeof(Vector2Int), "Vector2Int");
        public static readonly DataType Vector3 = new DataType(typeof(Vector3), "Vector3");
        public static readonly DataType Vector3Int = new DataType(typeof(Vector3Int), "Vector3int");
        public static readonly DataType Vector4 = new DataType(typeof(Vector4), "Vector4");
        public static readonly DataType Quaternion = new DataType(typeof(Quaternion), "Quaternion");
        public static readonly DataType Matrix4X4 = new DataType(typeof(Matrix4x4), "Matrix4x4");
        public static readonly DataType LayerMask = new DataType(typeof(LayerMask), "LayerMask");

        private static readonly Dictionary<string, DataType> StringTypeToDataType = new Dictionary<string, DataType>();

        private static DataType[] _listOfTypes = new DataType[]
        {
            //system data types
            Bool,
            Byte,
            SByte,
            Char,
            Double,
            Int16,
            Int32,
            Int64,
            Single,
            String,
            UInt16,
            UInt32,
            UInt64,
            Enum,

            //Unity data types
            Bounds,
            BoundsInt,
            Color,
            AnimationCurve,
            Gradient,
            Rect,
            RectInt,
            Vector2,
            Vector2Int,
            Vector3,
            Vector3Int,
            Vector4,
            Quaternion,
            Matrix4X4,
            LayerMask
        };
        public static DataType[] ListOfTypes => _listOfTypes;

        public static DataType GetDataType(Type dataType)
        {
            return GetDataType(
                GetTypeName(dataType)
            );
        }

        public static DataType GetDataType(string dataTypeString)
        {
            if (!StringTypeToDataType.ContainsKey(dataTypeString))
            {
                StringTypeToDataType[dataTypeString] = FindDataType(dataTypeString);
            }

            return StringTypeToDataType[dataTypeString];
        }

        /// <returns>true if the newDataType was added successfully; otherwise, false.</returns>
        public static bool AddDataType(DataType newDataType)
        {
            Assert.IsNotNull(newDataType, "Data type can't be null");
            if (GetDataType(newDataType.Type) == Unknown)
            {
                DataType[] newListOfTypes = new DataType[_listOfTypes.Length + 1];
                Array.Copy(_listOfTypes, newListOfTypes, _listOfTypes.Length);
                newListOfTypes[newListOfTypes.Length - 1] = newDataType;
                _listOfTypes = newListOfTypes;

                StringTypeToDataType[GetTypeName(newDataType.Type)] = newDataType;

                return true;
            }
            return false;
        }

        private static DataType FindDataType(string dataTypeString)
        {
            DataType[] supportedDataTypes = ListOfTypes;
            for (int i = 0, iSize = supportedDataTypes.Length; i < iSize; i++)
            {
                if (GetTypeName(supportedDataTypes[i].Type) == dataTypeString)
                {
                    return supportedDataTypes[i];
                }
            }

            Type type = Type.GetType(dataTypeString, false);
            if (type != null && type.IsEnum)
            {
                return new EnumDataTypeWrapper(type);
            }

            return Unknown;
        }

        private static string GetTypeName(Type dataType)
        {
            return dataType.AssemblyQualifiedName;
        }
    }
}