using System;
using UnityEngine;

namespace DFun.Bookmarks
{
    [Serializable]
    public class BookmarksGroupGridSize
    {
        [SerializeField] private bool useGroupGridSize = false;
        public bool UseGroupGridSize
        {
            get => useGroupGridSize;
            set => useGroupGridSize = value;
        }

        [SerializeField] private float groupGridSizeNormalized = 0f;
        public float GroupGridSizeNormalized
        {
            get =>  Mathf.Clamp01(groupGridSizeNormalized);
            set => groupGridSizeNormalized =  Mathf.Clamp01(value);
        }

        public BookmarksGroupGridSize()
        {
        }

        public BookmarksGroupGridSize(BookmarksGroupGridSize copyFrom)
        {
            useGroupGridSize = copyFrom.useGroupGridSize;
            GroupGridSizeNormalized = copyFrom.GroupGridSizeNormalized;
        }
    }
}