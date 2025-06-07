using System;
using System.Collections.Generic;
using System.Reflection;
using DFun.UnityDataTypes;
using Object = UnityEngine.Object;

namespace DFun.Invoker
{
    public static class InvokeMethodsProvider
    {
        public static List<MethodInfo> GetValidMethods(
            Object component, MethodsTypes allowMethods, bool staticOnly)
        {
            return GetValidMethods(component.GetType(), allowMethods, staticOnly);
        }

        public static List<MethodInfo> GetValidMethods(
            Type type, MethodsTypes allowMethods, bool staticOnly)
        {
            List<MethodInfo> validMethods = new List<MethodInfo>();

            BindingFlags bindingFlags = MethodTypeUtils.GetBindingFlags(allowMethods);

            MethodInfo[] allMethods = type.GetMethods(bindingFlags);
            for (int i = 0, iSize = allMethods.Length; i < iSize; i++)
            {
                MethodInfo methodInfo = allMethods[i];
                if (IsMethodValid(methodInfo, allowMethods, staticOnly, false))
                {
                    validMethods.Add(methodInfo);
                }
            }

            if (allowMethods.IsMethodTypeSelected(MethodsTypes.Properties))
            {
                PropertyInfo[] properties = type.GetProperties(bindingFlags);
                for (int i = 0, iSize = properties.Length; i < iSize; i++)
                {
                    AddPropertyValidMethods(properties[i], allowMethods, staticOnly, validMethods);
                }
            }

            return validMethods;
        }

        private static bool IsMethodValid(
            MethodInfo methodInfo, MethodsTypes allowMethods,
            bool staticOnly, bool allowSpecialName)
        {
            if (methodInfo.IsGenericMethod)
            {
                return false;
            }

            if (!allowSpecialName && methodInfo.IsSpecialName)
            {
                return false;
            }

            if (staticOnly && !methodInfo.IsStatic)
            {
                return false;
            }

            if (!allowMethods.IsMethodTypeSelected(MethodsTypes.NonVoidReturn)
                && methodInfo.ReturnType != typeof(void))
            {
                return false;
            }

            ParameterInfo[] methodArgs = methodInfo.GetParameters();
            if (AreArgsHaveSupportedTypes(methodArgs))
            {
                return true;
            }

            return false;
        }

        private static void AddPropertyValidMethods(
            PropertyInfo propertyInfo, MethodsTypes allowMethods, bool staticOnly,
            List<MethodInfo> result
        )
        {
            if (allowMethods.IsMethodTypeSelected(MethodsTypes.NonVoidReturn))
            {
                bool getMethodAdded = false;

                if (allowMethods.IsMethodTypeSelected(MethodsTypes.Public))
                {
                    if (IsPropertyGetMethodValid(propertyInfo, true, allowMethods, staticOnly,
                            out MethodInfo getMethod))
                    {
                        result.Add(getMethod);
                        getMethodAdded = true;
                    }
                }

                if (!getMethodAdded && allowMethods.IsMethodTypeSelected(MethodsTypes.NonPublic))
                {
                    if (IsPropertyGetMethodValid(propertyInfo, false, allowMethods, staticOnly,
                            out MethodInfo getMethod))
                    {
                        result.Add(getMethod);
                    }
                }
            }

            bool setMethodAdded = false;

            if (allowMethods.IsMethodTypeSelected(MethodsTypes.Public))
            {
                if (IsPropertySetMethodValid(propertyInfo, true, allowMethods, staticOnly, out MethodInfo setMethod))
                {
                    result.Add(setMethod);
                    setMethodAdded = true;
                }
            }

            if (!setMethodAdded && allowMethods.IsMethodTypeSelected(MethodsTypes.NonPublic))
            {
                if (IsPropertySetMethodValid(propertyInfo, false, allowMethods, staticOnly, out MethodInfo setMethod))
                {
                    result.Add(setMethod);
                }
            }
        }

        private static bool IsPropertyGetMethodValid(
            PropertyInfo propertyInfo, bool isPublic, MethodsTypes allowMethods, bool staticOnly,
            out MethodInfo getPropertyMethod)
        {
            getPropertyMethod = propertyInfo.GetGetMethod(!isPublic);
            if (getPropertyMethod == null)
            {
                return false;
            }

            return IsMethodValid(getPropertyMethod, allowMethods, staticOnly, true);
        }

        private static bool IsPropertySetMethodValid(
            PropertyInfo propertyInfo, bool isPublic, MethodsTypes allowMethods, bool staticOnly,
            out MethodInfo setPropertyMethod)
        {
            setPropertyMethod = propertyInfo.GetSetMethod(!isPublic);
            if (setPropertyMethod == null)
            {
                return false;
            }

            return IsMethodValid(setPropertyMethod, allowMethods, staticOnly, true);
        }

        private static bool AreArgsHaveSupportedTypes(ParameterInfo[] methodArgs)
        {
            for (int i = 0, iSize = methodArgs.Length; i < iSize; i++)
            {
                if (!IsArgSupported(methodArgs[i].ParameterType))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsArgSupported(Type targetType)
        {
            if (targetType.IsEnum && Enum.GetValues(targetType).Length > 0) return true;

            DataType[] supportedTypes = SupportedDataTypes.ListOfTypes;
            for (int i = 0, iSize = supportedTypes.Length; i < iSize; i++)
            {
                if (supportedTypes[i].Type == targetType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}