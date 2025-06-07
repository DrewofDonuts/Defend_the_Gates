using UnityEngine;

namespace DFun.GameObjectResolver
{
    public class DontDestroyOnLoadMono : MonoBehaviour
    {
        private static DontDestroyOnLoadMono _instance;
        public static DontDestroyOnLoadMono Instance
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }

                if (_instance != null)
                {
                    return _instance;
                }

                GameObject go = new GameObject("DontDestroyOnLoadMono");
                go.hideFlags = HideFlags.HideAndDontSave;

                DontDestroyOnLoad(go);

                return _instance = go.AddComponent<DontDestroyOnLoadMono>();
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}