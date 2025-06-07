using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class BookmarkSettingsPopup : PopupWindowContent
    {
        public bool IsOpened { get; private set; }

        private readonly Bookmark _bookmark;
        private readonly InvokesReferenceView _invokesReferenceView;
        private bool _dirty;

        private const string NameFieldControlId = "BookmarkNameTextField";
        private const string InvokesControlId = "InvokesField";

        public bool FocusOnNameField { get; set; }
        public bool FocusOnInvokesField { get; set; }

        private const float WindowWidth = 300f;
        private const float WindowHeight = 300f;
        private const float InvokesHeight = 200f;

        public BookmarkSettingsPopup(Bookmark bookmark)
        {
            _bookmark = bookmark;
            _invokesReferenceView = new InvokesReferenceView(_bookmark);
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(WindowWidth, WindowHeight);
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.LabelField("Name");
            GUI.SetNextControlName(NameFieldControlId);
            string newName = EditorGUILayout.TextField(_bookmark.BookmarkName, EditorStyles.textField);
            if (_bookmark.BookmarkName != newName)
            {
                _dirty = true;
                _bookmark.BookmarkName = newName;
            }

            DrawInvokesList();

            // PopupHelper.HandlePopupKeysEvents(Close);
            HandleFocus();
        }

        private void DrawInvokesList()
        {
            GUI.SetNextControlName(InvokesControlId);
            Rect invokesReferenceRect = new Rect(5f, 65f, WindowWidth - 10f, InvokesHeight);
            _dirty |= _invokesReferenceView.Draw(invokesReferenceRect);
        }

        private void HandleFocus()
        {
            if (FocusOnNameField)
            {
                FocusOnNameField = false;
                EditorGUI.FocusTextInControl(NameFieldControlId);
            }

            if (FocusOnInvokesField)
            {
                FocusOnInvokesField = false;
                EditorGUI.FocusTextInControl(InvokesControlId);
            }
        }

        private void Close()
        {
            editorWindow.Close();
        }

        public override void OnOpen()
        {
            IsOpened = true;
            _dirty = false;
            FocusOnNameField = true;
        }

        public override void OnClose()
        {
            IsOpened = false;

            if (_dirty)
            {
                Bookmarks bookmarks = BookmarksStorage.Get();
                if (bookmarks.SortType == SortType.NameAscending || bookmarks.SortType == SortType.NameDescending)
                {
                    bookmarks.Sort();
                }
                bookmarks.Dirty = true;
                BookmarksStorage.Save();
            }
        }
    }
}