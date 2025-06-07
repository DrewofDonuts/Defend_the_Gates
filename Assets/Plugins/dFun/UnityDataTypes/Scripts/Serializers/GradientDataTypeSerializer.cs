using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class GradientDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly Gradient DefaultValue = new Gradient();

        private const char FieldsSplitter = '\n';
        private const char LineDataSplitter = '|';
        private const char KeyDataSplitter = '#';

        private StringBuilder _sb;
        private Int32DataTypeSerializer _intSerializer;
        private SingleDataTypeSerializer _floatSerializer;
        private ColorDataTypeSerializer _colorSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();
            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();
            if (_colorSerializer == null) _colorSerializer = new ColorDataTypeSerializer();

            Gradient gradient = (Gradient)value;

            _sb.Append(_intSerializer.Serialize(gradient.mode));
            _sb.Append(FieldsSplitter);
            SerializeColorKeys(gradient.colorKeys, _sb);
            _sb.Append(FieldsSplitter);
            SerializeAlphaKeys(gradient.alphaKeys, _sb);

            return _sb.ToString();
        }

        private void SerializeColorKeys(GradientColorKey[] colorKeys, StringBuilder sb)
        {
            for (int i = 0, iSize = colorKeys.Length; i < iSize; i++)
            {
                SerializeColorKey(colorKeys[i], sb);
                if (i < iSize - 1)
                {
                    sb.Append(KeyDataSplitter);
                }
            }
        }

        private GradientColorKey[] DeserializeColorKeys(string[] split)
        {
            List<GradientColorKey> colorKeys = new List<GradientColorKey>(split.Length);

            for (int i = 0, iSize = split.Length; i < iSize; i++)
            {
                if (DeserializeColorKey(split[i], out GradientColorKey result))
                {
                    colorKeys.Add(result);
                }
            }

            return colorKeys.ToArray();
        }

        private void SerializeColorKey(GradientColorKey colorKey, StringBuilder sb)
        {
            sb.Append(_colorSerializer.Serialize(colorKey.color)).Append(LineDataSplitter)
                .Append(_floatSerializer.Serialize(colorKey.time));
        }

        private bool DeserializeColorKey(string serializedColorKey, out GradientColorKey result)
        {
            string[] keyParts = serializedColorKey.Split(LineDataSplitter);
            if (keyParts.Length != 2)
            {
                result = default;
                return false;
            }

            result = new GradientColorKey(
                _colorSerializer.DeserializeAndCast(keyParts[0]),
                _floatSerializer.DeserializeAndCast(keyParts[1])
            );
            return true;
        }

        private void SerializeAlphaKeys(GradientAlphaKey[] alphaKeys, StringBuilder sb)
        {
            for (int i = 0, iSize = alphaKeys.Length; i < iSize; i++)
            {
                SerializeAlphaKey(alphaKeys[i], sb);
                if (i < iSize - 1)
                {
                    sb.Append(KeyDataSplitter);
                }
            }
        }

        private GradientAlphaKey[] DeserializeAlphaKeys(string[] split)
        {
            List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>(split.Length);

            for (int i = 0, iSize = split.Length; i < iSize; i++)
            {
                if (DeserializeAlphaKey(split[i], out GradientAlphaKey result))
                {
                    alphaKeys.Add(result);
                }
            }

            return alphaKeys.ToArray();
        }

        private void SerializeAlphaKey(GradientAlphaKey alphaKey, StringBuilder sb)
        {
            sb.Append(_floatSerializer.Serialize(alphaKey.alpha)).Append(LineDataSplitter)
                .Append(_floatSerializer.Serialize(alphaKey.time));
        }

        private bool DeserializeAlphaKey(string serializedAlphaKey, out GradientAlphaKey result)
        {
            string[] keyParts = serializedAlphaKey.Split(LineDataSplitter);
            if (keyParts.Length != 2)
            {
                result = default;
                return false;
            }

            result = new GradientAlphaKey(
                _floatSerializer.DeserializeAndCast(keyParts[0]),
                _floatSerializer.DeserializeAndCast(keyParts[1])
            );
            return true;
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] parts = stringValue.Split(FieldsSplitter);
            if (parts.Length != 3) return DefaultValue;

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();
            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();
            if (_colorSerializer == null) _colorSerializer = new ColorDataTypeSerializer();

            GradientMode gradientMode = (GradientMode)_intSerializer.Deserialize(parts[0]);
            GradientColorKey[] colorKeys = DeserializeColorKeys(parts[1].Split(KeyDataSplitter));
            GradientAlphaKey[] alphaKeys = DeserializeAlphaKeys(parts[2].Split(KeyDataSplitter));

            Gradient gradient = new Gradient();
            gradient.mode = gradientMode;
            gradient.SetKeys(colorKeys, alphaKeys);

            return gradient;
        }
    }
}