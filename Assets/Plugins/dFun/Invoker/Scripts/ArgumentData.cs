using System;
using DFun.UnityDataTypes;
using UnityEngine;

namespace DFun.Invoker
{
    [Serializable]
    public class ArgumentData
    {
        [SerializeField] private string dataType;
        [SerializeField] private string argName;
        [SerializeField] private string serializedValue;

        public string DataType => dataType;
        public string ArgName => argName;
        public string SerializedValue => serializedValue;

        public ArgumentData()
        {
        }

        public ArgumentData(string dataType, string argName)
        {
            this.dataType = dataType;
            this.argName = argName;
            this.serializedValue = DataTypeSerialization.GetDefaultSerializedValue(dataType);
        }

        public ArgumentData(string dataType, string argName, string serializedValue)
        {
            this.dataType = dataType;
            this.argName = argName;
            this.serializedValue = serializedValue;
        }

        public ArgumentData(ArgumentData copyFrom)
            : this(copyFrom.dataType, copyFrom.argName, copyFrom.serializedValue)
        {
        }
    }
}