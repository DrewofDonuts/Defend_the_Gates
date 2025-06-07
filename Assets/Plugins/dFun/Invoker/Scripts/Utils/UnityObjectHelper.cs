using System.Collections.Generic;
using UnityEngine;

namespace DFun.Invoker
{
    public static class UnityObjectHelper
    {
        public static bool TryToGetGameObject(Object obj, out GameObject gameObject)
        {
            if (obj == null)
            {
                gameObject = default;
                return false;
            }

            if (obj is GameObject)
            {
                gameObject = (GameObject)obj;
                return true;
            }

            if (obj is Component)
            {
                gameObject = ((Component)obj).gameObject;
                return true;
            }

            gameObject = default;
            return false;
        }

        public static List<Object> GetObjectComponents(Object obj)
        {
            if (TryToGetGameObject(obj, out GameObject gameObject))
            {
                List<Object> allComponents = new List<Object> { gameObject };
                allComponents.AddRange(gameObject.GetComponents<Component>());
                return allComponents;
            }

            return new List<Object>(0);
        }
    }
}