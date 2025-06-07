using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DFun.UnityDataTypes
{
    public static class CustomDataTypeHelper
    {
        private static ICustomDataType[] _customDataTypes;

        public static void Initialize()
        {
            if (_customDataTypes == null)
            {
                _customDataTypes = FindCustomDataTypes();
            }

            for (int i = 0, iSize = _customDataTypes.Length; i < iSize; i++)
            {
                _customDataTypes[i].Initialize();
            }
        }

        private static ICustomDataType[] FindCustomDataTypes()
        {
#if UNITY_EDITOR
            return FindAndCreateInstances<ICustomDataType>();
#else
            return new ICustomDataType[0];
#endif
        }

#if UNITY_EDITOR
        private static T[] FindAndCreateInstances<T>()
        {
            return CreateInstances<T>(
                TypeCache.GetTypesDerivedFrom<T>()
            );
        }

        private static T[] CreateInstances<T>(TypeCache.TypeCollection typeCollection)
        {
            T[] instances = new T[typeCollection.Count];
            for (int i = 0, iSize = instances.Length; i < iSize; i++)
            {
                instances[i] = (T)Activator.CreateInstance(typeCollection[i]);
            }
            return instances;
        }
#endif
    }
}