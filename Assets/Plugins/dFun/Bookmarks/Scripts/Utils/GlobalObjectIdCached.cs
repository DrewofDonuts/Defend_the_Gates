using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class GlobalObjectIdCached
    {
        private static readonly GlobalObjectId NoneGlobalObjectId = new GlobalObjectId();

        private static readonly Dictionary<Object, GlobalObjectId> ObjToIdCache =
            new Dictionary<Object, GlobalObjectId>();

        private static readonly Dictionary<GlobalObjectId, Object> IdToObjCache =
            new Dictionary<GlobalObjectId, Object>();

        public static GlobalObjectId GetObjectId(Object obj)
        {
            if (ObjToIdCache.TryGetValue(obj, out GlobalObjectId id) && !id.Equals(NoneGlobalObjectId))
            {
                return id;
            }

            id = GlobalObjectId.GetGlobalObjectIdSlow(obj);
            ObjToIdCache[obj] = id;

            return id;
        }

        public static Object GetObject(GlobalObjectId id)
        {
            if (IdToObjCache.TryGetValue(id, out Object obj) && obj != null)
            {
                return obj;
            }

            obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
            if (obj != null)
            {
                IdToObjCache[id] = obj;
            }

            return obj;
        }
    }
}