using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public class AnimationCurveDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly AnimationCurve DefaultValue = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private const char KeyDataSplitter = ':';
        private const char KeysSplitter = '|';

        private StringBuilder _sb;
        private SingleDataTypeSerializer _floatSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            AnimationCurve animationCurve = (AnimationCurve)value;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();

            Keyframe[] keyframes = animationCurve.keys;
            for (int i = 0, iSize = keyframes.Length; i < iSize; i++)
            {
                Keyframe key = keyframes[i];
                _sb.Append(_floatSerializer.Serialize(key.time)).Append(KeyDataSplitter)
                    .Append(_floatSerializer.Serialize(key.value)).Append(KeyDataSplitter)
                    .Append(_floatSerializer.Serialize(key.inTangent)).Append(KeyDataSplitter)
                    .Append(_floatSerializer.Serialize(key.outTangent)).Append(KeyDataSplitter)
                    .Append(_floatSerializer.Serialize(key.inWeight)).Append(KeyDataSplitter)
                    .Append(_floatSerializer.Serialize(key.outWeight));

                if (i < iSize - 1)
                {
                    _sb.Append(KeysSplitter);
                }
            }
            return _sb.ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;
            
            string[] sKeyFrames = stringValue.Split(KeysSplitter);
            List<Keyframe> keys = new List<Keyframe>(sKeyFrames.Length);

            if (_floatSerializer == null) _floatSerializer = new SingleDataTypeSerializer();
            
            for (int i = 0, iSize = sKeyFrames.Length; i < iSize; i++)
            {
                string sKeyFrame = sKeyFrames[i];
                if (string.IsNullOrEmpty(sKeyFrame)) continue;

                string[] parts = sKeyFrame.Split(KeyDataSplitter);
                if (parts.Length != 6) continue;

                float time = _floatSerializer.DeserializeAndCast(parts[0]);
                float value = _floatSerializer.DeserializeAndCast(parts[1]);
                float inTangent = _floatSerializer.DeserializeAndCast(parts[2]);
                float outTangent = _floatSerializer.DeserializeAndCast(parts[3]);
                float inWeight = _floatSerializer.DeserializeAndCast(parts[4]);
                float outWeight = _floatSerializer.DeserializeAndCast(parts[5]);

                keys.Add(new Keyframe(time, value, inTangent, outTangent, inWeight, outWeight));
            }

            return new AnimationCurve(keys.ToArray());
        }
    }
}