namespace DFun.Bookmarks
{
    public class BookmarkInteractionState
    {
        public bool WasClicked { get; set; }
        public bool WasDoubleClicked { get; set; }
        public bool WasContextClicked { get; set; }
        public bool ReadyToShowContext { get; private set; }
        public bool DragStarted { get; set; }

        public void Reset()
        {
            WasClicked = false;
            WasDoubleClicked = false;
            ReadyToShowContext = WasContextClicked;
            WasContextClicked = false;
            DragStarted = false;
        }

        public void OnShowContext()
        {
            WasContextClicked = false;
            ReadyToShowContext = false;
        }
    }
}