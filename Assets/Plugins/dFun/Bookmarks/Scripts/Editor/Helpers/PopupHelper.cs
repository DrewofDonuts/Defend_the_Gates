using System;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class PopupHelper
    {
        private static readonly KeyCode[] ClosePopupKeyCodes = { KeyCode.Escape };
        private static readonly KeyCode[] ClosePopupKeyCodesExt = { KeyCode.Return, KeyCode.Escape };

        public static void HandlePopupKeysEvents(Action onPress)
        {
            HandlePopupKeysEvents(onPress, ClosePopupKeyCodes);
        }
        
        public static void HandlePopupKeysEventsExt(Action onPress)
        {
            HandlePopupKeysEvents(onPress, ClosePopupKeyCodesExt);
        }

        private static void HandlePopupKeysEvents(Action onPress, KeyCode[] closerKeyCodes)
        {
            Event e = Event.current;

            if (e.type == EventType.KeyUp && Array.IndexOf(closerKeyCodes, e.keyCode) >= 0)
            {
                onPress.Invoke();
            }
        }
    }
}