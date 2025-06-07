using UnityEngine;

namespace DFun.GameObjectResolver
{
    public class TypeReferenceWrapper : ScriptableObject
    {
        [SerializeField] private TypeReference typeReference;

        public TypeReference TypeReference
        {
            get => typeReference;
            set => typeReference = value;
        }
    }
}