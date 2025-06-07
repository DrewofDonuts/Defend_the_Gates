using UnityEngine;

namespace DFun.Bookmarks
{
    public class HighlightComponentData
    {
        public readonly GameObject componentParent;
        public readonly Component component;

        public bool ReadyToHighlight { get; set; }

        public HighlightComponentData(GameObject componentParent, Component component)
        {
            this.componentParent = componentParent;
            this.component = component;
        }

        public bool IsValid()
        {
            return componentParent != null && component != null;
        }
    }
}