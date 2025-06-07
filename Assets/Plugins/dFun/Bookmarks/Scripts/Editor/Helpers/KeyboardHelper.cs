using UnityEngine;

namespace DFun.Bookmarks
{
    public static class KeyboardHelper
    {
        public static bool WasDeleteButtonPressed()
        {
            return WasKeyPressed(KeyCode.Delete);
        }

        public static bool WasBackspaceButtonPressed()
        {
            return WasKeyPressed(KeyCode.Backspace);
        }

        public static bool WasRenameButtonPressed()
        {
            return WasKeyPressed(KeyCode.F2);
        }

        public static bool WasLeftButtonPressed()
        {
            return WasKeyPressed(KeyCode.LeftArrow);
        }

        public static bool WasRightButtonPressed()
        {
            return WasKeyPressed(KeyCode.RightArrow);
        }

        public static bool WasUpButtonPressed()
        {
            return WasKeyPressed(KeyCode.UpArrow);
        }

        public static bool WasDownButtonPressed()
        {
            return WasKeyPressed(KeyCode.DownArrow);
        }

        public static bool WasEnterButtonPressed()
        {
            return WasKeyPressed(KeyCode.Return)
                   || WasKeyPressed(KeyCode.KeypadEnter);
        }

        public static bool WasTabButtonPressed()
        {
            return WasKeyPressed(KeyCode.Tab);
        }

        public static bool WasKeyPressed(KeyCode keyCode)
        {
            Event e = Event.current;
            EventType eType = e.type;

            if (eType != EventType.KeyDown)
            {
                return false;
            }

            if (e.keyCode == keyCode)
            {
                return true;
            }

            return false;
        }

        public static bool IsControlModifierPressed()
        {
            return IsModifierPressed(EventModifiers.Control)
                   || IsModifierPressed(EventModifiers.Command);
        }

        public static bool IsShiftModifierPressed()
        {
            return IsModifierPressed(EventModifiers.Shift);
        }

        public static bool IsModifierPressed(EventModifiers modifier)
        {
            return (Event.current.modifiers & modifier) != 0;
        }
    }
}