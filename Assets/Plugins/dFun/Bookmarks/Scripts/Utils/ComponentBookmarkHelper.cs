using DFun.GameObjectResolver;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class ComponentBookmarkHelper
    {
        public static bool TryHighlightComponent(
            Object componentParent, ObjectReference objectReference, bool select, out Component resolvedComponent)
        {
            if (objectReference == null)
            {
                resolvedComponent = default;
                return false;
            }

            if (componentParent == null)
            {
                resolvedComponent = default;
                return false;
            }

            if (!ResolveComponentParentGameObject(componentParent, out GameObject componentParentGo))
            {
                resolvedComponent = default;
                return false;
            }

            if (GlobalObjectIdBookmarkHelper.ResolveBookmarkedObject(
                    objectReference.GlobalObjectId, out Object resolvedObj) && resolvedObj is Component targetComponent)
            {
                resolvedComponent = targetComponent;
                if (select)
                {
                    ComponentHighlighter.HighlightComponent(componentParentGo, resolvedComponent);
                }

                return true;
            }

            ComponentReference componentRef = objectReference.ComponentReference;
            if (componentRef == null)
            {
                resolvedComponent = default;
                return false;
            }

            if (ComponentReferenceUtils.TryResolveComponent(componentRef, componentParentGo, out resolvedComponent))
            {
                if (select)
                {
                    ComponentHighlighter.HighlightComponent(componentParentGo, resolvedComponent);
                }

                return true;
            }

            return false;
        }

        private static bool ResolveComponentParentGameObject(Object componentParent, out GameObject componentParentGo)
        {
            if (componentParent is GameObject go)
            {
                componentParentGo = go;
                return true;
            }

            if (componentParent is Component component)
            {
                componentParentGo = component.gameObject;
                return true;
            }

            componentParentGo = default;
            return false;
        }
    }
}