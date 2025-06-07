using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DFun.UnityDataTypes;

namespace DFun.Invoker
{
    public static class InvokeFormatter
    {
        private const string PropertySetterMethodPrefix = "set_";

        public static List<MethodInfo> SortByName(List<MethodInfo> methods)
        {
            methods.Sort((m1, m2) =>
            {
                string m1Name = m1.Name;
                string m2Name = m2.Name;

                bool isMethod1Setter = m1Name.StartsWith(PropertySetterMethodPrefix);
                bool isMethod2Setter = m2Name.StartsWith(PropertySetterMethodPrefix);

                if (isMethod1Setter && isMethod2Setter)
                {
                    return String.Compare(m1Name, m2Name, StringComparison.Ordinal);
                }

                if (isMethod1Setter) return -1;
                if (isMethod2Setter) return 1;

                return String.Compare(m1Name, m2Name, StringComparison.Ordinal);
            });
            return methods;
        }

        public static string GetFormattedMethodName(
            MethodInfo methodInfo, string componentTypeName, int componentIndex)
        {
            StringBuilder args = new StringBuilder();
            int paramsAmount = methodInfo.GetParameters().Length;
            for (int index = 0; index < paramsAmount; index++)
            {
                ParameterInfo arg = methodInfo.GetParameters()[index];
                args.Append(string.Format("{0}", GetTypeName(arg.ParameterType)));

                if (index < paramsAmount - 1)
                {
                    args.Append(", ");
                }
            }

            return GetFormattedMethodName(
                componentTypeName, componentIndex, methodInfo.Name, args.ToString()
            );
        }

        public static string GetFormattedMethodName(
            string componentTypeName, int componentIndex, string methodName, string args)
        {
            string componentIndexPrefix = componentIndex == 0 ? string.Empty : $" ({componentIndex})";
            if (methodName.StartsWith("set_"))
            {
                return string.Format(
                    "{0}{3}/{2} {1}", componentTypeName, methodName.Substring(4), args, componentIndexPrefix
                );
            }
            else
            if (methodName.StartsWith("get_"))
            {
                return string.Format(
                    "{0}{3}/{1} [Get]", componentTypeName, methodName.Substring(4), args, componentIndexPrefix
                );
            }
            else
            {
                return string.Format(
                    "{0}{3}/{1} ({2})", componentTypeName, methodName, args, componentIndexPrefix
                );
            }
        }

        private static string GetTypeName(Type t)
        {
            return SupportedDataTypes.GetDataType(t).Name;
        }
    }
}