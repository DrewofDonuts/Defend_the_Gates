using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarksGroupSettingsPopup : PopupWindowContent
    {
        private readonly BookmarksGroup _group;
        private bool _dirty;

        private const string NameFieldControlId = "BookmarkGroupNameTextField";
        private bool FocusOnNameField { get; set; }

        public BookmarksGroupSettingsPopup(BookmarksGroup group)
        {
            _group = group;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 145);
        }

        public override void OnGUI(Rect rect)
        {
            DrawName();
            DrawTooltip();

            EditorGUILayout.Space(20f);

            PopupHelper.HandlePopupKeysEventsExt(Close);
            HandleFocus();
        }

        private void HandleFocus()
        {
            if (FocusOnNameField)
            {
                FocusOnNameField = false;
                EditorGUI.FocusTextInControl(NameFieldControlId);
            }
        }

        private void Close()
        {
            editorWindow.Close();
        }

        private void DrawName()
        {
            EditorGUILayout.LabelField("Group Name");
            GUI.SetNextControlName(NameFieldControlId);
            string newName = EditorGUILayout.TextField(_group.Name, EditorStyles.textField);
            if (_group.Name != newName)
            {
                _dirty = true;
                _group.Name = newName;
            }
        }

        private void DrawTooltip()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tooltip");
            string newTooltip = EditorGUILayout.TextArea(
                _group.Description, EditorStyles.textArea, GUILayout.Height(60f)
            );
            if (_group.Description != newTooltip)
            {
                _dirty = true;
                _group.Description = newTooltip;
            }
        }

        public override void OnOpen()
        {
            _dirty = false;
            FocusOnNameField = true;
        }

        public override void OnClose()
        {
            if (_dirty)
            {
                BookmarksStorage.Get().Dirty = true;
                BookmarksStorage.Save();
                BookmarksWindow.ForceRepaintAllWindows();
            }
        }
    }
}