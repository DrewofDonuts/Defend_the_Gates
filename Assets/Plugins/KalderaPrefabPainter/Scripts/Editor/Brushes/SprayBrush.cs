using CollisionBear.WorldEditor.Distribution;
using CollisionBear.WorldEditor.Generation;
using CollisionBear.WorldEditor.Utils;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CollisionBear.WorldEditor.Brushes
{
    [System.Serializable]
    public class SprayBrush : BrushBase
    {
        public const float SprayIntensityMin = 1f;
        public const float SprayIntensityMax = 100f;

        [System.Serializable]
        public class SprayBrushSettings
        {
            public float BrushSize = AreaBrushSettings.BrushSizePresets[1].BrushSize;
            public float ObjectDensity = 1.0f;
            public float SprayIntensity = 10.0f;
        }

        private Vector3? StartHoverPosition;
        private double TimeToNextStroke;

        protected override string ButtonImagePath => "Icons/IconGridSpray.png";

        [SerializeField]
        private SprayBrushSettings Settings = new SprayBrushSettings();

        private List<GameObject> PlacedGameObjects = new List<GameObject>();
        private IGenerationBounds GenerationBounds;

        public SprayBrush()
        {
            GenerationBounds = new CircleBrush.CircleGenerationBounds();
        }

        public override void DrawBrushEditor(ScenePlacer placer)
        {
            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(KalderaEditorUtils.BrushSizeContent, GUILayout.Width(KalderaEditorUtils.OptionLabelWidth));
                var tmpBrushSize = EditorGUILayout.Slider(Settings.BrushSize, AreaBrushBase.BrushSizeMin, AreaBrushBase.BrushSizeMax);
                if (tmpBrushSize != Settings.BrushSize) {
                    SetBrushSize(tmpBrushSize, placer);
                }
            }

            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(KalderaEditorUtils.ObjectDensityContent, GUILayout.Width(KalderaEditorUtils.OptionLabelWidth));
                var tmpBrushSpacing = EditorGUILayout.Slider(Settings.ObjectDensity, AreaBrushBase.BrushSpacingMin, AreaBrushBase.BrushSpacingMax);
                if (tmpBrushSpacing != Settings.ObjectDensity) {
                    Settings.ObjectDensity = tmpBrushSpacing;
                    placer.NotifyChange();
                }
            }

            using (new EditorGUILayout.HorizontalScope()) {
                EditorGUILayout.LabelField(KalderaEditorUtils.SprayIntensityContent, GUILayout.Width(KalderaEditorUtils.OptionLabelWidth));
                var tmpSprayIntensity = EditorGUILayout.Slider(Settings.SprayIntensity, SprayIntensityMin, SprayIntensityMax);
                if (tmpSprayIntensity != Settings.SprayIntensity) {
                    Settings.SprayIntensity = tmpSprayIntensity;
                    placer.NotifyChange();
                }
            }
        }

        protected override List<Vector3> GetPlacementOffsetValues(Vector3 position, SelectionSettings selectionSettings, ScenePlacer placer)
        {
            var spacing = selectionSettings.GetSelectedItemSize() * (1f / Settings.ObjectDensity);
            return new RandomDistribution.GeneratedPositions(Settings.BrushSize, spacing, GenerationBounds).GetPoints();
        }

        protected override List<PlacementInformation> PlacementsToPlace(ScenePlacer placer)
        {
            return new List<PlacementInformation> { placer.PlacementCollection.Placements[Random.Range(0, placer.PlacementCollection.Placements.Count)] };
        }

        public override void DrawBrushHandle(Vector3 placementPosition, Vector3 mousePosition)
        {
            Handles.color = HandleBrushColor;
            Handles.DrawSolidDisc(placementPosition, Normal, Settings.BrushSize);

            Handles.color = HandleOutlineColor;
            Handles.DrawWireDisc(placementPosition, Normal, Settings.BrushSize);
        }

        public override bool HandleKeyEvents(Event currentEvent, ScenePlacer placer)
        {
            if (currentEvent.type == EventType.KeyDown) {
                foreach (var preset in AreaBrushSettings.BrushSizePresets) {
                    if (preset.EventMatch(currentEvent)) {
                        SetBrushSize(preset.BrushSize, placer);
                        return true;
                    }
                }
            }

            return false;
        }

        public override void MoveBrush(Vector3 position, Vector3 brushNormal, ScenePlacer placer)
        {
            base.MoveBrush(position, brushNormal, placer);

            placer.RotatatePlacement(Rotation, Normal);

            StartHoverPosition = null;
        }

        public override void StartPlacement(Vector3 position, ScenePlacer placer)
        {
            TimeToNextStroke = 0;
            PlacedGameObjects.Clear();
            base.StartPlacement(position, placer);
        }

        public override void ShiftHoverPlacement(Vector3 position, SelectionSettings settings, double deltaTime, ScenePlacer placer)
        {
            if ((placer.PlacementPosition - position).magnitude < MinDragDistance) {
                StartHoverPosition = null;
                return;
            }

            if (!StartHoverPosition.HasValue) {
                StartHoverPosition = position;
            }
        }

        public override void ActiveDragPlacement(Vector3 worldPosition, SelectionSettings settings, double deltaTime, ScenePlacer placer)
        {
            SprayPlacement(worldPosition, settings, deltaTime, placer);
        }

        public override void StaticDragPlacement(Vector3 position, SelectionSettings settings, double deltaTime, ScenePlacer placer)
        {
            SprayPlacement(position, settings, deltaTime, placer);
        }

        private void SprayPlacement(Vector3 worldPosition, SelectionSettings settings, double deltaTime, ScenePlacer placer)
        {
            var placeObjectInterval = GetObjectIntervalInSeconds();
            placer.MovePosition(placer.ScreenPosition, worldPosition);
            TimeToNextStroke += deltaTime;

            while (TimeToNextStroke > 0) {
                PlaceObject(worldPosition, settings, placer);
                TimeToNextStroke -= placeObjectInterval;
            }

            placer.GeneratePlacement();
        }

        private void PlaceObject(Vector3 worldPosition, SelectionSettings settings, ScenePlacer placer)
        {
            PlacedGameObjects.AddRange(PlaceObjects(worldPosition, null, settings, placer));
        }

        private float GetObjectIntervalInSeconds() => 1f / (Settings.SprayIntensity * GetBrushSize());

        public override List<GameObject> EndPlacement(Vector3 position, GameObject parentCollider, SelectionSettings settings, ScenePlacer placer)
        {
            StartHoverPosition = null;
            return PlacedGameObjects;
        }

        private void SetBrushSize(float size, ScenePlacer placer)
        {
            Settings.BrushSize = size;
            placer.NotifyChange();
        }

        public float GetBrushSize() => Mathf.Pow(Settings.BrushSize * 0.1f, 2) * Mathf.PI;
    }
}