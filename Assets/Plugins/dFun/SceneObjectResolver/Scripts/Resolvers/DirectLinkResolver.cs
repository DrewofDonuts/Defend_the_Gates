using System.Collections.Generic;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    public class DirectLinkResolver : IResolver
    {
        public bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            if (reference.GameObject != null)
            {
                candidates.Add(reference.GameObject);
                return true;
            }

            return false;
        }
    }
}