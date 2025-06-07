using System.Collections.Generic;
using UnityEngine;

namespace DFun.GameObjectResolver
{
    public class ByNameResolver : IResolver
    {
        private List<GameObject> _candidateGameObjects;

        public ByNameResolver()
        {
        }

        public ByNameResolver(List<GameObject> candidateGameObjects)
        {
            _candidateGameObjects = candidateGameObjects;
        }

        public void SetCandidateGameObjects(List<GameObject> candidateGameObjects)
        {
            _candidateGameObjects = candidateGameObjects;
        }

        public bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            bool resolved = false;
            for (int i = 0, iSize = _candidateGameObjects.Count; i < iSize; i++)
            {
                if (_candidateGameObjects[i].name == reference.GameObjectName)
                {
                    resolved = true;
                    if (reference.HasSiblingIndex
                        && reference.SiblingIndex == _candidateGameObjects[i].transform.GetSiblingIndex())
                    {
                        candidates.Insert(0, _candidateGameObjects[i]);
                    }
                    else
                    {
                        candidates.Add(_candidateGameObjects[i]);
                    }

                    if (firstOnly)
                    {
                        break;
                    }
                }
            }

            return resolved;
        }
    }
}