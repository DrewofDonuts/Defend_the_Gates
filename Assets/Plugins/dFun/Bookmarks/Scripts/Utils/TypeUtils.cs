using System;

namespace DFun.Bookmarks
{
    public static class TypeUtils
    {
        public static bool TryParseClassType(string classType, out Type type)
        {
            try
            {
                type = Type.GetType(classType.Trim(), true, true);
                return true;
            }
            catch (Exception)
            {
                // ignored
            }

            type = default;
            return false;
        }
    }
}