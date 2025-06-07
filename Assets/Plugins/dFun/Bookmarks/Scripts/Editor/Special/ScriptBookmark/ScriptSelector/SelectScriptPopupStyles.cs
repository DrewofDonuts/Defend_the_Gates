using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class SelectScriptPopupStyles
    {
        public const float WindowWidth = 350;
        public const float WindowHeight = 600;
        public const float ScrollViewHeight = 530;
        private const float SelectButtonWidth = 60;
        private const float TooltipIconWidth = 15;
        private const float ScrollBarWidth = 30;

        public static GUIStyle Caption => EditorStyles.boldLabel;
        public static GUIStyle SearchField => EditorStyles.toolbarSearchField;

        private static GUIStyle _cleanSearchButton;
        public static GUIStyle CleanSearchButton
        {
            get
            {
                if (_cleanSearchButton == null)
                {
                    _cleanSearchButton = "SearchCancelButton";
                }
                return _cleanSearchButton;
            }
        }

        public static GUIStyle ScriptButtonStyle => EditorStyles.label;
        public static GUIStyle ScriptButtonStyleSelected => EditorStyles.boldLabel;

        //layout options
        public static readonly GUILayoutOption LabelLayoutOpt = GUILayout.Width(
            WindowWidth - SelectButtonWidth - TooltipIconWidth - ScrollBarWidth
        );
        public static readonly GUILayoutOption SelectButtonLayoutOpt = GUILayout.Width(SelectButtonWidth);
        public static readonly GUILayoutOption TooltipButtonOpt = GUILayout.Width(TooltipIconWidth);

        public const float WaitIconHeight = 15;
        public const float WaitStubLineHeight = 17;
        public static GUIStyle WaitIconStyle => EditorStyles.label;
        public static readonly GUILayoutOption WaitIconOpt = GUILayout.Height(WaitIconHeight);
    }
}