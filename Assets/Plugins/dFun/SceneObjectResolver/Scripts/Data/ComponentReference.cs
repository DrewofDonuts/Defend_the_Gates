using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace DFun.GameObjectResolver
{
    [Serializable]
    public class ComponentReference
    {
        [SerializeField] private Component component;
        [SerializeField] private string classType;
        [SerializeField] private int typeIndex;

        public Component Component
        {
            get => component;
            set => component = value;
        }

        public string ClassType
        {
            get => classType;
            set => classType = value;
        }

        public int TypeIndex
        {
            get => typeIndex;
            set => typeIndex = value;
        }

        public string ReferenceName
        {
            get
            {
                if (Component != null)
                {
                    return " (" + Component.GetType().Name + ")";
                }

                if (string.IsNullOrEmpty(classType))
                {
                    return string.Empty;
                }

                string[] split = classType.Split('.');
                return split.Length > 0 ? " (" + split[split.Length - 1] + ")" : string.Empty;
            }
        }

        public bool ContainsData => classType != null;

        public ComponentReference()
        {
        }

        public ComponentReference(Component c, int index)
        {
            Assert.IsNotNull(c);
            Component = c;
            ClassType = c.GetType().AssemblyQualifiedName;
            TypeIndex = index;
        }

        public ComponentReference(ComponentReference copyFrom)
        {
            Component = copyFrom.Component;
            ClassType = copyFrom.ClassType;
            TypeIndex = copyFrom.TypeIndex;
        }
    }
}