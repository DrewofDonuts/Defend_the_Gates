using CollisionBear.WorldEditor.Extensions;
using CollisionBear.WorldEditor.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CollisionBear.WorldEditor.Brushes
{
    [System.Serializable]
    public class PathBrush : BrushBase
    {
        private const float MaxSegmentLength = 0.25f;
        private const float BrushHandleInterval = 6f;

        [System.Serializable]
        public class PathBrushSettings
        {
            public float BrushSpacing = 1f;
            public bool OrientWithBrush = false;
        }

        public override bool UseNormalRotation => false;

        protected override string ButtonImagePath => "Icons/IconPathTool.png";

        private List<Vector3> CurrentPath = new List<Vector3>();

        private Vector3? LastUserPosition;
        private int CurrentIndex;
        private int LastPlacementIndex;

        [SerializeField]
        private PathBrushSettings Settings = new PathBrushSettings();

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
            ResetDrag();

            StartDragPosition = position;
            EndDragPosition = position;
            Position = position;

            LastPlacementIndex = 0;

            AddFirstSegment(position);
        }

        public override List<GameObject> EndPlacement(Vector3 position, GameObject parentCollider, SelectionSettings settings, ScenePlacer placer)
        {
            ResetDrag();
            StartDragPosition = null;
            EndDragPosition = null;
            LastUserPosition = null;

            PruneEndPiece(placer.PlacementCollection);

            return base.EndPlacement(position, parentCollider, settings, placer);
        }

        public override void OnClearSelection(ScenePlacer placer)
        {
            StartDragPosition = null;
            EndDragPosition = null;
            LastUserPosition = null;
        }

        public override void ActiveDragPlacement(Vector3 position, SelectionSettings selectionSettings, double deltaTime, ScenePlacer placer)
        {
            EndDragPosition = position;
            AddSegmentedPath(position, selectionSettings, placer);
            placer.UpdatePlacements();
        }

        private void AddFirstSegment(Vector3 position)
        {
            CurrentPath.Add(position);
            LastUserPosition = position;
        }

        private void AddSegmentedPath(Vector3 position, SelectionSettings settings, ScenePlacer placer)
        {
            CurrentPath.AddRange(GetSegmentPath(LastUserPosition.Value, position));
            UpdatePath(settings, placer);
            LastUserPosition = position;
        }

        private List<Vector3> GetSegmentPath(Vector3 start, Vector3 end)
        {
            var result = new List<Vector3>();
            if (Vector3.Distance(start, end) == 0f) {
                return result;
            }

            var length = Vector3.Distance(start, end);

            if (length < MaxSegmentLength) {
                result.Add(end);
            } else {
                var segmentsRequired = Mathf.CeilToInt(length / MaxSegmentLength);
                var segmentLength = 1f / segmentsRequired;

                for (int i = 1; i <= segmentsRequired; i++) {
                    result.Add(Vector3.Lerp(start, end, i * segmentLength));
                }
            }

            return result;
        }

        public override void ShiftDragPlacement(Vector3 position, SelectionSettings settings, double deltaTime, ScenePlacer placer)
        {
            // Failsafe in case we get here
            if (!StartDragPosition.HasValue) {
                StartPlacement(position, placer);
                return;
            }

            var snappedPosition = GetRotationSnappedPosition(StartDragPosition.Value, position);
            ActiveDragPlacement(snappedPosition, settings, deltaTime, placer);
        }

        private void ResetDrag()
        {
            CurrentPath.Clear();
            CurrentIndex = 0;

            LastPlacementIndex = -1;
            LastUserPosition = null;
        }

        private void UpdatePath(SelectionSettings selectionSettings, ScenePlacer placer)
        {
            var pointsToAdd = PointsForPath(selectionSettings.GetSelectedItemSize() * Settings.BrushSpacing);

            foreach (var point in pointsToAdd) {
                var fixedPoint = GetOffsetPoint(point);
                fixedPoint.y = 0;

                var placementInformation = CreatePlacementInformation(Position, fixedPoint, selectionSettings);
                placementInformation.SetRotation(placementInformation.Rotation * QuaterRotationY);
                placer.PlacementCollection.Placements.Add(placementInformation);
                LastPlacementIndex = placer.PlacementCollection.Placements.IndexOf(placementInformation);
            }

            UpdateLastPlacement(placer);
        }

        private void UpdateLastPlacement(ScenePlacer placer)
        {
            if (LastPlacementIndex < 0) {
                return;
            }        

            var startPosition = CurrentPath[CurrentIndex];
            var endPosition = CurrentPath.Last();
            UpdatePlacementPosition(placer, startPosition, endPosition);
        }

        private void UpdatePlacementPosition(ScenePlacer placer, Vector3 startPosition, Vector3 endPosition)
        {
            var placement = placer.PlacementCollection.Placements[LastPlacementIndex];
            var position = Vector3.Lerp(startPosition, endPosition, 0.5f);

            placement.UpdateNormalizedOffset(GetOffsetPoint(position));

            if (Settings.OrientWithBrush) {
                var rotation = placement.Rotation.eulerAngles;

                rotation.y = OrientWithBrushRotation(startPosition, endPosition, placement.Item) + 90;
                placement.SetRotation(rotation);
            }
        }

        private List<Vector3> PointsForPath(float objectSpacing)
        {
            var result = new List<Vector3>();
            var lengthSinceLastPlacement = 0f;

            for (int i = Mathf.Max(CurrentIndex, 1); i < CurrentPath.Count; i++) {
                lengthSinceLastPlacement += Vector3.Distance(CurrentPath[i - 1], CurrentPath[i]);

                if (lengthSinceLastPlacement >= objectSpacing) {
                    lengthSinceLastPlacement -= objectSpacing;

                    var currentPosition = CurrentPath[CurrentIndex];

                    // If we place multiple object the same frame, give them all the opportunity to center themselves
                    if (result.Count > 0) {
                        var lastIndex = result.Count - 1;
                        result[lastIndex] = Vector3.Lerp(result[lastIndex], currentPosition, 0.5f);
                    }

                    result.Add(currentPosition);
                    CurrentIndex = i;
                }
            }

            return result;
        }

        private void PruneEndPiece(PlacementCollection placementCollection)
        {
            if (placementCollection.Placements.Count <= 1) {
                return;
            }

            var lastPlacement = placementCollection.Placements.Last();
            GameObject.DestroyImmediate(lastPlacement.GameObject);
            placementCollection.Placements.Remove(lastPlacement);
        }

        public override void DrawBrushHandle(Vector3 placementPosition, Vector3 mousePosition)
        {
            if (!HasDrag(StartDragPosition, EndDragPosition)) {
                return;
            }

            DrawIntialArrow();
            DrawFinalArrow();
        }

        private void DrawIntialArrow()
        {
            var rotation = Quaternion.identity;

            if (CurrentPath.Count > 1) {
                rotation = Quaternion.LookRotation((FindPointAtDistance(startIndex: 0, distance: BrushHandleInterval) - CurrentPath[0]).normalized) * QuaterRotationY;
            }

            DrawOnSceneViewMesh(KalderaEditorUtils.PlaneMesh, KalderaEditorUtils.ArrowMaterial, CurrentPath[0] - (rotation * Vector3.left * ArrowSize * 0.75f), rotation, new Vector3(ArrowSize, ArrowSize, ArrowSize));
        }

        private Vector3 FindPointAtDistance(int startIndex, float distance)
        {
            var currentIndex = startIndex;
            var currentLength = 0f;

            while (currentIndex < CurrentPath.Count - 1 && currentLength < distance) {
                currentLength += Vector3.Distance(CurrentPath[currentIndex], CurrentPath[currentIndex + 1]);
                currentIndex++;
            }

            return CurrentPath[currentIndex];
        }

        private void DrawFinalArrow()
        {
            var position = CurrentPath.Last();

            var rotation = Quaternion.identity;
            var startPosition = FindPointAtDistanceBackwards(CurrentPath.Count - 1, BrushHandleInterval);
            if (Vector3.Distance(startPosition, position) > 0f) {
                rotation = Quaternion.LookRotation((startPosition - position).normalized) * Quaternion.Inverse(QuaterRotationY);
            }

            DrawOnSceneViewMesh(KalderaEditorUtils.PlaneMesh, KalderaEditorUtils.LongArrowMaterial, position - (rotation * Vector3.left * ArrowSize * 0.75f), rotation, new Vector3(ArrowSize, ArrowSize, ArrowSize));
        }

        private Vector3 FindPointAtDistanceBackwards(int startIndex, float distance)
        {
            var currentIndex = startIndex;
            var currentLength = 0f;

            while (currentIndex > 2 && currentLength < distance) {
                currentLength += Vector3.Distance(CurrentPath[currentIndex], CurrentPath[currentIndex - 1]);
                currentIndex--;
            }

            return CurrentPath[currentIndex];
        }

        public override void DrawSceneHandleText(Vector2 screenPosition, Vector3 worldPosition, ScenePlacer placer)
        {
            var rotation = Quaternion.identity;

            if (EndDragPosition.HasValue && StartDragPosition.HasValue) {
                var dragDirection = GetDragDirection(EndDragPosition, EndDragPosition.Value);
                if (dragDirection.sqrMagnitude > 0f) {
                    rotation = Quaternion.LookRotation(dragDirection);
                }
            }

            try {
                DrawHandleTextAtOffset(screenPosition, 0, new GUIContent($"Rotation:\t {rotation.eulerAngles.y.ToString(RotationFormat)}"));
                DrawHandleTextAtOffset(screenPosition, 1, new GUIContent($"Object count: {placer.PlacementCollection.Placements.Count}"));
                DrawHandleTextAtOffset(screenPosition, 2, GetClearContent());
            } catch (System.Exception e) {
                Debug.LogException(e);
            }
        }

        protected override List<Vector3> GetPlacementOffsetValues(Vector3 position, SelectionSettings selectionSettings, ScenePlacer placer) => EmptyPointList;

        private float OrientWithBrushRotation(Vector3 startPosition, Vector3 endPosition, PaletteItem item)
        {
            var direction = GetDragDirection(startPosition, endPosition);
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