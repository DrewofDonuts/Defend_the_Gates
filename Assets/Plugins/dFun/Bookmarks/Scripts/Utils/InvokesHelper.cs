using System;
using DFun.GameObjectResolver;
using DFun.UnityDataTypes;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    public static class InvokesHelper
    {
        public static bool AllowInvokes => !KeyboardHelper.IsControlModifierPressed()
                                           && !KeyboardHelper.IsShiftModifierPressed();

        public static void DoInvokes(Bookmark bookmark, Object resolvedObject)
        {
            if (!AllowInvokes) return;
            if (resolvedObject == null) return;

            bookmark.ObjectReference.InvokesReference.Invoke(resolvedObject);
        }

        public static void DoInvokes(TypeReference typeReference, InvokesReference invokesReference)
        {
            if (!AllowInvokes) return;
            if (!typeReference.ContainsData) return;
            if (!invokesReference.ContainsData) return;

            if (typeReference.TryToResolveType(out Type type))
            {
                invokesReference.Invoke(type);
            }
        }
    }
}