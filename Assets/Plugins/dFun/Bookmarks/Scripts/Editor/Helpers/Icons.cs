using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class Icons
    {
        public static GUIContent BookmarksWindowIcon => EditorGUIUtility.isProSkin
            ? EditorGUIUtility.IconContent("d_FilterByType")
            : EditorGUIUtility.IconContent("FilterByType");

        public static GUIContent RemoveIcon
        {
            get
            {
#if UNITY_2023_1_OR_NEWER
                return EditorGUIUtility.IconContent("CrossIcon");
#else
                return EditorGUIUtility.isProSkin
                    ? EditorGUIUtility.IconContent("d_winbtn_win_close")
                    : EditorGUIUtility.IconContent("winbtn_win_close");
#endif
            }
        }

        public static GUIContent HelpIcon => EditorGUIUtility.IconContent("_Help");
        public static GUIContent DelayIcon => EditorGUIUtility.IconContent("d_WaitSpin00");
        public static GUIContent WarningIcon => EditorGUIUtility.IconContent("console.warnicon");

        private static GUIContent _invokesIcon;
        public static GUIContent InvokesIcon
        {
            get
            {
                if (_invokesIcon == null)
                {
                    _invokesIcon = EditorGUIUtility.IconContent("d_Animation Icon");
                    _invokesIcon.tooltip = "Contains Invokes";
                }
                return _invokesIcon;
            }
        }

        private static GUIContent _invokesIconBg;
        public static GUIContent InvokesIconBg
        {
            get
            {
                if (_invokesIconBg == null)
                {
                    _invokesIconBg = EditorGUIUtility.IconContent("sv_icon_dot0_pix16_gizmo");
                }
                return _invokesIconBg;
            }
        }      
        
        private static GUIContent _scriptIcon;
        public static GUIContent ScriptIcon
        {
            get
            {
                if (_scriptIcon == null)
                {
                    _scriptIcon = EditorGUIUtility.IconContent("d_cs Script Icon");
                }
                return _scriptIcon;
            }
        }

        public static GUIContent HelpIconWithTooltip(string tooltip)
        {
            GUIContent helpIcon = HelpIcon;
            helpIcon.tooltip = tooltip;
            return helpIcon;
        }
    }
}