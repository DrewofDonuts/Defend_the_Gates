using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarkPreviewIcon
    {
        private Texture _previewIcon;

        private bool _isBookmarkObjectNotFound;
        private Object _requestedIconForAsset;
        private bool IsLoadingIconRequested => _requestedIconForAsset != null;

        public Texture Get(Bookmark bookmark)
        {
            if (_previewIcon != null)
            {
                return _previewIcon;
            }

            if (_isBookmarkObjectNotFound)
            {
                return bookmark.Icon;
            }

            if (IsLoadingIconRequested)
            {
                if (AssetPreview.IsLoadingAssetPreview(_requestedIconForAsset.GetInstanceID()))
                {
                    return bookmark.Icon;
                }

                _previewIcon = AssetPreview.GetAssetPreview(_requestedIconForAsset);
                _requestedIconForAsset = null;

                if (_previewIcon == null)
                {
                    return bookmark.Icon;
                }
            }

            if (bookmark.Resolve(out Object resolvedObject))
            {
                if (!AssetBookmarkHelper.IsAsset(resolvedObject))
                {
                    _previewIcon = bookmark.Icon;
                    return _previewIcon;
                }

                RequestPreviewIcon(resolvedObject);
                return bookmark.Icon;
            }
            else
            {
                _isBookmarkObjectNotFound = true;
            }

            return bookmark.Icon;
        }

        private void RequestPreviewIcon(Object resolvedAsset)
        {
            _requestedIconForAsset = resolvedAsset;
            _previewIcon = AssetPreview.GetAssetPreview(resolvedAsset);
        }
    }
}