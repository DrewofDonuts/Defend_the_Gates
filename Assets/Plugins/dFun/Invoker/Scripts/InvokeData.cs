using System;
using DFun.UnityDataTypes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Invoker
{
    [Serializable]
    public class InvokeData
    {
        [SerializeField] private MethodsTypes allowedMethods = MethodsTypes.Public
                                                               | MethodsTypes.Static
                                                               | MethodsTypes.Properties;

        [SerializeField] private CallStates callState = CallStates.EditorMode
                                                        | CallStates.PlayMode;
        [SerializeField] private string componentAssemblyQualifiedName = string.Empty;
        [SerializeField] private int componentIndex = 0;
        [SerializeField] private string methodName = string.Empty;
        [SerializeField] private ArgumentData[] argumentsData = Array.Empty<ArgumentData>();

        public MethodsTypes AllowedMethods => allowedMethods;
        public CallStates CallState => callState;
        public string ComponentAssemblyQualifiedName => componentAssemblyQualifiedName;
        public int ComponentIndex => componentIndex;
        public string MethodName => methodName;
        public ArgumentData[] ArgumentsData => argumentsData;

        public Object ObjectInstance { get; set; }

        public bool HasAssignedMethod => !string.IsNullOrEmpty(componentAssemblyQualifiedName)
                                         && !string.IsNullOrEmpty(methodName);

        public static readonly InvokeData Default = new InvokeData();

        public InvokeData()
        {
        }

        public InvokeData(
            MethodsTypes allowedMethods, CallStates callState,
            string componentAssemblyQualifiedName, int componentIndex,
            string methodName, ArgumentInfo[] arguments)
        {
            this.allowedMethods = allowedMethods;
            this.callState = callState;
            this.componentAssemblyQualifiedName = componentAssemblyQualifiedName;
            this.componentIndex = componentIndex;
            this.methodName = methodName;

            argumentsData = new ArgumentData[arguments.Length];
            for (int i = 0, iSize = arguments.Length; i < iSize; i++)
            {
                Type argumentType = arguments[i].DataType;
                argumentsData[i] = new ArgumentData(
                    argumentType.AssemblyQualifiedName, arguments[i].ArgName
                );
            }
        }

        public InvokeData(InvokeData copyFrom)
        {
            allowedMethods = copyFrom.AllowedMethods;
            callState = copyFrom.CallState;
            componentAssemblyQualifiedName = copyFrom.ComponentAssemblyQualifiedName;
            componentIndex = copyFrom.ComponentIndex;
            methodName = copyFrom.MethodName;
            argumentsData = new ArgumentData[copyFrom.ArgumentsData.Length];
            for (int i = 0, iSize = copyFrom.ArgumentsData.Length; i < iSize; i++)
            {
                argumentsData[i] = new ArgumentData(copyFrom.argumentsData[i]);
            }
        }

        public void Invoke(bool catchExceptions = true, bool logExceptions = true)
        {
            Invoke(ObjectInstance, catchExceptions, logExceptions);
        }


        public void Invoke(Object objectInstance, bool catchExceptions = true, bool logExceptions = true)
        {
            InvokeDataHelper.Invoke(this, objectInstance, catchExceptions, logExceptions);
        }

        public void Invoke(Type type, bool catchExceptions = true, bool logExceptions = true)
        {
            InvokeDataHelper.Invoke(this, type, catchExceptions, logExceptions);
        }

        public Type[] GetArgumentsTypes()
        {
            Type[] argumentsTypes = new Type[argumentsData.Length];
            for (int i = 0, iSize = argumentsData.Length; i < iSize; i++)
            {
                argumentsTypes[i] = SupportedDataTypes.GetDataType(argumentsData[i].DataType).Type;
            }
            return argumentsTypes;
        }

        public object[] GetInvokeParameters()
        {
            ArgumentData[] arguments = ArgumentsData;
            object[] invokeParameters = new object[arguments.Length];
            for (int i = 0, iSize = arguments.Length; i < iSize; i++)
            {
                ArgumentData argumentData = arguments[i];
                invokeParameters[i] = DataTypeSerialization.GetDataTypeSerializer(argumentData.DataType)
                    .Deserialize(argumentData.SerializedValue);
            }

            return invokeParameters;
        }
    }
}