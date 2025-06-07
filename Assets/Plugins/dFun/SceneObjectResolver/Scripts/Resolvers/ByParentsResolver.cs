using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DFun.GameObjectResolver
{
    public class ByParentsResolver : IResolver
    {
        private List<Transform> _parents;
        private readonly ByNameResolver _byNameResolver = new ByNameResolver();

        public ByParentsResolver()
        {
            _parents = new List<Transform>(0);
        }

        public ByParentsResolver(List<Transform> parents)
        {
            _parents = parents;
        }

        public void SetParents(List<Transform> parents)
        {
            _parents = parents;
        }

        public bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            bool resolved = false;
            using (ListPool<GameObject>.Get(out List<GameObject> tmpChildren))
            {
                for (int i = 0, iSize = _parents.Count; i < iSize; i++)
                {
                    _parents[i].GetChildrenNonAlloc(tmpChildren);
                    _byNameResolver.SetCandidateGameObjects(tmpChildren);

                    if (_byNameResolver.TryResolveNonAlloc(reference, firstOnly, candidates))
                    {
                        resolved = true;
                        if (firstOnly)
                        {
                            break;
                        }
                    }
                }
            }

            return resolved;
        }
    }
}