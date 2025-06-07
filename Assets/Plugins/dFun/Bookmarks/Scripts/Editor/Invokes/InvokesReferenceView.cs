using DFun.GameObjectResolver;
using DFun.Invoker;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    /// List presentation of InvokesReference
    public class InvokesReferenceView
    {
        private readonly Bookmark _parentBookmark;

        private SerializedObject _serializedInvokesListWrapper;
        private SerializedObject SerializedInvokesListWrapper
        {
            get
            {
                if (_serializedInvokesListWrapper == null
                    || _serializedInvokesListWrapper.targetObject == null)
                {
                    InvokesListWrapper invokesListWrapper = ScriptableObject.CreateInstance<InvokesListWrapper>();
                    InvokesList invokesList = _parentBookmark.ObjectReference.InvokesReference.InvokesList;
                    invokesListWrapper.InvokesList = invokesList;
                    _serializedInvokesListWrapper = new SerializedObject(
                        invokesListWrapper,
                        GetWrapperContext(_parentBookmark)
                    );
                }
                return _serializedInvokesListWrapper;
            }
        }

        private SerializedProperty _serializedInvokesList;
        private SerializedProperty SerializedInvokesList
        {
            get
            {
                if (_serializedInvokesList == null)
                {
                    _serializedInvokesList = SerializedInvokesListWrapper.FindProperty("invokesList");
                }
                return _serializedInvokesList;
            }
        }

        private InvokesListPropertyDrawer _invokesListPropertyDrawer;
        private InvokesListPropertyDrawer InvokesListPropertyDrawer
        {
            get
            {
                if (_invokesListPropertyDrawer == null)
                {
                    _invokesListPropertyDrawer = new InvokesListPropertyDrawer();
                    _invokesListPropertyDrawer.CustomHeader = "Invokes";
                }
                return _invokesListPropertyDrawer;
            }
        }

        private Vector2 _scrollPos;

        public InvokesReferenceView(Bookmark bookmark)
        {
            _parentBookmark = bookmark;
        }

        private Object GetWrapperContext(Bookmark bookmark)
        {
            if (bookmark.Resolve(out Object contextObject))
            {
                return contextObject;
            }

            TypeReference typeReference = bookmark.ObjectReference.TypeReference;
            if (typeReference.ContainsData)
            {
                TypeReferenceWrapper typeReferenceWrapper = ScriptableObject.CreateInstance<TypeReferenceWrapper>();
                typeReferenceWrapper.TypeReference = typeReference;
                return typeReferenceWrapper;
            }

            return null;
        }

        public bool Draw(Rect rect)
        {
            bool dirty = false;
            GUILayout.BeginArea(rect);
            {
                SerializedObject sInvokesListWrapper = SerializedInvokesListWrapper;
                sInvokesListWrapper.Update();
                EditorGUI.BeginChangeCheck();
                {
                    _scrollPos = EditorGUILayout.BeginScrollView(
                        _scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar
                    );
                    {
                        InvokesListPropertyDrawer.DrawInLayout(SerializedInvokesList);
                    }
                    EditorGUILayout.EndScrollView();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    sInvokesListWrapper.ApplyModifiedProperties();
                    dirty = true;
                }
            }
            GUILayout.EndArea();
            return dirty;
        }
    }
}