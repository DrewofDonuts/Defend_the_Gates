using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace DFun.Invoker
{
    public class MethodDrawData
    {
        public readonly Object InvokeObject;
        public readonly string InvokeObjectAssemblyQualifiedName;
        /// Index in the same type components array on this game object.
        /// E.g. if game object contains 2 Audio Source components, first one's index is 0
        /// and second one's index is 1.
        public readonly int ComponentIndex;
        public readonly MethodInfo MethodInfo;
        public readonly string DropdownOption;
        public readonly string DisplayName;

        public readonly string InvokeObjectName;
        public string MethodName => MethodInfo.Name;

        private const string UnknownName = "Unknown";

        public MethodDrawData(Object invokeObject, int componentIndex, MethodInfo methodInfo)
            : this(invokeObject, invokeObject.GetType(), componentIndex, methodInfo)
        {
        }

        public MethodDrawData(Type type, int componentIndex, MethodInfo methodInfo)
            : this(null, type, componentIndex, methodInfo)
        {
        }

        private MethodDrawData(Object invokeObject, Type type, int componentIndex, MethodInfo methodInfo)
        {
            InvokeObject = invokeObject;
            InvokeObjectAssemblyQualifiedName = type.AssemblyQualifiedName;
            InvokeObjectName = invokeObject != null
                ? GetInvokeObjectName(invokeObject, methodInfo.IsStatic)
                : GetInvokeTypeName(type);

            ComponentIndex = componentIndex;
            MethodInfo = methodInfo;
            DropdownOption = InvokeFormatter.GetFormattedMethodName(
                methodInfo, InvokeObjectName, componentIndex
            );
            DisplayName = DropdownOption.Replace("/", ": ");
        }

        private static string GetInvokeObjectName(Object invokeObject, bool isMethodStatic)
        {
            if (invokeObject == null) return UnknownName;

            if (!isMethodStatic) return invokeObject.GetType().Name;

            if (!(invokeObject is MonoScript monoScript)) return invokeObject.GetType().Name;

            Type targetClassType = monoScript.GetClass();
            return GetInvokeTypeName(targetClassType);
        }

        private static string GetInvokeTypeName(Type targetClassType)
        {
            if (targetClassType == null) return UnknownName;
            return targetClassType.Name;
        }
    }
}