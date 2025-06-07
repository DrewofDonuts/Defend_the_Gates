using System.Collections.Generic;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    public interface IResolver
    {
        bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates);
    }
}