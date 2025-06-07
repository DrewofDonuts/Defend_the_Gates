using System;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    [Serializable]
    public class TypeReference
    {
        [SerializeField] private string typeName = string.Empty;
        [SerializeField] private string typeAssemblyQualifiedName = string.Empty;

        public string TypeName
        {
            get => typeName;
            set => typeName = value;
        }

        public string TypeAssemblyQualifiedName
        {
            get => typeAssemblyQualifiedName;
            set => typeAssemblyQualifiedName = value;
        }

        public bool ContainsData => !string.IsNullOrEmpty(typeAssemblyQualifiedName);

        public TypeReference()
        {
        }

        public TypeReference(TypeReference copyFrom)
        {
            typeName = copyFrom.TypeName;
            typeAssemblyQualifiedName = copyFrom.TypeAssemblyQualifiedName;
        }

        public bool TryToResolveType(out Type type)
        {
            return TypeUtils.TryParseClassType(TypeAssemblyQualifiedName, out type);
        }
    }
}