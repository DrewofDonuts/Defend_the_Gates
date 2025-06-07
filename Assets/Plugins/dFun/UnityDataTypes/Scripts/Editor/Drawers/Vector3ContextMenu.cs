using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public static class Vector3ContextMenu
    {
        public static void ShowContextMenu(ContextMenuData contextMenuData)
        {
            GenericMenu menu = new GenericMenu();
            {
                menu.AddItem(new GUIContent("Copy"), false, () => CopyVector3(contextMenuData));

                if (BufferContainsVector3Data())
                {
                    menu.AddItem(new GUIContent("Paste"), false, () => TryPasteVector3(contextMenuData));
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Paste"), false);
                }

                menu.AddItem(new GUIContent("Reset"), false, () => Reset(contextMenuData));
            }
            menu.ShowAsContext();
        }

        private static void CopyVector3(ContextMenuData contextMenuData)
        {
            Vector3 value = (Vector3)contextMenuData.NonSerializedValue;
            ContextMenuHelper.CopyBuffer = CreateVector3Buffer(value);
        }

        private static void TryPasteVector3(ContextMenuData contextMenuData)
        {
            if (TryParseVector3(ContextMenuHelper.CopyBuffer, out Vector3 res))
            {
                contextMenuData.OnValueChange.Invoke(res);
            }
        }

        private static void Reset(ContextMenuData contextMenuData)
        {
            contextMenuData.OnValueChange.Invoke(Vector3DataTypeSerializer.DefaultValue);
        }

        public static bool BufferContainsVector3Data()
        {
            return TryParseVector3(ContextMenuHelper.CopyBuffer, out Vector3 _);
        }

        public static bool TryParseVector3(string text, out Vector3 res)
        {
            res = Vector3.zero;
            float[] v = ContextMenuHelper.ParseFloats(text, "Vector3", 3);
            if (v == null)
                return false;
            res = new Vector3(v[0], v[1], v[2]);
            return true;
        }

        public static string CreateVector3Buffer(Vector3 value)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Vector3({0:g9},{1:g9},{2:g9})",
                value.x, value.y, value.z
            );
        }
    }
}