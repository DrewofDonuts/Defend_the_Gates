using System;

namespace DFun.Bookmarks
{
    public class BookmarkViewEvents
    {
        public event Action onCopyInput;
        public event Action onCopyCompressedInput;
        public event Action onPasteInput;
        public event Action onRemoveInput;

        public void FireCopyInputEvent()
        {
            if (onCopyInput != null)
            {
                onCopyInput.Invoke();
            }
        }
        
        public void FireCopyCompressedInputEvent()
        {
            if (onCopyCompressedInput != null)
            {
                onCopyCompressedInput.Invoke();
            }
        }

        public void FirePasteInputEvent()
        {
            if (onPasteInput != null)
            {
                onPasteInput.Invoke();
            }
        }

        public void FireRemoveInputEvent()
        {
            if (onRemoveInput != null)
            {
                onRemoveInput.Invoke();
            }
        }
    }
}