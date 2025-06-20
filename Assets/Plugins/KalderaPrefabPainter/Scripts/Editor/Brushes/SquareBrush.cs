﻿using CollisionBear.WorldEditor.Generation;
using UnityEditor;
using UnityEngine;

namespace CollisionBear.WorldEditor.Brushes
{
    [System.Serializable]
    public class SquareBrush : AreaBrushBase
    {
        private static readonly Vector3[] MeshCorners = new Vector3[4] {
            new Vector3(-1, 0,  -1),
            new Vector3(1,  0,  -1),
            new Vector3(1,  0,  1),
            new Vector3(-1, 0,  1)
        };

        private static readonly Vector3[] MeshLineSegments = new Vector3[8] {
            new Vector3(-1, 0, -1),
            new Vector3(1,  0, -1),
            new Vector3(1,  0, 1),
            new Vector3(-1, 0, 1),
            new Vector3(-1, 0, -1),
            new Vector3(-1, 0, 1),
            new Vector3(1,  0, 1),
            new Vector3(1,  0, -1),
        };

        private class SquareGenerationBounds : IGenerationBounds
        {
            public bool IsWithinBounds(float size, BoxRect box) => true;
        }

        protected override string ButtonImagePath => "Icons/IconGridSquare.png";

        private readonly Vector3[] MeshCornerCache = new Vector3[4];
        private readonly Vector3[] MeshLineSegmentCache = new Vector3[8];

        public SquareBrush()
        {
            GenerationBounds = new SquareGenerationBounds();
        }

        public override void DrawBrushHandle(Vector3 placementPosition, Vector3 mousePosition)
        {
            var rotation = Quaternion.LookRotation(Normal) * Quaternion.Euler(90, 0, 0) * Quaternion.Inverse(Quaternion.LookRotation(Rotation));
            var size = Settings.BrushSize;

            for (int i = 0; i < MeshCornerCache.Length; i++) {
                MeshCornerCache[i] = placementPosition + (rotation * MeshCorners[i] * size);
            }

            Handles.color = HandleBrushColor;
            Handles.DrawAAConvexPolygon(MeshCornerCache);

            for (int i = 0; i < MeshLineSegmentCache.Length; i++) {
                MeshLineSegmentCache[i] = placementPosition + (rotation * MeshLineSegments[i] * size);
            }

            Handles.color = HandleOutlineColor;
            Handles.DrawLines(MeshLineSegmentCache);

            if (HasDrag(StartDragPosition, EndDragPosition)) {
                DrawRotationCompass(StartDragPosition.Value, mousePosition, Rotation, Normal);
                DrawRotationArrow(StartDragPosition.Value, mousePosition, Rotation, Normal);
                return;
            }

            if (StartHoverPosition.HasValue) {
                DrawRotationCompass(StartHoverPosition.Value, mousePosition, Rotation, Normal);
                DrawRotationArrow(StartHoverPosition.Value, mousePosition, Rotation, Normal);
                return;
            }
        }
    }
}
