using System.Reflection;

namespace DFun.Invoker
{
    public static class MethodTypeUtils
    {
        public static bool IsMethodTypeSelected(this MethodsTypes allowMethodsTypes, MethodsTypes compareTo)
        {
            return (allowMethodsTypes & compareTo) != 0;
        }

        public static BindingFlags GetBindingFlags(MethodsTypes allowMethodsTypes)
        {
            allowMethodsTypes = ForceAddVisibilityFlag(allowMethodsTypes);
            return BindingFlags.Instance
                .AppendBindingFlagIfSelected(BindingFlags.Public, MethodsTypes.Public, allowMethodsTypes)
                .AppendBindingFlagIfSelected(BindingFlags.NonPublic, MethodsTypes.NonPublic, allowMethodsTypes)
                .AppendBindingFlagIfSelected(BindingFlags.Static, MethodsTypes.Static, allowMethodsTypes);
        }

        private static MethodsTypes ForceAddVisibilityFlag(MethodsTypes allowMethodsTypes)
        {
            if (allowMethodsTypes.IsMethodTypeSelected(MethodsTypes.Public)) return allowMethodsTypes;
            if (allowMethodsTypes.IsMethodTypeSelected(MethodsTypes.NonPublic)) return allowMethodsTypes;

            return allowMethodsTypes | MethodsTypes.Public;
        }

        private static BindingFlags AppendBindingFlagIfSelected(
            this BindingFlags resultBindingFlags, BindingFlags targetFlag,
            MethodsTypes targetMethodType, MethodsTypes allowedMethodsTypes)
        {
            if (allowedMethodsTypes.IsMethodTypeSelected(targetMethodType))
            {
                resultBindingFlags |= targetFlag;
            }
            return resultBindingFlags;
        }
    }
}