using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DFun.GameObjectResolver
{
    public class DontDestroyOnLoadSceneResolver : IResolver
    {
        private const string DontDestroyOnLoadSceneName = "DontDestroyOnLoad";

        private readonly SceneByPathAndNameResolver _sceneByPathAndNameResolver;

        public DontDestroyOnLoadSceneResolver(int rootDepth = 0)
        {
            _sceneByPathAndNameResolver = new SceneByPathAndNameResolver(SceneManager.GetActiveScene, rootDepth);
        }

        public bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            if (reference.ParentScenePath != DontDestroyOnLoadSceneName)
            {
                return false;
            }

            if (TryToGetDontDestroyScene(out Scene dontDestroyScene))
            {
                return _sceneByPathAndNameResolver.TryResolveNonAlloc(
                    dontDestroyScene, reference, firstOnly, candidates
                );
            }
            return false;
        }

        private bool TryToGetDontDestroyScene(out Scene dontDestroyScene)
        {
            if (!Application.isPlaying)
            {
                dontDestroyScene = default;
                return false;
            }

            DontDestroyOnLoadMono dontDestroyObj = DontDestroyOnLoadMono.Instance;
            if (dontDestroyObj == null)
            {
                dontDestroyScene = default;
                return false;
            }

            dontDestroyScene = dontDestroyObj.gameObject.scene;
            return dontDestroyScene.path == DontDestroyOnLoadSceneName;
        }
    }
}