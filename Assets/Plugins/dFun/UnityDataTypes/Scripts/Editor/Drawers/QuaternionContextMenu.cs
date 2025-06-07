using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace DFun.UnityDataTypes
{
    public static class QuaternionContextMenu
    {
        public static void ShowContextMenu(ContextMenuData contextMenuData)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Copy Quaternion"), false, () => Copy(contextMenuData));
            menu.AddItem(new GUIContent("Copy Euler"), false, () => CopyEuler(contextMenuData));
            if (BufferContainsQuaternionData())
            {
                menu.AddItem(new GUIContent("Paste"), false, () => TryPaste(contextMenuData));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Paste"), false);
            }
            menu.AddItem(new GUIContent("Reset"), false, () => Reset(contextMenuData));
            menu.ShowAsContext();
        }

        private static void Copy(ContextMenuData contextMenuData)
        {
            Quaternion value = (Quaternion)contextMenuData.NonSerializedValue;
            ContextMenuHelper.CopyBuffer = CreateQuaternionBuffer(value);
        }

        private static void CopyEuler(ContextMenuData contextMenuData)
        {
            Quaternion value = (Quaternion)contextMenuData.NonSerializedValue;
            ContextMenuHelper.CopyBuffer = Vector3ContextMenu.CreateVector3Buffer(value.eulerAngles);
        }

        private static void TryPaste(ContextMenuData contextMenuData)
        {
            if (TryParseQuaternion(ContextMenuHelper.CopyBuffer, out Quaternion res))
            {
                contextMenuData.OnValueChange.Invoke(res);
                return;
            }

            if (Vector3ContextMenu.TryParseVector3(ContextMenuHelper.CopyBuffer, out Vector3 resEuler))
            {
                res = Quaternion.Euler(resEuler);
                contextMenuData.OnValueChange.Invoke(res);
            }
        }

        private static void Reset(ContextMenuData contextMenuData)
        {
            contextMenuData.OnValueChange.Invoke(QuaternionDataTypeSerializer.DefaultValue);
        }

        private static bool BufferContainsQuaternionData()
        {
            return TryParseQuaternion(ContextMenuHelper.CopyBuffer, out Quaternion _)
                   || Vector3ContextMenu.BufferContainsVector3Data();
        }

        private static bool TryParseQuaternion(string text, out Quaternion res)
        {
            res = Quaternion.identity;
            float[] v = ContextMenuHelper.ParseFloats(text, "Quaternion", 4);
            if (v == null)
                return false;
            res = new Quaternion(v[0], v[1], v[2], v[3]);
            return true;
        }

        private static string CreateQuaternionBuffer(Quaternion value)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "Quaternion({0:g9},{1:g9},{2:g9},{3:g9})",
                value.x, value.y, value.z, value.w
            );
        }
    }
}