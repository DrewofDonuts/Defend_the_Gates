using System;
using DFun.GameObjectResolver;
using UnityEngine;

namespace DFun.Bookmarks
{
    [Serializable]
    public class BookmarksSettings
    {
        [SerializeField] private string dynamicObjectResolverClass = DefaultSceneObjectResolver.TypeName;
        [SerializeField] private BookmarksViewSettings viewSettings = new BookmarksViewSettings();

        public string DynamicObjectResolverClass
        {
            get => dynamicObjectResolverClass;
            set => dynamicObjectResolverClass = value;
        }

        public BookmarksViewSettings ViewSettings => viewSettings;

        public BookmarksSettings()
        {
            dynamicObjectResolverClass = DefaultSceneObjectResolver.TypeName;
        }

        public BookmarksSettings(BookmarksSettings copyFrom)
        {
            DynamicObjectResolverClass = copyFrom.DynamicObjectResolverClass;
            viewSettings = new BookmarksViewSettings(copyFrom.ViewSettings);
        }
    }
}