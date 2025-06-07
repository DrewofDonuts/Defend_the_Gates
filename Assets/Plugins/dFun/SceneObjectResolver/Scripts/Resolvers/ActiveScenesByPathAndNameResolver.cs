using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DFun.GameObjectResolver
{
    public class ActiveScenesByPathAndNameResolver : IResolver
    {
        private readonly SceneByPathAndNameResolver _sceneByPathAndNameResolver;
        private readonly DontDestroyOnLoadSceneResolver _destroyOnLoadSceneResolver;

        public ActiveScenesByPathAndNameResolver()
        {
            _sceneByPathAndNameResolver = new SceneByPathAndNameResolver(SceneManager.GetActiveScene, 0);
            _destroyOnLoadSceneResolver = new DontDestroyOnLoadSceneResolver(0);
        }

        public bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            int sceneCount = SceneManager.sceneCount;
            bool found = false;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                found |= _sceneByPathAndNameResolver.TryResolveNonAlloc(scene, reference, firstOnly, candidates);
                if (firstOnly && found)
                {
                    break;
                }
            }

            if (!found || !firstOnly)
            {
                found |= _destroyOnLoadSceneResolver.TryResolveNonAlloc(reference, firstOnly, candidates);
            }

            return found;
        }
    }
}