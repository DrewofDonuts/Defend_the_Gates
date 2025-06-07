using System;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class BookmarksClipboardCompressionHelper
    {
        private const string HeaderPrefix = "Paste this to *Window/[Ease Bookmarks]* https://u3d.as/35Hf";
        private static string Header => $"{HeaderPrefix} v{Version.Value}{Splitter}{Splitter}";
        private const char Splitter = '\n';

        public static string Compress(Bookmarks bookmarks)
        {
            return Header + StringCompression.Compress(JsonUtility.ToJson(bookmarks));
        }

        public static bool TryToDecompress(string data, out Bookmarks result)
        {
            if (data == null)
            {
                result = default;
                return false;
            }

            try
            {
                string[] lines = data.Split(Splitter);
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (TryToDecompressLine(lines[i], out result))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                result = default;
                return false;
            }

            result = default;
            return false;
        }

        private static bool TryToDecompressLine(string lineData, out Bookmarks result)
        {
            try
            {
                string decompressed = StringCompression.Decompress(lineData);
                result = JsonUtility.FromJson<Bookmarks>(decompressed);
                return result != null;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }
    }
}