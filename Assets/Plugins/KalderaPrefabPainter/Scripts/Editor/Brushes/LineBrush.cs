using CollisionBear.WorldEditor.Extensions;
using CollisionBear.WorldEditor.Utils;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CollisionBear.WorldEditor.Brushes
{
    [System.Serializable]
    public class LineBrush : BrushBase
    {
        [System.Serializable]
        public class LineBrushSettings
        {
            public float BrushSpacing = 1f;
            public bool OrientWithBrush = false;
        }

        public override bool UseNormalRotation => false;

        protected override string ButtonImagePath => "Icons/IconGridLine.png";

        [SerializeField]
        private LineBrushSettings Settings = new LineBrushSettings();

        public override void DrawBrushEditor(ScenePlacer placer)
        {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(KalderaEditorUtils.BrushSpacingContent, GUILayout.Width(KalderaEditorUtils.OptionLabelWidth));
                var tmpBrushSpacing = EditorGUILayout.Slider(Settings.BrushSpacing, AreaBrushBase.BrushSpacingMin, AreaBrushBase.BrushSpacingMax);
                if (tmpBrushSpacing != Settings.BrushSpacing) {
                    Settings.BrushSpacing = tmpBrushSpacing;
                    placer.NotifyChange();
                }
            }

            using (new EditorGUILayout.HorizontalScope()) {
                using (new EditorGUILayout.HorizontalScope()) {
                    EditorCustomGUILayout.SetGuiBackgroundColorState(Settings.OrientWithBrush);
                    if (GUILayout.Button(KalderaEditorUtils.OrientWithBrushContent, GUILayout.Width(KalderaEditorUtils.IconButtonSize), GUILayout.Height(KalderaEditorUtils.IconButtonSize))) {
                        Settings.OrientWithBrush = !Settings.OrientWithBrush;
                        placer.NotifyChange();
                    }
                }
            }
        }

        public override void StartPlacement(Vector3 position, ScenePlacer placer)
        {
            StartDragPosition = position;
            EndDragPosition = position;
            Position = position;
        }

        public override List<GameObject> EndPlacement(Vector3 position, GameObject parentCollider, SelectionSettings settings, ScenePlacer placer)
        {
            StartDragPosition = null;
            EndDragPosition = null;
            return base.EndPlacement(position, parentCollider, settings, placer);
        }

        public override void OnClearSelection(ScenePlacer placer)
        {
            StartDragPosition = null;
            EndDragPosition = null;
        }

        public override void ActiveDragPlacement(Vector3 position, SelectionSettings selectionSettings, double deltaTime, ScenePlacer placer)
        {
            var delta = placer.CurrentRaycastMode.GetDelta(StartDragPosition.Value, position);
            EndDragPosition = position;          

            var objectCount = PointCountToGenerate(delta.magnitude, selectionSettings.GetSelectedItemSize() * Settings.BrushSpacing);
            var currenPlacementCount = placer.PlacementCollection.Placements.Count;

            if (objectCount < currenPlacementCount) {
                PruneToObjectCount(objectCount, placer.PlacementCollection);
            } else if (objectCount >= placer.PlacementCollection.Placements.Count) {
                UpdateAndFillUpToCount(position, delta, currenPlacementCount, selectionSettings, placer.PlacementCollection);
            }

            Rotation = placer.CurrentRaycastMode.GetRotationDirection(Position, position);

            // Custom rotation. The standarized just wont work for the line brush
            LineBrushRotation(Position, position, placer);
        }

        private void LineBrushRotation(Vector3 startPosition, Vector3 endPosition, ScenePlacer placer)
        {
            var distance = Vector3.Distance(startPosition, endPosition);

            var normalRotation = placer.CurrentBrush.UseNormalRotation ? Quaternion.LookRotation(Normal) * Quaternion.Euler(-90, 0, 0) : Quaternion.identity;
            var quaternion = Rotation.sqrMagnitude == 0 ? Quaternion.identity : Quaternion.LookRotation(Rotation);

            var offsetQuaternion = normalRotation * quaternion;

            foreach (var placement in placer.PlacementCollection.Placements) {
                var offsetFactor = placement.FixedOffset.magnitude / distance;

                var position = Vector3.Lerp(startPosition, endPosition, offsetFactor);
                placement.UpdateNormalizedOffset(GetOffsetPoint(position));
                placement.SetRotation((placement.NormalizedRotation * offsetQuaternion).eulerAngles);
            }

            placer.UpdatePlacements();
        }

        public override void ShiftDragPlacement(Vector3 position, SelectionSettings settings, double deltaTime, ScenePlacer placer)
        {
            // Fail safe in case we get here
            if (!StartDragPosition.HasValue) {
                StartPlacement(position, placer);
                return;
            }

            var snappedPosition = GetRotationSnappedPosition(StartDragPosition.Value, position);
            ActiveDragPlacement(snappedPosition, settings, deltaTime, placer);
        }

        private void PruneToObjectCount(int objectCount, PlacementCollection placementCollection)
        {
            for (int i = placementCollection.Placements.Count; i > objectCount; i--) {
                placementCollection.Placements.Last().ClearPlacementGameObject();
                placementCollection.Placements.RemoveAt(placementCollection.Placements.Count - 1);
            }
        }

        private void UpdateAndFillUpToCount(Vector3 end, Vector3 delta, int currentObjectCount, SelectionSettings selectionSettings, PlacementCollection placementCollection)
        {
            var offsetsToGenerate = PointsForLineForward(delta, selectionSettings.GetSelectedItemSize() * Settings.BrushSpacing);

            for (int i = 0; i < offsetsToGenerate.Count; i++) {
                if (i >= currentObjectCount) {
                    var offset = offsetsToGenerate[i];
                    var placementInformation = CreatePlacementInformation(Position, GetOffsetPoint(offset), selectionSettings);
                    placementCollection.Placements.Add(placementInformation);
                }

                if (Settings.OrientWithBrush) {
                    var placement = placementCollection.Placements[i];
                    var rotation = placement.Rotation.eulerAngles;
                    rotation.y = OrientWithBrushRotation(end, placement.Item);
                    placement.SetRotation(rotation);
                }
            }
        }

        public override void DrawBrushHandle(Vector3 placementPosition, Vector3 mousePosition)
        {
            if (HasDrag(StartDragPosition, EndDragPosition)) {
                DrawLineArrow(StartDragPosition.Value, EndDragPosition.Value, Normal);
            }
        }

        public override void DrawSceneHandleText(Vector2 screenPosition, Vector3 worldPosition, ScenePlacer placer)
        {
            if (!EndDragPosition.HasValue || !StartDragPosition.HasValue) {
                DrawHandleTextAtOffset(screenPosition, 2, GetClearContent());
                return;
            }

            try {
                DrawHandleTextAtOffset(screenPosition, 0, new GUIContent($"Object count: {placer.PlacementCollection.Placements.Count}"));
                DrawHandleTextAtOffset(screenPosition, 1, new GUIContent($"Rotation:\t {placer.CurrentBrush.Rotation.DirectionToEuler().ToString(RotationFormat)}"));
                DrawHandleTextAtOffset(screenPosition, 2, GetClearContent());
            } catch (System.Exception e) {
                Debug.LogException(e);
            }
        }

        protected override List<Vector3> GetPlacementOffsetValues(Vector3 position, SelectionSettings selectionSettings, ScenePlacer placer) => EmptyPointList;

        private int PointCountToGenerate(float distance, float objectSpacing)
        {
            // See how many objects we can fit on the line, with a minimum of one
            return Mathf.Max(1, Mathf.CeilToInt(distance / objectSpacing));
        }

        private List<Vector3> PointsForLineForward(Vector3 delta, float objectSpacing)
        {
            var result = new List<Vector3>();

            var direction = Vector3.forward;
            var objectCount = PointCountToGenerate(delta.magnitude, objectSpacing);
            var individualOffset = 1f / objectCount;

            var directionalEnd = objectCount * objectSpacing * direction;
            var endPosition = Position + directionalEnd;

            for (int i = 0; i < objectCount; i++) {
                var offset = Vector3.Lerp(Position, endPosition, i * individualOffset);
                result.Add(offset);
            }

            return result;
        }

        protected override Vector3 GetItemRotation(Vector3 position, PaletteItem item, GameObject prefabObject)
        {
            var result = base.GetItemRotation(position, item, prefabObject);

            if (Settings.OrientWithBrush) {
                result.y = OrientWithBrushRotation(position, item);
            }

            return result;
        }

        private float OrientWithBrushRotation(Vector3 endPosition, PaletteItem item)
        {
            var direction = GetDragDirection(StartDragPosition, endPosition);
            var rotationToEnd = direction.DirectionToPerpendicularRotationY();
            return rotationToEnd + item.AdvancedOptions.RotationOffset.y;
        }

        private Vector3 GetDragDirection(Vector3? startDragPosition, Vector3 placementPosition)
        {
            if (startDragPosition.HasValue) {
                return (startDragPosition.Value - placementPosition).normalized;
            } else {
                return Vector3.zero;
            }
        }
    }
}