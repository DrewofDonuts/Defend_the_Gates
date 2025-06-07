using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BottomToolbarView
    {
        public const float ToolbarHeight = 21f;
        private const float LockToGroupToggleWidth = 16f;
        private const float IconSizeSliderWidth = 55f;
        private readonly GUIStyle _toolbarBg = "ProjectBrowserBottomBarBg";
        private readonly GUIContent _lockGridSizeTooltip = new GUIContent(
            string.Empty, "Lock grid size to the group"
        );

        private Bookmarks Bookmarks => BookmarksStorage.Get();

        private float GridSizeNormalized
        {
            get => Bookmarks.GridSizeNormalized;
            set => Bookmarks.GridSizeNormalized = value;
        }

        private bool IsGridSizeLockedToGroup
        {
            get => Bookmarks.IsGridSizeLockedToActiveGroup;
            set => Bookmarks.IsGridSizeLockedToActiveGroup = value;
        }

        private SortType SortType => Bookmarks.SortType;

        public void Draw(Rect toolbarRect)
        {
            GUILayout.BeginArea(toolbarRect, EditorStyles.toolbar);
            {
                Rect beginHorizontal = EditorGUILayout.BeginHorizontal(GUILayout.Height(ToolbarHeight));
                GUI.Label(beginHorizontal, GUIContent.none, _toolbarBg);
                {
                    DrawSortType();
                    GUILayout.FlexibleSpace();
                    DrawIconSizeSlider();
                    DrawLockToGroupToggle();
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }

        private void DrawSortType()
        {
            GUIContent shortNameContent = SortTypeSelector.GetShortNameContent(Bookmarks.SortType);
            if (EditorGUILayout.DropdownButton(shortNameContent, FocusType.Passive))
            {
                SortTypeSelector.Show(SortType);
            }
        }

        private void DrawLockToGroupToggle()
        {
            EditorGUI.BeginChangeCheck();
            {
                bool isGridSizeLockedToGroup = EditorGUILayout.Toggle(
                    IsGridSizeLockedToGroup, Styles.LockButton,
                    GUILayout.Width(LockToGroupToggleWidth)
                );
                if (EditorGUI.EndChangeCheck())
                {
                    IsGridSizeLockedToGroup = isGridSizeLockedToGroup;
                }
            }
            Rect toggleRect = GUILayoutUtility.GetLastRect();
            GUI.Label(toggleRect, _lockGridSizeTooltip, GUIStyle.none);
        }

        private void DrawIconSizeSlider()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(1.5f);
                EditorGUI.BeginChangeCheck();
                {
                    Rect sliderRect = EditorGUILayout.GetControlRect(GUILayout.Width(IconSizeSliderWidth));
                    float newGridSize = GUI.HorizontalSlider(sliderRect, GridSizeNormalized, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        GridSizeNormalized = newGridSize;
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}