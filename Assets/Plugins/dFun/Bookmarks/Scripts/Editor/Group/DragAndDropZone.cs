using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class DragAndDropZone
    {
        private bool AllowDragAndDrop => !DragAndDropHelper.IsDraggingAnyBookmarkObject();

        public void Draw()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(
                "Drag & Drop\nany asset or scene object here",
                Styles.DragNDropLabelStyle
            );

            DrawPasteButton();

            GUILayout.FlexibleSpace();
        }

        private static void DrawPasteButton()
        {
            if (!BookmarksClipboard.HasData)
            {
                return;
            }

            GUILayout.Space(30);
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Paste From Clipboard") || BookmarksGroupShortcuts.WasKeyboardPastePressed())
                {
                    BookmarksGroupClipboardHelper.PasteClipboardDataToActiveGroup();
                }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        public bool DoUpdate(Bookmarks bookmarks)
        {
            EventType eType = Event.current.type;

            switch (eType)
            {
                case EventType.DragUpdated when AllowDragAndDrop:
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    return false;

                case EventType.DragPerform when AllowDragAndDrop:
                    Object[] dragObjects = DragAndDrop.objectReferences;
                    if (DragAndDropHelper.IsDraggingAnyBookmarkObject(dragObjects))
                    {
                        return false;
                    }

                    BookmarksUndo.BeforeBookmarkCreated();
                    bookmarks.AddItems(dragObjects);
                    return true;
            }

            return false;
        }
    }
}