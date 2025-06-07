using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DFun.Bookmarks
{
    public static class ComponentsFinder
    {
        private const string HeaderNameEnding = "Header";
        private const string FooterNameEnding = "Footer";

        private static readonly List<VisualElement> AllInspectors = new List<VisualElement>(0);
        private static readonly List<VisualElement> TargetInspectors = new List<VisualElement>(0);

        public static List<VisualElement> FindComponentsInspectors(VisualElement root)
        {
            AllInspectors.Clear();
            FindAndAddComponentInspectors(root);

            return AllInspectors;
        }

        private static void FindAndAddComponentInspectors(VisualElement ve)
        {
            if (IsComponentInspector(ve))
            {
                AllInspectors.Add(ve);
                return;
            }

            for (int i = 0; i < ve.childCount; i++)
            {
                FindAndAddComponentInspectors(ve[i]);
            }
        }

        private static bool IsComponentInspector(VisualElement ve)
        {
            if (ve == null)
            {
                return false;
            }

            if (ve.name == null)
            {
                return false;
            }

            if (ve.childCount < 2)
            {
                return false;
            }

            return IsHeader(ve[0]) && IsFooter(GetFooter(ve));
        }

        public static VisualElement GetHeader(VisualElement ve)
        {
            return ve[0];
        }

        public static VisualElement GetFooter(VisualElement ve)
        {
            return ve[ve.childCount - 1];
        }

        private static bool IsHeader(VisualElement ve)
        {
            return IsNameEndsWith(ve, HeaderNameEnding);
        }

        private static bool IsFooter(VisualElement ve)
        {
            return IsNameEndsWith(ve, FooterNameEnding);
        }

        private static bool IsNameEndsWith(VisualElement ve, string targetEnding)
        {
            if (ve == null || ve.name == null)
            {
                return false;
            }

            return ve.name.EndsWith(targetEnding);
        }

        public static List<VisualElement> FindInspectorsForComponent(
            List<VisualElement> allInspectors, Component targetComponent)
        {
            TargetInspectors.Clear();

            string targetInspectorName = targetComponent.GetType().Name;
            //skip 1st inspector- usually it's an invisible game object inspector 
            for (int i = 1, iSize = allInspectors.Count; i < iSize; i++)
            {
                if (IsTargetInspector(allInspectors[i], targetInspectorName))
                {
                    TargetInspectors.Add(allInspectors[i]);
                }
            }

            return TargetInspectors;
        }

        private static bool IsTargetInspector(VisualElement inspector, string targetInspectorName)
        {
            if (inspector == null)
            {
                return false;
            }

            if (inspector.name == null)
            {
                return false;
            }

            return inspector.name.Replace(" ", string.Empty).Contains(targetInspectorName)
                   && HasTargetInspectorHeader(inspector, targetInspectorName);
        }

        private static bool HasTargetInspectorHeader(VisualElement inspector, string targetInspectorName)
        {
            if (inspector.childCount < 1)
            {
                return false;
            }

            VisualElement headerCandidate = inspector[0];
            if (headerCandidate == null)
            {
                return false;
            }

            string headerCandidateName = headerCandidate.name;
            if (headerCandidateName == null)
            {
                return false;
            }

            string normalizedHeaderName = headerCandidateName
                .Replace(" ", string.Empty)
                .Replace("(Script)", string.Empty);

            string targetHeaderName = targetInspectorName + HeaderNameEnding;

            return string.Equals(normalizedHeaderName, targetHeaderName, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}