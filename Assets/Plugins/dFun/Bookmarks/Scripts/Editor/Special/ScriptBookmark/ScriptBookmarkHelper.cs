using System;
using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class ScriptBookmarkHelper
    {
        public static void ShowAddScriptBookmarkPopup(BookmarksGroup targetGroup, Rect buttonGuiRect)
        {
            if (targetGroup == null) return;

            SelectScriptPopup selectScriptPopup = new SelectScriptPopup();
            selectScriptPopup.onScriptSelected += type => { CreateScriptBookmark(type, targetGroup); };
            PopupWindow.Show(buttonGuiRect, selectScriptPopup);
        }

        private static void CreateScriptBookmark(Type scriptType, BookmarksGroup targetGroup)
        {
            BookmarksUndo.BeforeBookmarkCreated();
            Bookmark typeBookmark = new Bookmark(scriptType);
            BookmarksStorage.Get().AddBookmarkToGroup(typeBookmark, targetGroup);
            BookmarksStorage.Save();
            BookmarksWindow.ForceRepaintAllWindows();
        }
    }
}