using System;
using UnityEngine;

namespace DFun.Bookmarks
{
    [Serializable]
    public class BookmarksViewSettings
    {
        [SerializeField] private bool drawIconsInListView;
        public bool DrawIconsInListView => drawIconsInListView;

        [SerializeField] private bool showSingleBookmarksTab;
        public bool ShowSingleBookmarksTab
        {
            get => showSingleBookmarksTab;
            set => showSingleBookmarksTab = value;
        }

        public BookmarksViewSettings()
        {
            drawIconsInListView = true;
            showSingleBookmarksTab = true;
        }

        public BookmarksViewSettings(BookmarksViewSettings copyFrom)
        {
            drawIconsInListView = copyFrom == null ? true : copyFrom.drawIconsInListView;
            showSingleBookmarksTab = copyFrom == null ? true : copyFrom.showSingleBookmarksTab;
        }
    }
}