using System.Collections.Generic;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    public class SceneObjectResolver : IResolver
    {
        private readonly List<IResolver> _resolvers = new List<IResolver>();

        public SceneObjectResolver(params IResolver[] resolvers)
        {
            _resolvers.AddRange(resolvers);
        }

        public SceneObjectResolver AddResolver(IResolver resolver)
        {
            _resolvers.Add(resolver);
            return this;
        }

        public SceneObjectResolver InsertResolver(int index, IResolver resolver)
        {
            _resolvers.Insert(index, resolver);
            return this;
        }

        public SceneObjectResolver RemoveResolver(IResolver resolver)
        {
            _resolvers.Remove(resolver);
            return this;
        }

        public bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            bool resolved = false;
            for (int i = 0, iSize = _resolvers.Count; i < iSize; i++)
            {
                if (_resolvers[i].TryResolveNonAlloc(reference, firstOnly, candidates))
                {
                    resolved = true;
                    if (firstOnly)
                    {
                        break;
                    }
                }
            }

            return resolved;
        }

        public SceneObjectResolver Clone()
        {
            return new SceneObjectResolver(_resolvers.ToArray());
        }
    }
}