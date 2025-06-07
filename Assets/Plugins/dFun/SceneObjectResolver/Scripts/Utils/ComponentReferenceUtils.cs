using System;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    public static class ComponentReferenceUtils
    {
        public static bool TryResolveComponent(
            ComponentReference componentRef, GameObject componentParent, out Component targetComponent)
        {
            //todo help resolve component with GlobalObjectId
            if (componentRef == null || componentParent == null)
            {
                targetComponent = default;
                return false;
            }

            if (componentRef.Component != null)
            {
                targetComponent = componentRef.Component;
                return true;
            }
            
            if (TypeUtils.TryParseClassType(componentRef.ClassType, out Type componentType))
            {
                return TryResolveComponent(componentParent, componentType, componentRef.TypeIndex, out targetComponent);
            }

            targetComponent = default;
            return false;
        }

        public static bool TryResolveComponent(
            GameObject parent, Type componentType, int index, out Component targetComponent)
        {
            if (parent == null || componentType == null)
            {
                targetComponent = default;
                return false;
            }

            Component[] componentsOfType = parent.GetComponents(componentType);
            if (componentsOfType.Length == 0)
            {
                targetComponent = default;
                return false;
            }

            index = Mathf.Clamp(index, 0, componentsOfType.Length - 1);
            targetComponent = componentsOfType[index];
            return true;
        }
    }
}