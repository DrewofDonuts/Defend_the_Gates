using System;
using System.Collections.Generic;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksClipboard
    {
        public static bool HasData
        {
            get
            {
                string systemCopyBuffer = GUIUtility.systemCopyBuffer;
                if (string.IsNullOrEmpty(systemCopyBuffer)) return false;

                int hashCode = systemCopyBuffer.GetHashCode();
                if (hashCode != _lastParsedBufferHashCode)
                {
                    TryToParseData(systemCopyBuffer, hashCode);
                }

                return DataInternal != null;
            }
        }

        private static Bookmarks DataInternal { get; set; }

        public static Bookmarks Data => DataInternal != null
            ? Bookmarks.CreateInstance().DeepCopy(DataInternal, false)
            : null;

        private static int _lastParsedBufferHashCode = 0;

        private static void TryToParseData(string systemCopyBuffer, int hashCode)
        {
            _lastParsedBufferHashCode = hashCode;

            try
            {
                DataInternal = JsonUtility.FromJson<Bookmarks>(systemCopyBuffer);
            }
            catch (Exception)
            {
                if (BookmarksClipboardCompressionHelper.TryToDecompress(
                        systemCopyBuffer, out Bookmarks decompressedBookmarks))
                {
                    DataInternal = decompressedBookmarks;
                }
                else
                {
                    DataInternal = null;
                }
            }
        }

        public static void CopyToClipboard(Bookmark bookmark, bool compress)
        {
            CopyToSystemBuffer(
                BookmarksWrapHelper.WrapBookmarks(bookmark), compress
            );
        }

        public static void CopyToClipboard(List<Bookmark> bookmarks, bool compress)
        {
            CopyToSystemBuffer(
                BookmarksWrapHelper.WrapBookmarks(bookmarks), compress
            );
        }

        public static void CopyToClipboard(BookmarksGroup group, bool compress)
        {
            CopyToSystemBuffer(
                BookmarksWrapHelper.WrapBookmarks(group), compress
            );
        }

        public static void CopyToClipboard(Bookmarks bookmarks, bool compress)
        {
            CopyToSystemBuffer(bookmarks, compress);
        }

        public static string BuildGroupsCountText(int groupsCount)
        {
            if (groupsCount == 1) return "1 group";
            return $"{groupsCount} group";
        }

        public static string BuildBookmarksCountText(int bookmarksCount)
        {
            if (bookmarksCount == 1) return "1 bookmark";
            return $"{bookmarksCount} bookmarks";
        }

        private static void CopyToSystemBuffer(Bookmarks bookmarks, bool compress)
        {
            GUIUtility.systemCopyBuffer = ToSystemBufferString(bookmarks, compress);
        }

        private static string ToSystemBufferString(Bookmarks bookmarks, bool compress)
        {
            if (compress)
            {
                return BookmarksClipboardCompressionHelper.Compress(bookmarks);
            }
            else
            {
                return JsonUtility.ToJson(bookmarks);
            }
        }
    }
}