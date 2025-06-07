using System;
using DFun.GameObjectResolver;
using UnityEditor;

namespace DFun.Bookmarks
{
    public static class DynamicObjectResolverTypeHelper
    {
        public static TypeCache.TypeCollection AvailableResolvers =>
            TypeCache.GetTypesDerivedFrom<SceneObjectResolver>();

        private static string[] _namesCache = Array.Empty<string>();

        public static bool FindIndexOfResolver(
            string targetResolverClass, TypeCache.TypeCollection resolvers, out int targetIndex)
        {
            for (int i = 0, iSize = resolvers.Count; i < iSize; i++)
            {
                if (GetTypeSerializableName(resolvers[i]) == targetResolverClass)
                {
                    targetIndex = i;
                    return true;
                }
            }

            targetIndex = -1;
            return false;
        }

        public static string[] GetNames(TypeCache.TypeCollection availableResolvers)
        {
            if (_namesCache == null || _namesCache.Length != availableResolvers.Count)
            {
                _namesCache = new string[availableResolvers.Count];
            }

            for (int i = 0, iSize = availableResolvers.Count; i < iSize; i++)
            {
                _namesCache[i] = GetTypeName(availableResolvers[i]);
            }

            return _namesCache;
        }

        public static string GetTypeName(Type type)
        {
            return type.Name;
        }

        public static string GetTypeSerializableName(Type type)
        {
            return type.AssemblyQualifiedName;
        }
    }
}