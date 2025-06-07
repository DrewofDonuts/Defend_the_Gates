using System;
using System.Reflection;

namespace DFun.Bookmarks
{
    public static class StaticClassChecker
    {
        public static bool IsStaticClass(Type type)
        {
            return type.IsClass
                   && type.IsAbstract
                   && type.IsSealed;
        }

        public static bool ContainsStaticMember(Type type)
        {
            bool hasAnyStaticMethod = type
                .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Length > 0;
            if (hasAnyStaticMethod) return true;

            bool hasAnyStaticProperty = type
                .GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Length > 0;

            if (hasAnyStaticProperty) return true;

            return false;
        }

        public static bool IsStaticOrContainsStaticMember(Type type)
        {
            return IsStaticClass(type)
                   || ContainsStaticMember(type);
        }
    }
}