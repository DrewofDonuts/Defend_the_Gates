using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DFun.Bookmarks
{
    public class ComponentHighlightAction
    {
        public bool IsValid => _targetHeader != null
                               && _highlightImage != null;

        public bool IsFinished { get; private set; }
        public EditorWindow ParentParentInspector { get; private set; }

        private readonly VisualElement _targetHeader;
        private readonly Image _highlightImage;

        private readonly Color _highlightColor = new Color(1.0f, 0.8f, 0.0f, 0.5f);

        private const float HighlightDuration = 1.2f;
        private float _highlightTimer;

        public ComponentHighlightAction(VisualElement targetInspector, EditorWindow parentInspector)
        {
            _targetHeader = ComponentsFinder.GetHeader(targetInspector);
            ParentParentInspector = parentInspector;

            _highlightImage = CreateHighlightImage();
            _targetHeader?.Add(_highlightImage);
        }

        private Image CreateHighlightImage()
        {
            Rect headerContentRect = _targetHeader.contentRect;
            int imageWidth = (int) headerContentRect.width;
            int imageHeight = (int) headerContentRect.height;

            if (imageWidth <= 0 || imageHeight <= 0)
            {
                return null;
            }

            return new Image
            {
                pickingMode = PickingMode.Ignore,
                image = Styles.MakeTex(imageWidth, imageHeight, _highlightColor)
            };
        }

        public void Update(float dt)
        {
            _highlightTimer += dt;
            if (_highlightTimer > HighlightDuration || !IsValid)
            {
                Finish();
            }
        }

        public void Finish()
        {
            IsFinished = true;
            _highlightImage?.RemoveFromHierarchy();
        }
    }
}