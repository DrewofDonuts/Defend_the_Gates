using UnityEngine;

namespace DFun.Bookmarks
{
    public class OptionalRect
    {
        public static readonly OptionalRect None = new OptionalRect();

        public readonly bool Exists;
        public readonly Rect Data;

        private OptionalRect()
        {
            Exists = false;
        }

        public OptionalRect(Rect data)
        {
            Exists = true;
            Data = data;
        }
    }
}