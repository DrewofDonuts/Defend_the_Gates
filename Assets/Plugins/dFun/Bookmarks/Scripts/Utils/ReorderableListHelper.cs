using UnityEditorInternal;

namespace DFun.Bookmarks
{
    public static class ReorderableListHelper
    {
        public static bool SelectionAllowed
        {
            get
            {
#if UNITY_2021_1_OR_NEWER 
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsElementSelected(ReorderableList list, int elementIndex)
        {
#if UNITY_2021_1_OR_NEWER
            return list.IsSelected(elementIndex);
#else
            return false;
#endif
        }

        public static void Select(ReorderableList list, int elementIndex, bool append)
        {
#if UNITY_2021_1_OR_NEWER
            list.Select(elementIndex, append);
#endif
        }

        public static void ClearSelection(ReorderableList list)
        {
#if UNITY_2021_1_OR_NEWER
            list.ClearSelection();
#endif
        }
    }
}