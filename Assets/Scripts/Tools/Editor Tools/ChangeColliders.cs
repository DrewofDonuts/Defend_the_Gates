using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
#if UNITY_EDITOR
    public class ChangeColliders : EditorWindow
    {
        public GameObject[] objects;
        public Function function;

        public ColliderTypes colliderTypes;
        public bool setToIsTrigger;
        public LayerMask ignoreLayers;
        public Renderer childRenderer;

        ColliderConfiguration colliderConfiguration;

        [MenuItem("Tools/Etheral/Change Colliders")]
        public static void ShowWindow()
        {
            GetWindow<ChangeColliders>("Change Colliders");
        }

        void OnEnable()
        {
            colliderConfiguration = new ColliderConfiguration();
        }

        void OnGUI()
        {
            var guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.wordWrap = true;
            guiStyle.normal.textColor = Color.white;

            GUILayout.Label("Instructions \n 1. Add Colliders to array. " +
                            "\n2. Set which action to take" +
                            "\n3. Set additional configurations" +
                            "\n4. Click Change Colliders",
                guiStyle);

            SerializedObject serializedObject = new SerializedObject(this);
            serializedObject.Update();

            SerializedProperty property3 = serializedObject.FindProperty("function");
            EditorGUILayout.PropertyField(property3, true);

            if (function == Function.SetTrigger)
            {
                SerializedProperty property2 = serializedObject.FindProperty("setToIsTrigger");
                EditorGUILayout.PropertyField(property2, true);
            }


            if (function == Function.AddColliders)
            {
                SerializedProperty property4 = serializedObject.FindProperty("colliderTypes");
                EditorGUILayout.PropertyField(property4, true);
            }

            if (function == Function.SizeCollidersFromChildren)
            {
                SerializedProperty property5 = serializedObject.FindProperty("colliderTypes");
                SerializedProperty property6 = serializedObject.FindProperty("childRenderer");

                EditorGUILayout.PropertyField(property5, true);
                EditorGUILayout.PropertyField(property6, true);
            }

            if (function == Function.IgnoreLayers)
            {
                SerializedProperty property7 = serializedObject.FindProperty("ignoreLayers");
                EditorGUILayout.PropertyField(property7, true);
            }


            SerializedProperty property = serializedObject.FindProperty("objects");
            EditorGUILayout.PropertyField(property, true);


            serializedObject.ApplyModifiedProperties();


            if (GUILayout.Button("Change Colliders"))
                ChangeSelectedColliders();


            if (objects != null)
            {
                if (GUILayout.Button("Clear Objects"))
                    ClearObjects();
            }
        }

        void ChangeSelectedColliders()
        {
            if (function == Function.RemoveColliders)
            {
                if (EditorUtility.DisplayDialog("Remove Colliders", "Are you sure you want to remove colliders?", "Yes",
                        "No"))
                    RemoveColliders();
                else
                    return;
            }

            if (function == Function.DisableColliders)
            {
                if (EditorUtility.DisplayDialog("Disable Colliders", "Are you sure you want to disable colliders?",
                        "Yes",
                        "No"))
                    DisableColliders();
                else
                    return;
            }

            if (function == Function.EnableColliders)
            {
                if (EditorUtility.DisplayDialog("Enable Colliders", "Are you sure you want to enable colliders?", "Yes",
                        "No"))
                    EnableColliders();
                else
                    return;
            }

            if (function == Function.AddColliders)
            {
                if (EditorUtility.DisplayDialog("Add Colliders",
                        "Are you sure you want to clear original colliders and add colliders?", "Yes",
                        "No"))
                    AddColliders();
                else
                    return;
            }


            if (function == Function.SetTrigger)
            {
                if (EditorUtility.DisplayDialog("Change Colliders", "Are you sure you want to change colliders to " +
                                                                    "IsTrigger: " + setToIsTrigger + "?", "Yes",
                        "No"))
                    SetTrigger();
                else
                    return;
            }

            if (function == Function.SizeCollidersFromChildren)
            {
                if (EditorUtility.DisplayDialog("Size Colliders From Children",
                        "Are you sure you want to size colliders from children?", "Yes",
                        "No"))
                    SizeColliderFromChildren();
                else
                    return;
            }

            if (function == Function.IgnoreLayers)
            {
                if (EditorUtility.DisplayDialog("Ignore Layers",
                        "Are you sure you want to ignore layers?", "Yes",
                        "No"))
                    SetIgnoreLayers();
                else
                    return;
            }
        }

        void AddColliders()
        {
            if (objects == null) return;
            if (colliderTypes == ColliderTypes.BoxCollider)
                foreach (var _object in objects)
                {
                    _object.AddComponent<BoxCollider>();
                    colliderConfiguration.CreateBoxCollider(_object);
                }

            if (colliderTypes == ColliderTypes.SphereCollider)
                foreach (var _object in objects)
                {
                    _object.AddComponent<SphereCollider>();
                    colliderConfiguration.CreateSphereCollider(_object);
                }

            if (colliderTypes == ColliderTypes.CapsuleCollider)
                foreach (var _object in objects)
                {
                    _object.AddComponent<CapsuleCollider>();
                    colliderConfiguration.CreateCapsuleCollider(_object);
                }

            if (colliderTypes == ColliderTypes.MeshCollider)
                foreach (var _object in objects)
                {
                    _object.AddComponent<MeshCollider>();
                }

            ClearObjects();
        }

        void DisableColliders()
        {
            if (objects == null) return;
            foreach (var _object in objects)
            {
                Collider[] colliders = _object.GetComponents<Collider>();
                Collider[] childColliders = _object.GetComponentsInChildren<Collider>();
                colliders = colliders.Concat(childColliders).ToArray();

                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
            }

            ClearObjects();
        }

        void EnableColliders()
        {
            if (objects == null) return;
            foreach (var _object in objects)
            {
                Collider[] colliders = _object.GetComponents<Collider>();
                Collider[] childColliders = _object.GetComponentsInChildren<Collider>();
                colliders = colliders.Concat(childColliders).ToArray();

                foreach (var collider in colliders)
                {
                    collider.enabled = true;
                }
            }

            ClearObjects();
        }


        void RemoveColliders()
        {
            if (objects == null) return;
            foreach (var _object in objects)
            {
                Collider[] colliders = _object.GetComponents<Collider>();
                Collider[] childColliders = _object.GetComponentsInChildren<Collider>();
                colliders = colliders.Concat(childColliders).ToArray();

                foreach (var collider in colliders)
                {
                    DestroyImmediate(collider);
                }
            }

            ClearObjects();
        }

        void SetIgnoreLayers()
        {
            if (objects == null) return;

            foreach (var _object in objects)
            {
                Collider[] colliders = _object.GetComponents<Collider>();
                Collider[] childColliders = _object.GetComponentsInChildren<Collider>();
                colliders = colliders.Concat(childColliders).ToArray();

                foreach (var collider in colliders)
                {
                    collider.excludeLayers = ignoreLayers;
                }
            }

            ClearObjects();
        }

        void SetTrigger()
        {
            if (objects == null) return;

            foreach (var _object in objects)
            {
                Collider[] colliders = _object.GetComponents<Collider>();
                Collider[] childColliders = _object.GetComponentsInChildren<Collider>();
                colliders = colliders.Concat(childColliders).ToArray();

                foreach (var collider in colliders)
                {
                    collider.isTrigger = setToIsTrigger;
                }
            }

            ClearObjects();
        }

        void ClearObjects()
        {
            objects = null;
        }

        void SizeColliderFromChildren()
        {
            foreach (var _object in objects)
            {
                var currentCollider = _object.GetComponent<Collider>();
                if (currentCollider != null)
                    DestroyImmediate(currentCollider);

                var originalScale = _object.transform.localScale;


                _object.transform.localScale = Vector3.one;

                ColliderConfiguration colliderConfiguration = new ColliderConfiguration();

                var retrievedChildRenderer = childRenderer.GetComponentInChildren<Renderer>();

                switch (colliderTypes)
                {
                    case ColliderTypes.BoxCollider:
                        currentCollider =
                            colliderConfiguration.CreateBoxCollider(_object.gameObject, retrievedChildRenderer);
                        break;
                    case ColliderTypes.SphereCollider:
                        currentCollider =
                            colliderConfiguration.CreateSphereCollider(_object.gameObject, retrievedChildRenderer);
                        break;
                    case ColliderTypes.CapsuleCollider:
                        currentCollider =
                            colliderConfiguration.CreateCapsuleCollider(_object.gameObject, retrievedChildRenderer);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _object.transform.localScale = originalScale;
            }
        }

        public enum Function
        {
            AddColliders,
            DisableColliders,
            EnableColliders,
            IgnoreLayers,
            RemoveColliders,
            SetTrigger,
            SizeCollidersFromChildren
        }
    }
#endif
}