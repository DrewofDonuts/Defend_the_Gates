using System;

namespace DFun.Bookmarks
{
    [Flags]
    public enum SelectionFlags
    {
        /// Call EditorGUIUtility.PingObject for an object
        Ping = 1 << 0,

        /// Set an object as a Selection.activeObject
        Active = 1 << 1,

        /// Append an object to a Selection.objects array
        Append = 1 << 2,

        PingAndActive = Ping | Active,
        PingAndAppend = Ping | Append,
        PingAndActiveAndAppend = Ping | Active | Append
    }

    public static class SelectionFlagsUtils
    {
        public static bool IsFlagSet(this SelectionFlags flag, SelectionFlags compareTo)
        {
            return (flag & compareTo) != 0;
        }
    }
}