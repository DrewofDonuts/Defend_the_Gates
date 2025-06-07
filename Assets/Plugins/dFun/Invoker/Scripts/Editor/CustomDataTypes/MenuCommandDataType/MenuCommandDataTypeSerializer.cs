using System.Text;
using DFun.UnityDataTypes;
using UnityEditor;

namespace DFun.Invoker
{
    public class MenuCommandDataTypeSerializer : IDataTypeSerializer
    {
        public string DefaultSerializedValue => Serialize(DefaultValue);
        private static readonly MenuCommand DefaultValue = new MenuCommand(default);

        private const char Splitter = ';';

        private StringBuilder _sb;
        private Int32DataTypeSerializer _intSerializer;
        private UnityObjectDataTypeSerializer _unityObjectSerializer;

        public string Serialize(object value)
        {
            if (value == null) return string.Empty;

            if (_sb == null) _sb = new StringBuilder();
            _sb.Clear();

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();
            if (_unityObjectSerializer == null) _unityObjectSerializer = new UnityObjectDataTypeSerializer();

            MenuCommand menuCommandValue = (MenuCommand)value;

            return _sb
                .Append(_unityObjectSerializer.Serialize(menuCommandValue.context)).Append(Splitter)
                .Append(_intSerializer.Serialize(menuCommandValue.userData))
                .ToString();
        }

        public object Deserialize(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue)) return DefaultValue;

            string[] split = stringValue.Split(Splitter);
            if (split.Length != 2) return DefaultValue;

            if (_intSerializer == null) _intSerializer = new Int32DataTypeSerializer();
            if (_unityObjectSerializer == null) _unityObjectSerializer = new UnityObjectDataTypeSerializer();

            return new MenuCommand(
                _unityObjectSerializer.DeserializeAndCast(split[0]),
                _intSerializer.DeserializeAndCast(split[1])
            );
        }
    }
}