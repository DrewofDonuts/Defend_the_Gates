using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace DFun.GameObjectResolver
{
    public class SceneByPathAndNameResolver : IResolver
    {
        private int _rootDepth;
        private Func<Scene> _sceneProvider;
        private readonly ByNameResolver _byNameResolver = new ByNameResolver();
        private readonly ByParentsResolver _byParentResolver = new ByParentsResolver();

        public SceneByPathAndNameResolver(Func<Scene> sceneProvider, int rootDepth = 0)
        {
            SetSceneProvider(sceneProvider);
            SetRootDepth(rootDepth);
        }

        public void SetSceneProvider(Func<Scene> sceneProvider)
        {
            _sceneProvider = sceneProvider;
        }

        public void SetRootDepth(int rootDepth)
        {
            _rootDepth = rootDepth;
        }

        public virtual bool TryResolveNonAlloc(SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            return TryResolveNonAlloc(_sceneProvider.Invoke(), reference, firstOnly, candidates);
        }

        public virtual bool TryResolveNonAlloc(
            Scene scene, SceneObjectReference reference, bool firstOnly, List<GameObject> candidates)
        {
            if (!reference.IsOnScene(scene))
            {
                return false;
            }

            return TryResolve(reference, scene, firstOnly, candidates);
        }

        private bool TryResolve(
            SceneObjectReference reference, Scene scene, bool firstOnly, List<GameObject> candidates)
        {
            using (ListPool<GameObject>.Get(out List<GameObject> tmpRootGameObjects))
            using (ListPool<Transform>.Get(out List<Transform> matchedParents))
            {
                scene.GetRootObjectsNonAlloc(tmpRootGameObjects, _rootDepth);

                if (!reference.HasGameObjectScenePath)
                {
                    _byNameResolver.SetCandidateGameObjects(tmpRootGameObjects);
                    return _byNameResolver.TryResolveNonAlloc(reference, firstOnly, candidates);
                }

                SceneObjectResolverUtils.GetMatchedParents(
                    tmpRootGameObjects, reference.GameObjectScenePath, matchedParents
                );

                _byParentResolver.SetParents(matchedParents);
                return _byParentResolver.TryResolveNonAlloc(reference, firstOnly, candidates);
            }
        }
    }
}