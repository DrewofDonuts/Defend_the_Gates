using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace DFun.Bookmarks
{
    [InitializeOnLoad]
    public class ComponentHighlighter
    {
        private static readonly List<HighlightComponentData> ScheduledHighlights = new List<HighlightComponentData>(1);
        private static readonly List<ComponentHighlightAction> ActiveHighlights = new List<ComponentHighlightAction>(1);

        private static readonly List<ComponentHighlightAction>
            HighlightsToClear = new List<ComponentHighlightAction>(1);

        private static float _lastUpdateTime;

        private static PropertyInfo _objectTrackerPropertyInfo;
        private static PropertyInfo ObjectTrackerPropertyInfo
        {
            get
            {
                if (_objectTrackerPropertyInfo == null)
                {
                    Assembly editorAssembly = typeof(Editor).Assembly;
                    Type inspectorWindowType = editorAssembly.GetType("UnityEditor.InspectorWindow");
                    _objectTrackerPropertyInfo = inspectorWindowType.GetProperty("tracker");
                }

                return _objectTrackerPropertyInfo;
            }
        }

        static ComponentHighlighter()
        {
            EditorApplication.update += Update;
        }

        public static void HighlightComponent(GameObject componentParent, Component component)
        {
            ScheduledHighlights.Add(new HighlightComponentData(componentParent, component));
        }

        private static void Update()
        {
            float dt = Time.realtimeSinceStartup - _lastUpdateTime;
            _lastUpdateTime = Time.realtimeSinceStartup;
            HandleScheduledHighlights(dt);
            HandleActiveHighlights(dt);
        }

        private static void HandleScheduledHighlights(float dt)
        {
            for (int i = ScheduledHighlights.Count - 1; i >= 0; i--)
            {
                HighlightComponentData scheduledHighlight = ScheduledHighlights[i];
                if (scheduledHighlight.ReadyToHighlight)
                {
                    TryHighlightComponent(scheduledHighlight);
                    ScheduledHighlights.RemoveAt(i);
                }
                else
                {
                    //skip one update and after that start to show highlight
                    scheduledHighlight.ReadyToHighlight = true;
                }
            }
        }

        private static void HandleActiveHighlights(float dt)
        {
            for (int i = ActiveHighlights.Count - 1; i >= 0; i--)
            {
                ComponentHighlightAction highlightAction = ActiveHighlights[i];
                highlightAction.Update(dt);
                if (highlightAction.IsFinished)
                {
                    ActiveHighlights.RemoveAt(i);
                }
            }
        }

        private static void CancelActiveHighlights(EditorWindow targetInspector)
        {
            HighlightsToClear.Clear();

            for (int i = 0, iSize = ActiveHighlights.Count; i < iSize; i++)
            {
                ComponentHighlightAction highlight = ActiveHighlights[i];
                if (highlight.ParentParentInspector != targetInspector)
                {
                    continue;
                }

                highlight.Finish();
                HighlightsToClear.Add(highlight);
            }

            for (int i = 0, iSize = HighlightsToClear.Count; i < iSize; i++)
            {
                ActiveHighlights.Remove(HighlightsToClear[i]);
            }
        }

        private static void TryHighlightComponent(HighlightComponentData highlightComponentData)
        {
            if (!highlightComponentData.IsValid())
            {
                return;
            }

            Object[] views = Resources.FindObjectsOfTypeAll(typeof(EditorWindow));
            for (int i = 0, iSize = views.Length; i < iSize; i++)
            {
                if (views[i] is EditorWindow editorWindow
                    && editorWindow.GetType().FullName == "UnityEditor.InspectorWindow")
                {
                    TryHighlightComponent(
                        highlightComponentData.componentParent, highlightComponentData.component, editorWindow
                    );
                }
            }
        }

        private static void TryHighlightComponent(
            GameObject gameObject, Component targetComponent, EditorWindow inspectorWindow)
        {
            if (!IsInspectorOfObject(inspectorWindow, gameObject))
            {
                return;
            }

            List<VisualElement> allInspectors = ComponentsFinder.FindComponentsInspectors(
                inspectorWindow.rootVisualElement
            );
            List<VisualElement> targetInspectors = ComponentsFinder.FindInspectorsForComponent(
                allInspectors, targetComponent
            );

            if (targetInspectors.Count == 0)
            {
                return;
            }

            Component[] components = gameObject.GetComponents(targetComponent.GetType());
            int componentIndex = Array.IndexOf(components, targetComponent);

            if (componentIndex < 0)
            {
                return;
            }

            if (componentIndex > targetInspectors.Count - 1)
            {
                return;
            }

            VisualElement targetInspector = targetInspectors[componentIndex];

            CancelActiveHighlights(inspectorWindow);
            ComponentHighlightAction highlightAction = new ComponentHighlightAction(targetInspector, inspectorWindow);
            if (highlightAction.IsValid)
            {
                ActiveHighlights.Add(highlightAction);
            }
        }

        private static bool IsInspectorOfObject(EditorWindow inspectorWindow, GameObject targetObject)
        {
            try
            {
                ActiveEditorTracker activeEditorTracker = (ActiveEditorTracker) ObjectTrackerPropertyInfo.GetValue(
                    inspectorWindow
                );
                Editor[] activeEditors = activeEditorTracker.activeEditors;
                if (activeEditors.Length == 0)
                {
                    return false;
                }

                Editor gameObjectEditor = activeEditors[0];
                Object editorObject = gameObjectEditor.target;
                if (editorObject == null)
                {
                    return false;
                }

                return targetObject == editorObject;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void PrintVisualElementStructure(VisualElement ve)
        {
            StringBuilder sb = new StringBuilder();
            BuildVisualElementStructure(ve, 0, sb);
            Debug.Log(sb);
        }

        private static void BuildVisualElementStructure(VisualElement ve, int indentLevel, StringBuilder result)
        {
            for (int i = 0, iSize = indentLevel; i < iSize; i++)
            {
                result.Append("-");
            }

            result.Append(" ").Append(ve.name).AppendLine();

            for (int i = 0; i < ve.childCount; i++)
            {
                VisualElement child = ve[i];
                BuildVisualElementStructure(child, indentLevel + 1, result);
            }
        }
    }
}