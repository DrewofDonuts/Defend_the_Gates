using System;
using DFun.GameObjectResolver;
using DFun.UnityDataTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    [Serializable]
    public class Bookmark
    {
        [SerializeField] private ObjectReference objectReference;
        [SerializeField] private string bookmarkName;
        [SerializeField] private long creationTime; // UTC timestamp
        [SerializeField] private int customSortIndex;
        [SerializeField] private Texture icon;

        public ObjectReference ObjectReference
        {
            get => objectReference;
            private set => objectReference = value;
        }

        public string BookmarkName
        {
            get => bookmarkName;
            set => bookmarkName = value;
        }

        public long CreationTime
        {
            get => creationTime;
            set => creationTime = value;
        }

        public int CustomSortIndex
        {
            get => customSortIndex;
            set => customSortIndex = value;
        }

        public Texture Icon
        {
            get
            {
                if (icon == null)
                {
                    return Icons.WarningIcon?.image;
                }
                return icon;
            }
            private set
            {
                if (icon != value)
                {
                    icon = value;
                    BookmarksStorage.Get().Dirty = true;
                    BookmarksStorage.Save();
                }
            }
        }

        private BookmarkPreviewIcon _previewIcon;
        public Texture PreviewIcon
        {
            get
            {
                if (_previewIcon == null) _previewIcon = new BookmarkPreviewIcon();
                return _previewIcon.Get(this);
            }
        }

        public bool ContainsData => !GlobalObjectIdBookmarkHelper.IsNone(ObjectReference.GlobalObjectId)
                                    || ObjectReference.SceneObjectReference.ContainsData
                                    || ObjectReference.AssetReference.ContainsData
                                    || ObjectReference.ComponentReference.ContainsData
                                    || ObjectReference.InvokesReference.ContainsData
                                    || ObjectReference.TypeReference.ContainsData;

        public Bookmark(Object o)
        {
            Assert.IsNotNull(o, "Bookmark object can't be null");

            ObjectReference = new ObjectReference
            {
                GlobalObjectId = GlobalObjectIdBookmarkHelper.GetObjectId(o).ToString(),
                SceneObjectReference = CreateSceneObjectReference(o),
                AssetReference = CreateAssetReference(o),
                ComponentReference = CreateComponentReference(o),
                ContextAssetReference = CreateContextAssetReference(o),
                InvokesReference = new InvokesReference(),
                TypeReference = new TypeReference()
            };

            BookmarkName = ObjectReference.Name;
            CreationTime = TimeUtils.TimestampUtc;

            icon = GetIcon(o);
        }

        public Bookmark(Type type)
        {
            Assert.IsNotNull(type, "Bookmark type can't be null");

            ObjectReference = new ObjectReference();
            TypeReference typeReference = ObjectReference.TypeReference;
            typeReference.TypeName = type.Name;
            typeReference.TypeAssemblyQualifiedName = type.AssemblyQualifiedName;

            BookmarkName = ObjectReference.Name;
            CreationTime = TimeUtils.TimestampUtc;

            GUIContent scriptIcon = Icons.ScriptIcon;
            if (scriptIcon != null)
            {
                icon = scriptIcon.image;
            }
        }

        public Bookmark(Bookmark copyFrom)
        {
            ObjectReference = copyFrom.ObjectReference != null
                ? new ObjectReference(copyFrom.ObjectReference)
                : null;
            BookmarkName = copyFrom.BookmarkName;
            CreationTime = copyFrom.CreationTime;
            CustomSortIndex = copyFrom.customSortIndex;
            icon = copyFrom.icon;
        }

        private AssetReference CreateAssetReference(Object o)
        {
            return AssetBookmarkHelper.IsAsset(o)
                ? new AssetReference
                {
                    AssetObject = o,
                    AssetGuid = AssetBookmarkHelper.TryGetAssetGuid(o)
                }
                : new AssetReference();
        }

        private AssetReference CreateContextAssetReference(Object o)
        {
            if (PrefabBookmarkHelper.TryToGetContextPrefab(o, out string contextPrefabAssetPath))
            {
                Object contextPrefab = AssetDatabase.LoadAssetAtPath<Object>(contextPrefabAssetPath);
                return CreateAssetReference(contextPrefab);
            }

            if (AssetBookmarkHelper.TryToGetParentScene(o, out Object parentScene))
            {
                return CreateAssetReference(parentScene);
            }

            return new AssetReference();
        }

        private SceneObjectReference CreateSceneObjectReference(Object o)
        {
            if (o is Component component)
            {
                return component.gameObject.CreateSceneObjectReference(true);
            }

            if (o is GameObject go)
            {
                return go.CreateSceneObjectReference(true);
            }

            return new SceneObjectReference();
        }

        private ComponentReference CreateComponentReference(Object o)
        {
            if (o is Component component)
            {
                Component[] sameTypeComponents = component.GetComponents(component.GetType());
                return new ComponentReference(
                    component,
                    Array.IndexOf(sameTypeComponents, component)
                );
            }

            return new ComponentReference();
        }

        public bool Open(SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            bool openSuccess = Open(true, out Object resolvedObject, true, selectionFlags);
            DoInvokes(openSuccess, resolvedObject);
            return openSuccess;
        }

        public bool Resolve(out Object resolvedObject)
        {
            return Open(false, out resolvedObject, false);
        }

        public void DoInvokes()
        {
            bool resolved = Resolve(out Object resolvedObject);
            DoInvokes(resolved, resolvedObject);
        }

        public void DoInvokes(bool objectResolved, Object resolvedObject)
        {
            if (objectResolved)
            {
                InvokesHelper.DoInvokes(this, resolvedObject);
            }
            else
            {
                InvokesHelper.DoInvokes(
                    ObjectReference.TypeReference,
                    ObjectReference.InvokesReference
                );
            }
        }

        private bool Open(
            bool select, out Object resolvedObject, bool allowLogs = true,
            SelectionFlags selectionFlags = SelectionFlags.PingAndActive)
        {
            if (GlobalObjectIdBookmarkHelper.OpenBookmark(this, select, out Object bookmarkedObject, selectionFlags))
            {
                bool isComponent = ComponentBookmarkHelper.TryHighlightComponent(
                    bookmarkedObject, ObjectReference, select, out Component resolvedComponent
                );
                UpdateIcon(isComponent ? resolvedComponent : bookmarkedObject);
                resolvedObject = isComponent ? resolvedComponent : bookmarkedObject;
                return true;
            }

            if (AssetBookmarkHelper.IsAsset(this))
            {
                AssetBookmarkHelper.OpenBookmark(this, select, out Object resolvedAsset, selectionFlags);
                UpdateIcon(resolvedAsset);
                resolvedObject = resolvedAsset;
                return true;
            }

            if (PrefabBookmarkHelper.OpenBookmark(this, select, out Object resolvedPrefabObject))
            {
                bool isComponent = ComponentBookmarkHelper.TryHighlightComponent(
                    resolvedPrefabObject, ObjectReference, select, out Component resolvedComponent
                );
                UpdateIcon(isComponent ? resolvedComponent : resolvedPrefabObject);
                resolvedObject = isComponent ? resolvedComponent : resolvedPrefabObject;
                return true;
            }

            if (SceneBookmarkHelper.OpenBookmark(this, select, out Object resolvedSceneObject))
            {
                bool isComponent = ComponentBookmarkHelper.TryHighlightComponent(
                    resolvedSceneObject, ObjectReference, select, out Component resolvedComponent
                );
                UpdateIcon(isComponent ? resolvedComponent : resolvedSceneObject);
                resolvedObject = isComponent ? resolvedComponent : resolvedSceneObject;
                return true;
            }

            if (ObjectReference?.TypeReference?.ContainsData == true)
            {
                resolvedObject = default;
                return false;
            }

            else
            {
                if (select)
                {
                    TryToSelectContextAsset();
                }

                if (allowLogs)
                {
                    Debug.LogFormat("Bookmark not found: {0}", BookmarkName);
                }
            }

            resolvedObject = default;
            return false;
        }

        private void TryToSelectContextAsset()
        {
            if (AssetBookmarkHelper.TryToSelectContextPrefab(this))
            {
                return;
            }

            SceneBookmarkHelper.TrySelectScene(this);
        }

        private void UpdateIcon(Object o)
        {
            Icon = GetIcon(o);
        }

        private Texture GetIcon(Object o)
        {
            return EditorGUIUtility.ObjectContent(o, o.GetType())?.image;
        }
    }
}