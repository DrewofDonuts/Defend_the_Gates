#if UNITY_EDITOR
using System;
using System.Linq;
using Interfaces;
using UnityEditor;
using UnityEngine;

namespace Etheral
{
    public class SetKeys : EditorWindow
    {
        [MenuItem("Tools/Etheral/Set Keys")]
        public static void ShowWindow()
        {
            GetWindow<SetKeys>("Set Unique Keys");
        }

        void OnEnable()
        {
            titleContent = new UnityEngine.GUIContent("Set Unique Keys");
        }

        void OnGUI()
        {
            var monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            var objectsWithIKey = monoBehaviours.OfType<ISetKey>();


            if (GUILayout.Button("Set Keys"))
            {
                foreach (var obj in objectsWithIKey)
                {
                    Debug.Log(obj);
                    obj.SetKey();
                }
            }
        }
    }
}
#endif