using System;
using NUnit.Framework;
using UnityEngine;

namespace DFun.UnityDataTypes.Tests
{
    public class DataTypesSerializationTest
    {
        [Test]
        public void TestDefaultDataTypesSerialization()
        {
            DataType[] supportedDataTypes = SupportedDataTypes.ListOfTypes;
            TestDefaultDataTypesSerialization(supportedDataTypes);
        }

        public static void TestDefaultDataTypesSerialization(DataType[] dataTypes)
        {
            for (int i = 0, iSize = dataTypes.Length; i < iSize; i++)
            {
                DataType dataType = dataTypes[i];
                if (dataType is EnumDataType)
                {
                    continue;
                }
                IDataTypeSerializer dataTypeSerializer = DataTypeSerialization.GetDataTypeSerializer(dataType);
                Assert.IsNotNull(dataTypeSerializer);

                string defaultSerializedValue = DataTypeSerialization.GetDefaultSerializedValue(dataType.Type);
                object deserializedValue = dataTypeSerializer.Deserialize(defaultSerializedValue);
                string serializedValue = dataTypeSerializer.Serialize(deserializedValue);

                UnityEngine.Assertions.Assert.AreEqual(defaultSerializedValue, serializedValue);
            }
        }

        public static void TestDataTypeBase<T>(T originalValue, Func<T, T, bool> customComparator = null)
        {
            DataType type = SupportedDataTypes.GetDataType(typeof(T));
            IDataTypeSerializer serializer = DataTypeSerialization.GetDataTypeSerializer(type);

            string serializedValue = serializer.Serialize(originalValue);
            object deserializedValue = serializer.Deserialize(serializedValue);
            T deserializedCastedValue = (T)deserializedValue;

            if (customComparator != null)
            {
                UnityEngine.Assertions.Assert.IsTrue(customComparator.Invoke(originalValue, deserializedCastedValue));
            }
            else
            {
                UnityEngine.Assertions.Assert.AreEqual(originalValue, deserializedCastedValue);
            }
        }

        private static void TestDataTypeDeserializeBase<T>(T originalValue, string[] serializedValues,
            Func<T, T, bool> customComparator = null)
        {
            UnityEngine.Assertions.Assert.IsTrue(serializedValues.Length >= 2);

            DataType type = SupportedDataTypes.GetDataType(typeof(T));
            IDataTypeSerializer serializer = DataTypeSerialization.GetDataTypeSerializer(type);

            T[] deserializedValues = new T[serializedValues.Length];

            for (int i = 0, iSize = serializedValues.Length; i < iSize; i++)
            {
                string serializedValue = serializedValues[i];
                object deserializedValue = serializer.Deserialize(serializedValue);
                deserializedValues[i] = (T)deserializedValue;
            }

            if (customComparator != null)
            {
                for (int i = 0, iSize = deserializedValues.Length; i < iSize; i++)
                {
                    UnityEngine.Assertions.Assert.IsTrue(customComparator.Invoke(originalValue, deserializedValues[i]));
                }
            }
            else
            {
                for (int i = 1, iSize = deserializedValues.Length; i < iSize; i++)
                {
                    UnityEngine.Assertions.Assert.AreEqual(originalValue, deserializedValues[i]);
                }
            }
        }

        private static bool AreTheSame(float f1, float f2)
        {
            return Math.Abs(f1 - f2) < float.Epsilon;
        }

        [Test]
        public void TestDataType_Bool()
        {
            TestDataTypeBase(true);
        }

        [Test]
        public void TestDataType_Byte()
        {
            TestDataTypeBase<byte>(42);
        }

        [Test]
        public void TestDataType_SByte()
        {
            TestDataTypeBase<sbyte>(-42);
        }

        [Test]
        public void TestDataType_Char()
        {
            TestDataTypeBase('u');
        }

        [Test]
        public void TestDataType_Int16()
        {
            TestDataTypeBase<short>(42);
        }

        [Test]
        public void TestDataType_Int32()
        {
            TestDataTypeBase(42);
        }

        [Test]
        public void TestDataType_Int64()
        {
            TestDataTypeBase<long>(42);
        }

        [Test]
        public void TestDataType_Single()
        {
            float testValue = 42.28f;

            TestDataTypeBase(testValue);

            string dotSerializedValue = "42.28";
            string commaSerializedValue = "42,28";
            TestDataTypeDeserializeBase(testValue, new[] { dotSerializedValue, commaSerializedValue });
        }

        [Test]
        public void TestDataType_Double()
        {
            double testValue = 42.24d;

            TestDataTypeBase(testValue);

            string dotSerializedValue = "42.24";
            string commaSerializedValue = "42,24";
            TestDataTypeDeserializeBase(testValue, new[] { dotSerializedValue, commaSerializedValue });
        }

        [Test]
        public void TestDataType_String()
        {
            TestDataTypeBase("foo-bar");
        }

        [Test]
        public void TestDataType_UInt16()
        {
            TestDataTypeBase<ushort>(42);
        }

        [Test]
        public void TestDataType_UInt32()
        {
            TestDataTypeBase<uint>(42);
        }

        [Test]
        public void TestDataType_UInt64()
        {
            TestDataTypeBase<ulong>(42);
        }

        [Test]
        public void TestDataType_Bounds()
        {
            TestDataTypeBase(new Bounds(
                    new Vector3(12.21f, 23.32f, 34.35f),
                    new Vector3(145.5f, 56.65f, 78.87f)
                )
            );
        }

        [Test]
        public void TestDataType_BoundsInt()
        {
            TestDataTypeBase(new BoundsInt(12, 23, 35, 145, 56, 78));
        }

        [Test]
        public void TestDataType_Color()
        {
            TestDataTypeBase(Color.grey);
        }

        [Test]
        public void TestDataType_AnimationCurve()
        {
            TestDataTypeBase(
                AnimationCurve.EaseInOut(0.5f, 0.42f, 2f, 0.14f),
                (ac1, ac2) =>
                {
                    Keyframe[] keys1 = ac1.keys;
                    Keyframe[] keys2 = ac2.keys;

                    if (keys1.Length != keys2.Length) return false;

                    for (int i = 0, iSize = keys1.Length; i < iSize; i++)
                    {
                        Keyframe key1 = keys1[i];
                        Keyframe key2 = keys2[i];

                        if (!AreTheSame(key1.time, key2.time)) return false;
                        if (!AreTheSame(key1.value, key2.value)) return false;
                        if (!AreTheSame(key1.inTangent, key2.inTangent)) return false;
                        if (!AreTheSame(key1.outTangent, key2.outTangent)) return false;
                        if (!AreTheSame(key1.inWeight, key2.inWeight)) return false;
                        if (!AreTheSame(key1.outWeight, key2.outWeight)) return false;
                    }

                    return true;
                }
            );
        }

        [Test]
        public void TestDataType_Gradient()
        {
            Gradient gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.blue, 0f),
                    new GradientColorKey(Color.red, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 0.75f)
                }
            );
            TestDataTypeBase(gradient, (g1, g2) =>
            {
                if (g1.mode != g2.mode) return false;

                GradientColorKey[] colorKeys1 = g1.colorKeys;
                GradientColorKey[] colorKeys2 = g2.colorKeys;
                if (colorKeys1.Length != colorKeys2.Length) return false;

                for (int i = 0, iSize = colorKeys1.Length; i < iSize; i++)
                {
                    GradientColorKey colorKey1 = colorKeys1[i];
                    GradientColorKey colorKey2 = colorKeys2[i];

                    if (colorKey1.color != colorKey2.color) return false;
                    if (!AreTheSame(colorKey1.time, colorKey2.time)) return false;
                }

                GradientAlphaKey[] alphaKeys1 = g1.alphaKeys;
                GradientAlphaKey[] alphaKeys2 = g2.alphaKeys;

                if (alphaKeys1.Length != alphaKeys2.Length) return false;

                for (int i = 0, iSize = alphaKeys1.Length; i < iSize; i++)
                {
                    GradientAlphaKey alphaKey1 = alphaKeys1[i];
                    GradientAlphaKey alphaKey2 = alphaKeys2[i];

                    if (!AreTheSame(alphaKey1.alpha, alphaKey2.alpha)) return false;
                    if (!AreTheSame(alphaKey1.time, alphaKey2.time)) return false;
                }

                return true;
            });
        }

        [Test]
        public void TestDataType_Rect()
        {
            TestDataTypeBase(new Rect(1f, 2f, 3f, 4f));
        }

        [Test]
        public void TestDataType_RectInt()
        {
            TestDataTypeBase(new RectInt(1, 2, 3, 4));
        }

        [Test]
        public void TestDataType_Vector2()
        {
            TestDataTypeBase(new Vector2(1f, 2f));
        }

        [Test]
        public void TestDataType_Vector2Int()
        {
            TestDataTypeBase(new Vector2Int(4, 2));
        }

        [Test]
        public void TestDataType_Vector3()
        {
            TestDataTypeBase(new Vector3(1f, 2f, 3f));
        }

        [Test]
        public void TestDataType_Vector3Int()
        {
            TestDataTypeBase(new Vector3Int(1, 2, 3));
        }

        [Test]
        public void TestDataType_Vector4()
        {
            TestDataTypeBase(new Vector4(1f, 2f, 3f, 4f));
        }

        [Test]
        public void TestDataType_Quaternion()
        {
            TestDataTypeBase(
                Quaternion.Euler(new Vector3(-15f, 30f, 90f)),
                (q1, q2) =>
                    AreTheSame(q1.x, q2.x)
                    && AreTheSame(q2.y, q2.y)
                    && AreTheSame(q1.z, q2.z)
                    && AreTheSame(q1.w, q2.w)
            );
        }

        [Test]
        public void TestDataType_Matrix4X4()
        {
            TestDataTypeBase(new Matrix4x4(
                new Vector4(0.1f, 0.2f, 0.3f, 0.4f),
                new Vector4(0.5f, 0.6f, 0.7f, 0.8f),
                new Vector4(0.9f, 0.10f, 0.11f, 0.12f),
                new Vector4(0.13f, 0.14f, 0.15f, 0.16f)
            ));
        }

        [Test]
        public void TestDataType_LayerMask()
        {
            TestDataTypeBase<LayerMask>(int.MaxValue);
        }
    }
}