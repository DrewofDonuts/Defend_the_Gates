using System;

namespace DFun.GameObjectResolver
{
    public static class TypeUtils
    {
        public static bool TryParseClassType(string classType, out Type type)
        {
            if (classType == null)
            {
                type = default;
                return false;
            }

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